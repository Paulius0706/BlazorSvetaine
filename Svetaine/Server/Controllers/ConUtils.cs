using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Svetaine.Shared.DbModels;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Svetaine.Server.Controllers
{
    public static class ConUtils
    {
        public static DbSet<T> GetTable<T>(this ServerDbContext dbContext) where T : class
        {
            return dbContext.GetType().GetProperty(typeof(T).Name).GetValue(dbContext) as DbSet<T>;
        }



        // obsolete
        public static int GetId<T>(this object entity) where T : class
        {
            return Convert.ToInt32(typeof(T).GetProperty("Id").GetValue(entity));
        }



        public static T FirstOrDefaultById<T>(this IQueryable<T> table, int id) where T : class
        {
            return table.FirstOrDefault(o => EF.Property<int>(o, "Id") == id);
        }


        public static IQueryable<T2> WhereBoundEntity<T1, T2>(this IQueryable<T2> table, T1 entity) where T1 : class where T2 : class
        {
            return table.Where(o => EF.Property<T1>(o, typeof(T1).Name) == entity).AsQueryable();
        }




    }

}

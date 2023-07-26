using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Svetaine.Server.Controllers;
using Svetaine.Server.Helpers;
using Svetaine.Shared.DbModels;
using System.Reflection;

namespace Svetaine.Server
{
    [ApiController]
    public class Entity2Controller<T1,T2> : ControllerBase  
        where T1 : class 
        where T2 : class
    {
        public ServerDbContext dbContext { get; private set; }
        public IValid Valid { get; private set; }
        public Entity2Controller(ServerDbContext dbContext, IValid Valid)
        {
            this.dbContext = dbContext;
            this.Valid = Valid;
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<T2>>> Get(int T1Id)
        {
            T1 t1 = dbContext.GetTable<T1>().FirstOrDefaultById(T1Id);
            if (t1 == null) BadRequest(typeof(T1).Name + "with Id:" + T1Id + " dont exist");

            return dbContext.GetTable<T2>().WhereBoundEntity(t1).ToList();
        }
        [HttpGet("{Id}")]
        public virtual async Task<ActionResult<T2>> Get(int T1Id, int Id)
        {
            T1 t1 = dbContext.GetTable<T1>().FirstOrDefaultById(T1Id);
            if (t1 == null) BadRequest(typeof(T1).Name + "with Id:" + T1Id + " dont exist");

            T2 t2 = dbContext.GetTable<T2>().WhereBoundEntity(t1).FirstOrDefaultById(Id);
            if (t2 == null) BadRequest(typeof(T2).Name + "with Id:" + Id + " dont exist");

            return t2;
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post(int T1Id, [FromBody] T2 entity)
        {
            if (!Valid.Valid(out string error, entity)) return BadRequest(error);

            T1 t1 = dbContext.GetTable<T1>().FirstOrDefaultById(T1Id);
            if (t1 == null) BadRequest(typeof(T1).Name + "with Id:" + T1Id + " dont exist");

            typeof(T2).GetProperty(typeof(T1).Name).SetValue(entity, t1);

            try
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync();
                return Ok(entity.GetId<T2>());

            }
            catch { return Ok(-1); }
        }
        [HttpDelete("{Id}")]
        [HttpPut]
        public virtual async Task<ActionResult> Put(int T1Id, int Id, [FromBody] T2 entity)
        {
            if (!Valid.Valid(out string error, entity)) return BadRequest(error);

            T1 t1 = dbContext.GetTable<T1>().FirstOrDefaultById(T1Id);
            if (t1 == null) BadRequest(typeof(T1).Name + "with Id:" + T1Id + " dont exist");

            T2 t2 = dbContext.GetTable<T2>().WhereBoundEntity(t1).FirstOrDefaultById(Id);
            if (t2 == null) BadRequest(typeof(T2).Name + "with Id:" + Id + " dont exist");

            if (entity.GetId<T2>() != Id) BadRequest(typeof(T2).Name + "entity Id:" + Id + " is not tha same as url");

            try
            {
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
            }
            catch { return Ok(false); }
            return Ok(true);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int T1Id,int Id)
        {
            T1 t1 = dbContext.GetTable<T1>().FirstOrDefaultById(T1Id);
            if (t1 == null) BadRequest(typeof(T1).Name + "with Id:" + T1Id + " dont exist");

            T2 t2 = dbContext.GetTable<T2>().WhereBoundEntity(t1).FirstOrDefaultById(Id);
            if (t2 == null) BadRequest(typeof(T2).Name + "with Id:" + Id + " dont exist");

            try
            {
                dbContext.Remove(t2);
                await dbContext.SaveChangesAsync();
            }
            catch { return BadRequest(); }
            return Ok();
        }
    }
}

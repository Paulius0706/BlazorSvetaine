using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Svetaine.Server.Controllers;
using Svetaine.Server.Helpers;
using Svetaine.Shared.DbModels;

namespace Svetaine.Server
{
    [ApiController]
    public class EntityController<T> : ControllerBase where T : class
    {
        public ServerDbContext dbContext { get; private set; }
        public IValid Valid { get; private set; }
        public EntityController(ServerDbContext dbContext, IValid Valid)
        {
            this.dbContext = dbContext;
            this.Valid = Valid;
        }

        [HttpGet]
        public virtual async Task<ActionResult<List<T>>> Get()
        {
            return dbContext.GetTable<T>().ToList();
        }
        [HttpGet("{Id}")]
        public virtual async Task<ActionResult<T>> Get(int Id)
        {
            return dbContext.GetTable<T>().FirstOrDefaultById(Id);// AsEnumerable().FirstOrDefault(o => o.GetId<T>() == Id);
        }

        [HttpPost]
        public virtual async Task<ActionResult> Post([FromBody] T entity)
        {
            if (!Valid.Valid(out string error, entity)) return BadRequest(error);
            try
            {
                dbContext.Add(entity);
                await dbContext.SaveChangesAsync();
                return Ok(entity.GetId<T>());

            }
            catch{ return Ok(-1); }
        }
        [HttpPut("{Id}")]
        public virtual async Task<ActionResult> Put(int Id, [FromBody] T entity)
        {
            if (!Valid.Valid(out string error, entity)) return BadRequest(error);
            try
            {
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
            }
            catch { return Ok(false); }
            return Ok(true);
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                dbContext.Remove(dbContext.GetTable<T>().FirstOrDefaultById(Id));
                await dbContext.SaveChangesAsync();
            }
            catch { return BadRequest(); }
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Svetaine.Server.Helpers;
using Svetaine.Shared.DbModels;

namespace Svetaine.Server.Controllers
{
    public class ReserveTimeValid : IValid
    {
        public bool Valid(out string error, object entity)
        {
            error = "";
            ReserveTime time = (ReserveTime)entity;
            if (time.Day < 2000_00_00) error = "date is invalid";
            if (time.TimeIndex < 6 * 6) error = "time is invalid (to small)";
            if (time.TimeIndex > 22 * 6) error = "time is invalid (to big)";
            return String.IsNullOrEmpty("");
        }
    }
    [ApiController]
    [Route("api/User/{T1Id}/[controller]")]
    public class ReserveTimeController : Entity2Controller<User,ReserveTime>
    {
        public ReserveTimeController(ServerDbContext dbContext) : base(dbContext,new ReserveTimeValid())
        {
        }
    }
}

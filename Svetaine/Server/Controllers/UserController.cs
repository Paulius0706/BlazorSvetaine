using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Svetaine.Server.Helpers;
using Svetaine.Shared;
using Svetaine.Shared.DbModels;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Svetaine.Server.Controllers
{
    public class UserValid : IValid
    {
        public bool Valid(out string error, object entity)
        {
            error = "";
            User user = (User)entity;
            if (String.IsNullOrEmpty(user.Name)) { error += "Name field is empty\n"; }
            if (String.IsNullOrEmpty(user.Email)) { error += "Email field is empty\n"; }
            if (String.IsNullOrEmpty(user.Password)) { error += "Password field is empty\n"; }
            return String.IsNullOrEmpty(error);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : EntityController<User>
    {
        public UserController(ServerDbContext dbContext) :base(dbContext, new UserValid())
        {

        }
    }
}

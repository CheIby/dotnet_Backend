using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DTO;
using server.Entities;
using System.Net;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MydbContext MydbContext;

        public UserController(MydbContext MydbContext)
        {
            this.MydbContext = MydbContext;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            var List = await MydbContext.Users.Select(
                s => new UserDTO
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    Username = s.Username,
                    Password = s.Password,
                    Score = s.Score
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int Id)
        {
            UserDTO User = await MydbContext.Users.Select(s => new UserDTO
            {
                Id = s.Id,
                UserId = s.UserId,
                Username = s.Username,
                Password = s.Password,
                Score = s.Score
            }).FirstOrDefaultAsync(s => s.Id == Id);
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpPost("InsertUser")]
        public async Task<HttpStatusCode> InsertUser(UserDTO User)
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            var entity = new User()
            {
                UserId = myuuidAsString,
                Username = User.Username,
                Password = User.Password,
                Score = 0
            };
            MydbContext.Users.Add(entity);
            await MydbContext.SaveChangesAsync();
            return HttpStatusCode.Created;
        }

        // [HttpPut("UpdateUser")]
        // public async Task<HttpStatusCode> UpdateUser(UserDTO User)
        // {
        //     var entity = await MydbContext.Users.FirstOrDefaultAsync(s => s.Id == User.Id);
        //     entity.FirstName = User.FirstName;
        //     entity.LastName = User.LastName;
        //     entity.Username = User.Username;
        //     entity.Password = User.Password;
        //     entity.EnrollmentDate = User.EnrollmentDate;
        //     await MydbContext.SaveChangesAsync();
        //     return HttpStatusCode.OK;
        // }

        // [HttpDelete("DeleteUser/{Id}")]
        // public async Task<HttpStatusCode> DeleteUser(int Id)
        // {
        //     var entity = new User()
        //     {
        //         Id = Id
        //     };
        //     MydbContext.Users.Attach(entity);
        //     MydbContext.Users.Remove(entity);
        //     await MydbContext.SaveChangesAsync();
        //     return HttpStatusCode.OK;
        // }
    }
}

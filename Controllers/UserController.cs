using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DTO;
using server.Entities;
using System.Net;
using System.IO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MydbContext MydbContext;
        private readonly IConfiguration Configuration;

        public UserController(MydbContext MydbContext, IConfiguration Configuration)
        {
            this.MydbContext = MydbContext;
            this.Configuration = Configuration;
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

            var sorted = List.OrderBy(e => e.Score).ToList();

            if (sorted.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return sorted;
            }
        }

        [HttpPatch("[action]/{UserId}")]
        public async Task<HttpStatusCode> UpdateUser(int UserId,[FromForm] UpdateUserDTO updateUser){
            var foundUser = await MydbContext.Users.FindAsync(UserId);
            if (User is null){
                return HttpStatusCode.NotFound;
            }
            bool match = BCrypt.Net.BCrypt.Verify(updateUser.Password, foundUser.Password);
            if (match){
                try{
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(),"static",updateUser.Image.FileName);
                    await updateUser.Image.CopyToAsync(new FileStream(filepath, FileMode.Create));
                    foundUser.Username = updateUser.Username;
                    foundUser.UserImg=  updateUser.Image.FileName.ToString();
                    await MydbContext.SaveChangesAsync();
                    return HttpStatusCode.OK;
                 }catch(Exception err){
                return HttpStatusCode.BadRequest;
                }
            }
            return HttpStatusCode.Unauthorized;
        }

        [HttpGet("[action]/{UserId}")]
        public async Task<IActionResult> GetUserById(string UserId)
        {   
            UserDTO User = await MydbContext.Users.Select(s => new UserDTO
            {
                Id = s.Id,
                UserId = s.UserId,
                Username = s.Username,
                Password = s.Password,
                Score = s.Score,
                UserImg = s.UserImg
            }).FirstOrDefaultAsync(s => s.UserId == UserId);
            if (User == null)
            {
                return NotFound();
            }
            else
            {
                var userInfo = new GetUserInfoDTO();
                userInfo.Id = User.Id;
                userInfo.Username = User.Username;
                userInfo.Score = User.Score;
                userInfo.UserImg = User.UserImg;
                return Ok(userInfo);
            }
        }

        // [HttpPost("UploadIMG")]
        // public async Task<HttpStatusCode> upload(IFormFile Image){
        //     try{
        //         var filepath = Path.Combine(Directory.GetCurrentDirectory(),"static",Image.FileName);
        //         await Image.CopyToAsync(new FileStream(filepath, FileMode.Create));
        //         return HttpStatusCode.OK;
        //     }catch(Exception err){
        //         return HttpStatusCode.BadRequest;
        //     }
           
        // }

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

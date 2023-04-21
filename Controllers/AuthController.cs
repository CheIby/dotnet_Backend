using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.DTO;
using server.DTO.User;
using server.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MydbContext MydbContext;
        private readonly IConfiguration Configuration;

        public AuthController(MydbContext MydbContext, IConfiguration Configuration)
        {
            this.MydbContext = MydbContext;
            this.Configuration = Configuration;
        }

        [HttpPost("Register")]
        public async Task<HttpStatusCode> Register(AddUserDTO User)
        {
            if(User.Password.Length==0 || User.Username.Length==0){
                return HttpStatusCode.BadRequest;
            }
            try{
                Guid myuuid = Guid.NewGuid();
                string myuuidAsString = myuuid.ToString();
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password);
                var entity = new User()
                {
                    UserId = myuuidAsString,
                    Username = User.Username,
                    Password = passwordHash,
                    Score = 0,
                    UserImg = "nullUser.png"
                };
                MydbContext.Users.Add(entity);
                await MydbContext.SaveChangesAsync();
                return HttpStatusCode.Created;
            }catch{
                return HttpStatusCode.InternalServerError;
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDTO User)
        {
            var foundUser = await MydbContext.Users.FirstOrDefaultAsync(e => e.Username == User.Username);
            if (foundUser == null){
                return BadRequest("Invalid user request!!!");
            }
            bool match = BCrypt.Net.BCrypt.Verify(User.Password, foundUser.Password);
            if (match)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]{
                    new Claim("Id", foundUser.Id.ToString()),
                    new Claim("UserId", foundUser.UserId.ToString()),
                    new Claim("Username", foundUser.Username.ToString()),
                    new Claim("Score", foundUser.Score.ToString()),
                    new Claim("UserImg", foundUser.UserImg.ToString())
                    };
                var tokeOptions = new JwtSecurityToken(
                    issuer: Configuration["JWT:Issuer"],
                    audience: Configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new TokenDTO
                {
                    Token = tokenString,
                });
            }
            else
            {
                return Unauthorized("Wong Password");
            }
        }
    }
}

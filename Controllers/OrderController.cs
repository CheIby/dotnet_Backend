using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.DTO;
using server.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly MydbContext MydbContext;

        public OrderController(MydbContext MydbContext){
            this.MydbContext = MydbContext;
        }

        [HttpGet("GetOrders")]
        // [Authorize]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrder(){
            // string authHeader = Request.Headers["Authorization"];
            // string[] lstAuthHeader = authHeader.Split(" "); 
            // var token = lstAuthHeader[1];
            // JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(token);
            // var claims = decodedToken.Claims;
            // string myVariable = claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            // Console.WriteLine(myVariable);
            var lst = await MydbContext.Orders.Select(
                e => new OrderDTO
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    OrderId = e.OrderId,
                    PiorityScore = e.PiorityScore,
                    Restaurant = e.Restaurant,
                    Detail = e.Detail,
                    ReceiveLocation = e.ReceiveLocation
                }
            ).ToListAsync();

            var sorted = lst.OrderByDescending(e => e.PiorityScore).ToList();

            if (sorted.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return sorted;
            }
        }

        [HttpPost("AddOrder")]
        public async Task<HttpStatusCode> CreateOrder(AddOrderDTO Order){
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            try{
                var newOrder = new Order()
                    {
                        UserId = Order.UserId,
                        OrderId = myuuidAsString,
                        PiorityScore = Order.PiorityScore,
                        Restaurant = Order.Restaurant,
                        Detail = Order.Detail,
                        ReceiveLocation = Order.ReceiveLocation
                    };
                MydbContext.Orders.Add(newOrder);
                await MydbContext.SaveChangesAsync();
                return HttpStatusCode.Created;
            }catch{
                return HttpStatusCode.BadRequest;
            }
        }
    }
}

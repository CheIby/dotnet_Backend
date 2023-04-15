namespace server.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public string? OrderId { get; set; } 

        public string? Restaurant { get; set; }

        public string? Detail { get; set; } 

        public string? ReceiveLocation { get; set; }
    }
}

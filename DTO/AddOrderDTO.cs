namespace server.DTO
{
    public class AddOrderDTO
    {
        public string? UserId { get; set; } 

        public int PiorityScore {get; set;}

        public string? Restaurant { get; set; }

        public string? Detail { get; set; } 

        public string? ReceiveLocation { get; set; }
    }
}

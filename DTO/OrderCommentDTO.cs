namespace server.DTO
{
    public class OrderCommentDTO
    {
        public int Id { get; set; }

        public string? UserId { get; set; } 

        public string? OrderId { get; set; } 

        public string? Comment { get; set; } 
    }
}

namespace server.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int Score { get; set; }
        public string? UserImg {get;set;}
    }
}

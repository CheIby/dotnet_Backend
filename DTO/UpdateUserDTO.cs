namespace server.DTO
{
    public class UpdateUserDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public IFormFile? Image{get;set;}
    }
}
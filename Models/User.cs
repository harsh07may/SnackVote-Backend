namespace SnackVote_Backend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();     
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string UserRole { get; set; } = string.Empty;
    }
}

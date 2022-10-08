namespace SnackVote_Backend.Models
{
    public class Menu
    {
        public int MenuId { get; set; } 
        public string MenuName { get; set; } = string.Empty;
        public string MenuDescription { get; set; } = string.Empty;
        public string MenuCategory { get; set; } = string.Empty;
        public byte[] MenuImage { get; set; } = Array.Empty<byte>();
    }
}

namespace SnackVote_Backend.DTO
{
    public class MenuUploadDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } = null;
    }
}

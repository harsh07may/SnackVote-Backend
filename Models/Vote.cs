namespace SnackVote_Backend.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}

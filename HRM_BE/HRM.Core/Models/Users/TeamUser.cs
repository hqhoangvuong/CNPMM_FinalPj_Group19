namespace HRM.Core.Models.Users
{
    public class TeamUser
    {
        public string TeamId { get; set; }
        public virtual Team Team { get; set; }
        public string UserId { get; set; }
    }
}

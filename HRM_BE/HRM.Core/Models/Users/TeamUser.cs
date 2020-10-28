namespace HRM.Core.Models.Users
{
    public class TeamUser
    {
        public string Id { get; set; }
        public string TeamId { get; set; }
        public string UserId { get; set; }
        public virtual Team Team { get; set; }
    }
}

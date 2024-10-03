namespace ChatAppMVC.DataAccessLayer.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        [ForeignKey("User1")]
        public string User1Id { get; set; }
        public ApplicationUser User1 { get; set; }
        [ForeignKey("User2")]
        public string User2Id { get; set; }
        public ApplicationUser User2 { get; set; }
    }
}

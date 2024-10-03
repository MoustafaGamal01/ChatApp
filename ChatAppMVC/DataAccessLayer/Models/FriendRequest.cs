namespace ChatAppMVC.DataAccessLayer.Models
{
    public enum RequestStatus
    {
        Pending,
        Accepted
    }
    public class FriendRequest
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
        [Required]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [Required]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}

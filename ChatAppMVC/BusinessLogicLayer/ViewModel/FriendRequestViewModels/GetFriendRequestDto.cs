namespace ChatAppMVC.BusinessLogicLayer.Dtos.FriendRequestViewModels
{
    public class GetFriendRequestDto
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

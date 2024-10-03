namespace ChatAppMVC.DataAccessLayer.Models.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Friendship, FriendshipDto>();

            CreateMap<Message, GetMessageDto>()
            .ForMember(c => c.SenderName, opt => opt.MapFrom(c => c.User.UserName))
            .ForMember(m => m.Timestamp, opt => opt.MapFrom(m => m.SentAt))
;
            CreateMap<ApplicationUser, GetUserViewModel>();
               

            CreateMap<Notification, GetNotificationDto>()
               .ForMember(n => n.SenderName, opt => opt.MapFrom(n => n.Sender.UserName));

            CreateMap<FriendRequest, GetFriendRequestDto>()
                            .ForMember(fr => fr.SenderName, opt => opt.MapFrom(fr => fr.Sender.UserName))
                            .ForMember(fr => fr.SenderId, opt => opt.MapFrom(fr => fr.SenderId));

            CreateMap<ChatUser, GetChatUserViewModel>()
                .ForMember(c => c.ChatName, opt => opt.MapFrom(c => c.DisplayName))
                .ForMember(c => c.LastMessage, opt => opt.MapFrom(c => c.Chat.Messages.OrderBy(o => o.SentAt).LastOrDefault().Content))
                .ForMember(c => c.LastMessageTime, opt => opt.MapFrom(c => c.Chat.Messages.OrderBy(o => o.SentAt).LastOrDefault().SentAt.ToString()))
                .ForMember(c => c.ChatType, opt => opt.MapFrom(c => c.Chat.Type))
                .ForMember(c=>c.ChatPictureUrl, opt=>opt.MapFrom(c=>c.ChatPictureUrl));
        }
    }
}

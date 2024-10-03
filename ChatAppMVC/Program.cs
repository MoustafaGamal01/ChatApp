using ChatAppMVC.DataAccessLayer.UnitOfWork.IUnitOfWork;
using ChatAppMVC.DataAccessLayer.UnitOfWork;

namespace ChatAppMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationFormats.Add("/PresentationLayer/Views/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/PresentationLayer/Views/Shared/{0}.cshtml");
                });
            builder.Services.AddDbContext<ChatAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RemoteCS")));

            builder.Services.AddSignalR();
            
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddRazorPages();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                options => options.Password.RequireDigit = true)
                .AddEntityFrameworkStores<ChatAppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(option =>
                option.TokenLifespan = TimeSpan.FromHours(2));


            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IChatUserRepository, ChatUserRepository>();
            builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
            builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IChatUserService, ChatUserService>();

            builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapHub<FriendRequestHub>("/friendRequestHub");
            app.MapHub<ChatHub>("/chatHub");
            app.MapHub<NotificationHub>("/notificationHub");
            app.Run();
        }
    }
}

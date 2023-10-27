namespace Wheel.Email
{
    public static class MailKitExtensions
    {
        public static IServiceCollection AddMailKit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<MailKitOptions>(options =>
            {
                configuration.GetSection("MailKit").Bind(options);
            });
            return services;
        }
    }
}

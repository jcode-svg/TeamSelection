using WebApi.ConfigurationModels;

namespace WebApi.Extensions
{
    public static class ConfigurationExtension
    {
        public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSetting>(configuration.GetSection("AppSetting"));
        }
    }
}

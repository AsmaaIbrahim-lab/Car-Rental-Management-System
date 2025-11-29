namespace Car_rental_management_system.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplication configCORS ( this WebApplication app,IConfiguration config)
        {
            app.UseCors();
            return app;
        }
    }
}

using CCDMSServices.ORM.MigrationRunner;

namespace CCDMS_API.Extensions
{
    public  static class IApplicationBuilderExtensions
    {
        public static void ApplyPendingMigrations(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var instance = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                instance.ApplyPendingMigrations();
            }
        }
    }
}

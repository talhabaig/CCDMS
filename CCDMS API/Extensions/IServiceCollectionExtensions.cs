using CCDMSServices.ORM.Context;
using Microsoft.EntityFrameworkCore;

namespace CCDMS_API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseService(this IServiceCollection services,IConfiguration  configuration)
        {
            return
                     services.AddDbContext<CCDMSDbContext>(options =>
                     {
                         options.UseSqlite(configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(3600));
                     }, ServiceLifetime.Transient);
        }

    }
}

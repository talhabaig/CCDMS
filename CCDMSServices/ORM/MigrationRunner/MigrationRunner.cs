using CCDMSServices.ORM.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.ORM.MigrationRunner
{
    public class MigrationRunner : IMigrationRunner
    {
        private readonly CCDMSDbContext db;
        public MigrationRunner(CCDMSDbContext db)
        {
            this.db = db;
        }

        public void ApplyPendingMigrations()
        {
            try
            {
                if (db.Database.GetPendingMigrations().Any())
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}

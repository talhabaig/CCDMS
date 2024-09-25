using CCDMSServices.ORM.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.ORM.Context
{
    public class CCDMSDbContext : DbContext
    {
        public CCDMSDbContext(DbContextOptions<CCDMSDbContext> options)
           : base(options)
        {
          
        }
        public DbSet<Files> Files { get; set; }
        public DbSet<FileData> FileData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace MoneyManager.Data
{
    public sealed class DbContextSingleton
    {
        private static readonly Lazy<ApplicationDbContext> lazyInstance =
            new Lazy<ApplicationDbContext>(() => new ApplicationDbContext(CreateOptions()));

        public static ApplicationDbContext Instance => lazyInstance.Value;

        private DbContextSingleton() { }

        private static DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            optionsBuilder.UseNpgsql(connectionString);
            return optionsBuilder.Options;
        }
    }
}

namespace ISP.Data
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Define DbSet for each table in your database
        public DbSet<Atsiliepimas> Atsiliepimas { get; set; }
        public DbSet<Automobilis> Automobilis { get; set; }
        public DbSet<Naudotojas> Naudotojas { get; set; }
        public DbSet<Draudimas> Draudimas { get; set; }
    }
}

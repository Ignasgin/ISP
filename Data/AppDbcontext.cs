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
        public DbSet<Perziuretas_Automobilis> Perziuretas_Automobilis { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<Trumpalaike_Rezervacija> Trumpalaike_Rezervacija { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the one-to-many relationship between Automobilis and Draudimas
            modelBuilder.Entity<Draudimas>()
                .HasOne(d => d.Automobilis)
                .WithMany(a => a.Draudimai)
                .HasForeignKey(d => d.Fk_Automobilis_Id_Automobilis)
                .OnDelete(DeleteBehavior.Cascade);  // Ensure delete cascade

            modelBuilder.Entity<Perziuretas_Automobilis>()
                .HasOne(p => p.Automobilis)
                .WithMany(a => a.Perziuretas_Automobilis)
                .HasForeignKey(p => p.Fk_Automobilis_Id_Automobilis)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Automobilis)
                .WithMany()
                .HasForeignKey(r => r.Fk_Automobilis_Id_Automobilis)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trumpalaike_Rezervacija>()
                .HasOne(r => r.Automobilis)
                .WithMany()
                .HasForeignKey(r => r.Fk_Automobilis_Id_Automobilis)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

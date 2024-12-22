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
        public DbSet<Servisas> Servisas {get;set;}
        public DbSet<Paslauga> Paslauga {get;set;}
        public DbSet<Servisu_paslaugos> Servisu_paslaugos{get;set;}
        public DbSet<Pranesimas> Pranesimas { get; set; }



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
            
            modelBuilder.Entity<Automobilis>()
                .HasMany(x => x.Atsiliepimai)
                .WithOne(x => x.Automobilis)
                .HasForeignKey(x => x.Fk_Automobilis_Id_Automobilis)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Naudotojas>()
                .HasMany(x => x.Atsiliepimai)
                .WithOne(x => x.Naudotojas)
                .HasForeignKey(x => x.Fk_Naudotojas_Id_Naudotojas)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Naudotojas>()
                .HasMany(x => x.Perziureti_Automobiliai)
                .WithOne(x => x.Naudotojas)
                .HasForeignKey(x => x.Fk_Naudotojas_Id_Naudotojas)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Naudotojas>()
                .HasMany(x => x.pranesimas)
                .WithOne(x => x.naudotojas)
                .HasForeignKey(x => x.fk_Naudotojas_id_Naudotojas)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Servisu_paslaugos>()
        .HasKey(sp => new { sp.fk_Servisas_id_Servisas, sp.fk_Paslauga_id_Paslauga });

    // Nustatome santykį tarp Servisas ir Servisu_paslaugos
    modelBuilder.Entity<Servisu_paslaugos>()
        .HasOne(sp => sp.Service) // Servisas
        .WithMany(s => s.ServicePaslaugos) // Servisas turi daug paslaugų
        .HasForeignKey(sp => sp.fk_Servisas_id_Servisas); // Naudojame fk_Servisasid_Servisas kaip užsienio raktą

    // Nustatome santykį tarp Paslauga ir Servisu_paslaugos
    modelBuilder.Entity<Servisu_paslaugos>()
        .HasOne(sp => sp.Paslauga) // Paslauga
        .WithMany(p => p.ServicePaslaugos) // Paslauga turi daug servisų per tarpinę lentelę
        .HasForeignKey(sp => sp.fk_Paslauga_id_Paslauga); // Naudojame fk_Paslaugaid_Paslauga kaip užsienio raktą

    // Nustatome pirminius raktus Paslauga ir Servisas lentelėms
    modelBuilder.Entity<Paslauga>()
        .HasKey(p => p.id_Paslauga);

    modelBuilder.Entity<Servisas>()
        .HasKey(s => s.Id_Servisas);
        }
    }
}

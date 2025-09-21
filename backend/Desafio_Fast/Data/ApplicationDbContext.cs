using Microsoft.EntityFrameworkCore;
using Desafio_Fast.Models;

namespace Desafio_Fast.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<Ata> Atas { get; set; }
        public DbSet<AtaColaborador> AtaColaboradores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AtaColaborador>()
                .HasKey(ac => new { ac.AtaId, ac.ColaboradorId });

            modelBuilder.Entity<AtaColaborador>()
                .HasOne(ac => ac.Ata)
                .WithMany(a => a.AtaColaboradores)
                .HasForeignKey(ac => ac.AtaId);

            modelBuilder.Entity<AtaColaborador>()
                .HasOne(ac => ac.Colaborador)
                .WithMany(c => c.AtaColaboradores)
                .HasForeignKey(ac => ac.ColaboradorId);

            modelBuilder.Entity<Ata>()
                .HasOne(a => a.Workshop)
                .WithMany(w => w.Atas)
                .HasForeignKey(a => a.WorkshopId);

            modelBuilder.Entity<Colaborador>()
                .HasIndex(c => c.Nome);

            modelBuilder.Entity<Workshop>()
                .HasIndex(w => w.Nome);

            modelBuilder.Entity<Workshop>()
                .HasIndex(w => w.DataRealizacao);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
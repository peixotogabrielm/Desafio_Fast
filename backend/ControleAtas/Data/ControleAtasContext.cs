using ControleAtas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleAtas.Data;

public class ControleAtasContext : DbContext
{
    public ControleAtasContext(DbContextOptions<ControleAtasContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Workshop> Workshops => Set<Workshop>();

    public DbSet<Colaborador> Colaboradores => Set<Colaborador>();

    public DbSet<Ata> Atas => Set<Ata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Workshop>()
            .HasMany(w => w.Atas)
            .WithOne(a => a.Workshop)
            .HasForeignKey(a => a.WorkshopId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ata>()
            .HasMany(a => a.Colaboradores)
            .WithMany(c => c.Atas)
            .UsingEntity<Dictionary<string, object>>(
                "AtaColaborador",
                j => j
                    .HasOne<Colaborador>()
                    .WithMany()
                    .HasForeignKey("ColaboradorId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Ata>()
                    .WithMany()
                    .HasForeignKey("AtaId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("AtaId", "ColaboradorId");
                    j.ToTable("AtaColaboradores");
                });
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}

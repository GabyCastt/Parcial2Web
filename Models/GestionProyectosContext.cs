using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestionProyectoFINAL.Models;
using Microsoft.AspNetCore.Identity;

namespace GestionProyectoFINAL.Models
{
    public partial class GestionProyectosContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public GestionProyectosContext()
        {
        }

        public GestionProyectosContext(DbContextOptions<GestionProyectosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<Proyecto> Proyectos { get; set; }
        public virtual DbSet<Tarea> Tareas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost;database=Gestiiii;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Necesario para Identity

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE6F0CD42604C");
                entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.Puesto).HasMaxLength(100);
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.HasKey(e => e.ProyectoId).HasName("PK__Proyecto__CF241D459D062CF0");
                entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.FechaFin).HasColumnType("datetime");
                entity.Property(e => e.FechaInicio).HasColumnType("datetime");
                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.HasMany(d => d.Empleados).WithMany(p => p.Proyectos)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProyectoEmpleado",
                        r => r.HasOne<Empleado>().WithMany()
                            .HasForeignKey("EmpleadoId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ProyectoE__Emple__3C69FB99"),
                        l => l.HasOne<Proyecto>().WithMany()
                            .HasForeignKey("ProyectoId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__ProyectoE__Proye__3B75D760"),
                        j =>
                        {
                            j.HasKey("ProyectoId", "EmpleadoId").HasName("PK__Proyecto__D67CA32A6361E46D");
                            j.ToTable("ProyectoEmpleado");
                            j.IndexerProperty<int>("ProyectoId").HasColumnName("ProyectoID");
                            j.IndexerProperty<int>("EmpleadoId").HasColumnName("EmpleadoID");
                        });
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(e => e.TareaId).HasName("PK__Tareas__5CD8367150E2D8C5");
                entity.Property(e => e.TareaId).HasColumnName("TareaID");
                entity.Property(e => e.Descripcion).HasMaxLength(255);
                entity.Property(e => e.Estado).HasMaxLength(50);
                entity.Property(e => e.FechaFin).HasColumnType("datetime");
                entity.Property(e => e.FechaInicio).HasColumnType("datetime");
                entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");

                entity.HasOne(d => d.Proyecto).WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.ProyectoId)
                    .HasConstraintName("FK__Tareas__Proyecto__3F466844");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
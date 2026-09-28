using GestionAula.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionAula.Data.Data;

public partial class GestionAulaContext : DbContext
{
    public GestionAulaContext(DbContextOptions<GestionAulaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }
    public virtual DbSet<Grado> Grados { get; set; }
    public virtual DbSet<Materia> Materias { get; set; }
    public virtual DbSet<Matricula> Matriculas { get; set; }
    public virtual DbSet<Nota> Notas { get; set; }
    public virtual DbSet<ProfesorMaterium> ProfesorMateria { get; set; }
    public virtual DbSet<Profesore> Profesores { get; set; }
    public virtual DbSet<Seccione> Secciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.AlumnoId).HasName("PK__Alumnos__90A6AA137F604B72");
            entity.HasIndex(e => e.DNI, "UQ__Alumnos__C035B8DD5664C7F5").IsUnique();
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.DNI).HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Nombres).HasMaxLength(100);
        });

        modelBuilder.Entity<Grado>(entity =>
        {
            entity.HasKey(e => e.GradoId).HasName("PK__Grados__5A8DF6D4830C336F");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.MateriaId).HasName("PK__Materias__0D019DE12A5671A3");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.HasKey(e => e.MatriculaId).HasName("PK__Matricul__908CEE022B99EF90");
            entity.Property(e => e.AnioEscolar).HasDefaultValue(2025);

            entity.HasOne(d => d.Alumno).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.AlumnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matricula_Alumno");

            entity.HasOne(d => d.Grado).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.GradoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matricula_Grado");

            entity.HasOne(d => d.Seccion).WithMany(p => p.Matriculas)
                .HasForeignKey(d => d.SeccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Matricula_Seccion");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(e => e.NotaId).HasName("PK__Notas__EF36CC1A03AD1236");
            entity.HasIndex(e => new { e.AlumnoId, e.MateriaId }, "UQ_Alumno_Materia").IsUnique();
            entity.Property(e => e.Parcial1).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Parcial2).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Parcial3).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Parcial4).HasColumnType("decimal(4, 2)");

            entity.HasOne(d => d.Alumno).WithMany(p => p.Nota)
                .HasForeignKey(d => d.AlumnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notas_Alumno");

            entity.HasOne(d => d.Materia).WithMany(p => p.Nota)
                .HasForeignKey(d => d.MateriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notas_Materia");
        });

        modelBuilder.Entity<ProfesorMaterium>(entity =>
        {
            entity.HasKey(e => e.ProfesorMateriaId).HasName("PK__Profesor__731A639F67521350");
            entity.HasIndex(e => new { e.ProfesorId, e.MateriaId }, "UQ_Profesor_Materia").IsUnique();

            entity.HasOne(d => d.Materia).WithMany(p => p.ProfesorMateria)
                .HasForeignKey(d => d.MateriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfMat_Materia");

            entity.HasOne(d => d.Profesor).WithMany(p => p.ProfesorMateria)
                .HasForeignKey(d => d.ProfesorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfMat_Profesor");
        });

        modelBuilder.Entity<Profesore>(entity =>
        {
            entity.HasKey(e => e.ProfesorId).HasName("PK__Profesor__4DF3F0C8849C4D22");
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Seccione>(entity =>
        {
            entity.HasKey(e => e.SeccionId).HasName("PK__Seccione__18B616416148EB17");
            entity.Property(e => e.Nombre).HasMaxLength(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
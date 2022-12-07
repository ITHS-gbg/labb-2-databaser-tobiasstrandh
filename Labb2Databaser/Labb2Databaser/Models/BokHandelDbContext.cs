using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Labb2Databaser.Models;

public partial class BokHandelDbContext : DbContext
{
    public BokHandelDbContext()
    {
    }

    public BokHandelDbContext(DbContextOptions<BokHandelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BokFormatTbl> BokFormatTbls { get; set; }

    public virtual DbSet<ButikTbl> ButikTbls { get; set; }

    public virtual DbSet<ButiksSaldoTbl> ButiksSaldoTbls { get; set; }

    public virtual DbSet<BöckerTbl> BöckerTbls { get; set; }

    public virtual DbSet<FörfattareTbl> FörfattareTbls { get; set; }

    public virtual DbSet<FörlagTbl> FörlagTbls { get; set; }

    public virtual DbSet<GenreTbl> GenreTbls { get; set; }

    public virtual DbSet<Ordrar> Ordrars { get; set; }

    public virtual DbSet<OrdrarDetaljer> OrdrarDetaljers { get; set; }

    public virtual DbSet<SpårkTbl> SpårkTbls { get; set; }

    public virtual DbSet<VAntalSåldaPerGenre> VAntalSåldaPerGenres { get; set; }

    public virtual DbSet<VHurMångaSåldaIvarjeButikNov2022> VHurMångaSåldaIvarjeButikNov2022s { get; set; }

    public virtual DbSet<VTitlarPerFörfattare> VTitlarPerFörfattares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-90FBFP4U;Initial Catalog=BokHandelDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BokFormatTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BokForma__3214EC27C5297FE2");

            entity.ToTable("BokFormatTbl");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<ButikTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ButikTbl__3214EC075962EAB0");

            entity.ToTable("ButikTbl");
        });

        modelBuilder.Entity<ButiksSaldoTbl>(entity =>
        {
            entity.HasKey(e => new { e.ButiksId, e.Isbn }).HasName("PK_ButiksSaldo");

            entity.ToTable("ButiksSaldoTbl");

            entity.Property(e => e.ButiksId).HasColumnName("ButiksID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butiks).WithMany(p => p.ButiksSaldoTbls)
                .HasForeignKey(d => d.ButiksId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ButiksID");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.ButiksSaldoTbls)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ISBN");
        });

        modelBuilder.Entity<BöckerTbl>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PK__BöckerTb__447D36EB7EFEA723");

            entity.ToTable("BöckerTbl");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.BokFormatId).HasColumnName("BokFormatID");
            entity.Property(e => e.FörlagId).HasColumnName("FörlagID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Utgivningsdatum).HasColumnType("date");

            entity.HasOne(d => d.BokFormat).WithMany(p => p.BöckerTbls)
                .HasForeignKey(d => d.BokFormatId)
                .HasConstraintName("FK__BöckerTbl__BokFo__7FEAFD3E");

            entity.HasOne(d => d.Förlag).WithMany(p => p.BöckerTbls)
                .HasForeignKey(d => d.FörlagId)
                .HasConstraintName("FK__BöckerTbl__Förla__7EF6D905");

            entity.HasOne(d => d.Genre).WithMany(p => p.BöckerTbls)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__BöckerTbl__Genre__00DF2177");

            entity.HasOne(d => d.SpråkNavigation).WithMany(p => p.BöckerTbls)
                .HasForeignKey(d => d.Språk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BöckerTbl__Språk__7E02B4CC");

            entity.HasMany(d => d.Författares).WithMany(p => p.Isbns)
                .UsingEntity<Dictionary<string, object>>(
                    "FörfattareBöckerTbl",
                    r => r.HasOne<FörfattareTbl>().WithMany()
                        .HasForeignKey("FörfattareId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Författar__Förfa__0880433F"),
                    l => l.HasOne<BöckerTbl>().WithMany()
                        .HasForeignKey("Isbn")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Författare__ISBN__078C1F06"),
                    j =>
                    {
                        j.HasKey("Isbn", "FörfattareId").HasName("PK_FörfattareBöcker");
                        j.ToTable("FörfattareBöckerTbl");
                    });
        });

        modelBuilder.Entity<FörfattareTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Författa__3214EC07A1A55533");

            entity.ToTable("FörfattareTbl");

            entity.Property(e => e.Födelsedatum).HasColumnType("date");
        });

        modelBuilder.Entity<FörlagTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FörlagTb__3214EC27E8870C85");

            entity.ToTable("FörlagTbl");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<GenreTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GenreTbl__3214EC2797F50865");

            entity.ToTable("GenreTbl");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<Ordrar>(entity =>
        {
            entity.HasKey(e => new { e.OrderNr, e.Isbn }).HasName("PK__Ordrar__67D7AF19B95A5577");

            entity.ToTable("Ordrar");

            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ordrar__ISBN__04AFB25B");

            entity.HasOne(d => d.OrderNrNavigation).WithMany(p => p.Ordrars)
                .HasForeignKey(d => d.OrderNr)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ordrar__OrderNr__03BB8E22");
        });

        modelBuilder.Entity<OrdrarDetaljer>(entity =>
        {
            entity.HasKey(e => e.OrderNr).HasName("PK__Ordrar_d__C3907C77EB77E4E9");

            entity.ToTable("Ordrar_detaljer");

            entity.Property(e => e.OrderNr).ValueGeneratedNever();
            entity.Property(e => e.OrderDatum).HasColumnType("date");

            entity.HasOne(d => d.Butiks).WithMany(p => p.OrdrarDetaljers)
                .HasForeignKey(d => d.ButiksId)
                .HasConstraintName("FK__Ordrar_de__Butik__70A8B9AE");
        });

        modelBuilder.Entity<SpårkTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SpårkTbl__3214EC27E5585B85");

            entity.ToTable("SpårkTbl");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<VAntalSåldaPerGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vAntalSåldaPerGenre");

            entity.Property(e => e.AntalSålda)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TotalVärde)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VHurMångaSåldaIvarjeButikNov2022>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vHurMångaSåldaIVarjeButikNov2022");
        });

        modelBuilder.Entity<VTitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vTitlarPerFörfattare");

            entity.Property(e => e.AntalTitlar)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TotalVärdeLager)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Ålder)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

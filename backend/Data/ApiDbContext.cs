using System;
using System.Collections.Generic;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public partial class ApiDbContext : DbContext
{
    public ApiDbContext()
    {
    }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuthToken> AuthTokens { get; set; }

    public virtual DbSet<FailedLogin> FailedLogins { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    //    }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuthToke__3214EC07762D7072");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AccessToken).HasMaxLength(1024);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken).HasMaxLength(1024);

            entity.HasOne(d => d.User).WithMany(p => p.AuthTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AuthTokens_Users");
        });

        modelBuilder.Entity<FailedLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FailedLo__3214EC077DFFC52D");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AttemptAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07FE0E7BE9");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105346D25C49A").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSess__3214EC07F692A7F8");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Device).HasMaxLength(255);
            entity.Property(e => e.IpAddress).HasMaxLength(45);

            entity.HasOne(d => d.User).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserSessions_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

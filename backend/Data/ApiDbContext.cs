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
    public virtual DbSet<User> Users { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

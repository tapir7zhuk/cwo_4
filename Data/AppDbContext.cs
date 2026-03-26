using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrugStoreStatistics.Models;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreStatistics.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Medicine> Medicines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medicine>()
            .Property(m => m.Price)
            .HasPrecision(18, 2);
    }
}
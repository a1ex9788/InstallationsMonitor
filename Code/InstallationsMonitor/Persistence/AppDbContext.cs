using InstallationsMonitor.Entities;
using InstallationsMonitor.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace InstallationsMonitor.Persistence
{
    internal class AppDbContext : DbContext
    {
        private readonly DatabaseOptions databaseOptions;

        internal DbSet<Installation> Installations { get; set; } = null!;

        internal DbSet<FileOperation> FileOperations { get; set; } = null!;

        public AppDbContext(DatabaseOptions databaseOptions)
            : base(new DbContextOptions<AppDbContext>())
        {
            this.databaseOptions = databaseOptions;

            DirectoryInfo? directory = Directory.GetParent(this.databaseOptions.DatabaseFullName);

            if (directory != null && !Directory.Exists(directory.FullName))
            {
                Directory.CreateDirectory(directory.FullName);
            }

            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.databaseOptions.DatabaseFullName}");
        }
    }
}
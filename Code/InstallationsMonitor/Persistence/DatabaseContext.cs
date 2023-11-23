using InstallationsMonitor.Domain;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace InstallationsMonitor.Persistence
{
    public class DatabaseContext : DbContext
    {
        private readonly DatabaseOptions databaseOptions;

        public DbSet<Installation> Installations { get; set; } = null!;

        public DbSet<FileChange> FileChanges { get; set; } = null!;

        public DbSet<FileCreation> FileCreations { get; set; } = null!;

        public DbSet<FileDeletion> FileDeletions { get; set; } = null!;

        public DbSet<FileRenaming> FileRenamings { get; set; } = null!;

        public DatabaseContext(DatabaseOptions databaseOptions)
            : base(new DbContextOptions<DatabaseContext>())
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
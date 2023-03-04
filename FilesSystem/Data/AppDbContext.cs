using FilesSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FilesSystem.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Folder> Folders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Folder>().HasData(
               new Folder() { Id = 1, Name = "Creating Digital Images", ParentId = null, Path = ""},
               new Folder() { Id = 2, Name = "Resources", ParentId = 1, Path =""},
               new Folder() { Id = 3, Name = "Primary Sources", ParentId = 2, Path = ""},
               new Folder() { Id = 4, Name = "Secondary Sources", ParentId = 2, Path = ""},
               new Folder() { Id=5, Name = "Evidence", ParentId = 1, Path = ""},
               new Folder() { Id = 6, Name = "Graphic Products", ParentId = 1, Path = ""}, 
               new Folder () { Id = 7, Name = "Process", ParentId = 6, Path = ""},
               new Folder() { Id = 8, Name = "Final Product", ParentId = 6, Path = ""}
               );
        }
    }
}

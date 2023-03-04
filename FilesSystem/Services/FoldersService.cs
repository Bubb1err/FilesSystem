using FilesSystem.Data;
using FilesSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq.Expressions;

namespace FilesSystem.Services
{
    public class FoldersService : IFoldersService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        internal DbSet<Folder> _dbSet;
        public FoldersService(AppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _dbSet = _context.Set<Folder>();
            _enviroment = enviroment;
        }

        public async Task AddAsync(Folder folder)
        {
            await _dbSet.AddAsync(folder);
            await SaveAsync();
        }

        public async Task<List<Folder>> GetAllAsync(Expression<Func<Folder, bool>>? filter = null)
        {
            IQueryable<Folder> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task WriteZipHierarchyToDb(IFormFile zip)
        {
            using (ZipArchive archive = new ZipArchive(zip.OpenReadStream(), ZipArchiveMode.Read))
            {
                var folders = new List<Folder>();
                string path = Path.Combine(_enviroment.WebRootPath, "upload", zip.FileName.Split('.')[0]);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                archive.ExtractToDirectory(path);
                await TraverseTree(path);
                Directory.Delete(path, recursive: true);
            }
        }
       protected async Task TraverseTree(string root)
        {
            Stack<Folder> dirs = new Stack<Folder>(20);

            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            var parts = root.Split('\\');
            var folder = new Folder()
            {
                Name = parts[parts.Length - 1],
                ParentId = null,
                Path = root
            };
            dirs.Push(folder);
            await AddAsync(folder);

            while (dirs.Count > 0)
            {
                Folder currentDir = dirs.Pop();

                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir.Path);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string str in subDirs)
                {
                    parts = str.Split('\\');
                    folder = new Folder()
                    {
                        Name = parts[parts.Length - 1],
                        ParentId = currentDir.Id,
                        Path = str
                    };
                    dirs.Push(folder);
                    await AddAsync(folder);
                }

            }
        }
    }
}

using CsvHelper;
using FilesSystem.Data;
using FilesSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Text;

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
        public async Task AddRangeAsync(IEnumerable<Folder> folders)
        {
            await _dbSet.AddRangeAsync(folders);
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
            try
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
            catch(ArgumentException) 
            {
                throw new ArgumentException();
            }
            catch(InvalidDataException) 
            {
                throw new InvalidDataException();
            }
            catch (IOException)
            {
                throw new IOException();
            }
            catch (Exception)
            {
                throw new Exception();
            }


        }
        public byte[] GetBytesFromDb()
        {
            var folders = _dbSet;
            try
            {
                using var mem = new MemoryStream();
                using var writer = new StreamWriter(mem, Encoding.UTF8);
                using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
                {
                    csvWriter.WriteField("Id");
                    csvWriter.WriteField("Name");
                    csvWriter.WriteField("Parent Id");
                    csvWriter.WriteField("Path");
                    csvWriter.NextRecord();
                    foreach (var folder in folders)
                    {
                        csvWriter.WriteField(folder.Id);
                        csvWriter.WriteField(folder.Name);
                        var parentId = folder.ParentId == null ? -1 : folder.ParentId;
                        csvWriter.WriteField(parentId);
                        csvWriter.WriteField(" ");
                        csvWriter.NextRecord();

                    }
                    writer.Flush();
                    mem.Seek(0, SeekOrigin.Begin);
                    return mem.ToArray();
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException();

            }
            catch(IOException)
            {
                throw new IOException();
            }
            catch(Exception)
            {
                throw new Exception();
            }
            
        }
        public async Task ParseCsvAndAddToDb(IFormFile file)
        {
            try
            {
                var lines = new List<string>();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                        lines.Add(reader.ReadLine());
                }
                int lastInd = _dbSet.OrderBy(x => x.Id).Last().Id;
                List<Folder> folders = lines.Skip(1)
                                            .Select(line => ParseCsv(line, lastInd))
                                            .ToList();
                await AddRangeAsync(folders);
            }
            catch (ArgumentException) { throw new ArgumentException(); }
            catch(Exception) { throw new Exception(); }
            
        }
        protected Folder ParseCsv(string line, int lastId)
        {
            try
            {
                var parts = line.Split(',');
                return new Folder()
                {
                    Name = parts[1],
                    ParentId = int.Parse(parts[2]) == -1 ? null : int.Parse(parts[2]) + lastId,
                    Path = ""
                };
            }
            catch (Exception ex) { throw new Exception(); }
            
        }
        protected async Task TraverseTree(string root)
        {
            try
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
                    catch(Exception ex) { throw new Exception(); }
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
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException();
            }
            catch (IOException)
            {
                throw new IOException();
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }
    }
}

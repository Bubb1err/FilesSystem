using FilesSystem.Models;
using System.Linq.Expressions;

namespace FilesSystem.Services
{
    public interface IFoldersService
    {
        byte[] GetBytesFromDb();
        Task WriteZipHierarchyToDb(IFormFile zip);
        Task AddAsync(Folder folder);
        Task<List<Folder>> GetAllAsync(Expression<Func<Folder, bool>>? filter = null);
        Task SaveAsync();
        Task ParseCsvAndAddToDb(IFormFile file);
        Task AddRangeAsync(IEnumerable<Folder> folders);
    }
}

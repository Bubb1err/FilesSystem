using FilesSystem.Models;

namespace FilesSystem.Services
{
    public class FolderManager
    {
        private readonly IFoldersService _service;
        public FolderManager(IFoldersService service)
        {
            _service = service;
        }
        //Adds folder and subfolders to db
        public void TraverseTree(string root)
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
            _service.AddAsync(folder);

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
                    _service.AddAsync(folder);
                }

            }
        }
    }
}

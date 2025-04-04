using System.IO;
using File_Processor.Models;
using File_Processor.Services;

namespace File_Processor.Controllers
{
    internal class DirectoryController
    {
        private DirectoryService _service;

        public DirectoryController()
        {
            _service = new DirectoryService();
        }
        public bool AddDirectory(string path, string name)
        {
            if (path.StartsWith("\""))
            {
                path = path.Substring(1);
            }
            if (path.EndsWith("\""))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return _service.AddDirectoryToDb(new DirectoryModel(path, name));
        }

        public bool RemoveDirectory(string path)
        {
            return _service.DeleteDirectoryFromDb(new DirectoryModel(path, ""));
        }

        public List<DirectoryModel> GetDirectories()
        {
            List<DirectoryModel> dirs = _service.GetDirectoriesFromDb();

            for (int i = dirs.Count - 1; i >= 0; i--)
            {
                DirectoryModel dir = dirs[i];
                if (!Directory.Exists(dir.directoryPath))
                {
                    dirs.RemoveAt(i);
                    RemoveDirectory(dir.directoryPath);
                }
            }

            return dirs;
        }
    }
}

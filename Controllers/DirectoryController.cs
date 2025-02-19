using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return _service.GetDirectoriesFromDb();
        }
    }
}

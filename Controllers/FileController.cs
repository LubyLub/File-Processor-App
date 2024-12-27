using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using File_Processor.Models;
using System.Security.Cryptography;
using Extensions.Data;
using System.IO;
using System.Runtime.CompilerServices;

namespace File_Processor.Controllers
{
    internal class FileController
    {
        private FileModel file;

        internal FileController(String path)
        {
            String name = path.Split('\\').Last();
            ulong hash = FileToHash(path);
            DateTime first = File.GetCreationTime(path);
            DateTime last = File.GetLastWriteTime(path);
            file = new FileModel(path, hash, name, last, first);
        }

        internal static ulong FileToHash(String path) 
        {
            using (var stream = File.OpenRead(path))
            {
                var state = XXHash.CreateState64();
                XXHash.UpdateState64(state, stream);
                return XXHash.DigestState64(state);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    class FileModelPathComparer : IEqualityComparer<FileModel>
    {
        public bool Equals(FileModel? x, FileModel? y)
        {
            if (x == null || y == null) { return false; }
            return x.filePath == y.filePath;
        }

        public int GetHashCode([DisallowNull] FileModel obj)
        {
            return obj.filePath.GetHashCode();
        }
    }
}

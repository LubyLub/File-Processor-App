﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using File_Processor.Models;
using System.IO;

namespace File_Processor.Services
{
    internal class GeneralProcessor : FileProcessor
    {
        private protected override string readWholeFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
}

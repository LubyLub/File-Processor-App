using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Exceptions
{
    internal class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException() { }
        public NoInternetConnectionException(string message) : base(message) { }
    }
}

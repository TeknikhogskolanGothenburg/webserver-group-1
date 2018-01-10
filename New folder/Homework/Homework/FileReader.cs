using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Homework
{
    public class FileReader
    {
        public string Index { get; set; }
        public FileReader()
        {
            Index = File.ReadAllText(Environment.CurrentDirectory + @"../../../../../../" + @"Content/index.html");
        }

    }
}

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
        public string PianoCat { get; set; }
        public byte[] Panda { get; set; }

        public FileReader()
        {

            PianoCat = File.ReadAllText(Environment.CurrentDirectory + @"../../../../../../" + @"Content/pianocat.gif");
            Index = File.ReadAllText(Environment.CurrentDirectory + @"../../../../../../" + @"Content/index.html");
            Panda = File.ReadAllBytes(Environment.CurrentDirectory + @"../../../../../../" + @"Content/laughing_panda.jpg");

         
        }

     

    }
}

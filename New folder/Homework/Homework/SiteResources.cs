using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Homework
{
    class SiteResources
    {
        public string Root = Environment.CurrentDirectory + "../../../../../../" + "Content";
        public List<string> FilePaths = new List<string>();
        public SiteResources()
        {
            FilePaths = FindAllFiles(Root);
            foreach (string path in FilePaths)
            {
                Console.WriteLine(path);
            }
        }


        //returns the path of each file in a given directory and all its subfolders
        public List<string> FindAllFiles(string path)
        {
            List<string> result = new List<string>();
            foreach (string entity in Directory.GetFileSystemEntries(path,"*"))
            {
                if(entity.Substring(entity.Length - 5, 5).Contains("."))
                {
                    result.Add(entity);
                }
                else
                {
                    foreach (string s in FindAllFiles(entity))
                    {
                        result.Add(s);
                    }
                }                
            }
            return result;
        }

        public string GetExtension(string path)
        {
            string result = "";
            do
            {
                result = path[path.Length - 1].ToString() + result;
                path = path.Substring(0, path.Length - 1);
            }
            while (path[path.Length - 1].ToString() != ".");

            return result;
        }
    }
}

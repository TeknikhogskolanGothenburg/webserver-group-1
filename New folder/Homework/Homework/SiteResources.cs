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
        public Dictionary<string, string> FilePaths { get; set; }
        public SiteResources()
        {
            FilePaths = ConvertToDic(FindAllFiles(Root));            
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

        private Dictionary<string,string> ConvertToDic(List<string> list)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string path in list)
            {
                string trash = (Environment.CurrentDirectory + "../../../../../../" + "Content");
                string key = path.Substring(trash.Length, path.Length - trash.Length);
                result.Add(key, path);
            }
            return result;
        }
    }
}

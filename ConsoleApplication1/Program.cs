using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static string[] SearchFiles(string dir, string regex)
        {
            string[] files = Directory.GetFiles(dir, regex, SearchOption.AllDirectories);

            return files;
        }

        static string CreateTargetDir()
        {
            DateTime dt = DateTime.Now;
            string folderName = dt.ToString("yyyyMMdd-HHmmss");

            Directory.CreateDirectory(folderName);

            return folderName;
        }

        static string GetNewFileName(string originalName)
        {
            string newName = "";
            string searchWord = ".*-.*-(?<new>.*?)_.*";
            Regex re = new Regex(searchWord, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            for (Match m = re.Match(originalName); m.Success; m = m.NextMatch())
            {
                newName = m.Groups["new"].Value + ".jpg";
            }
            return newName;
        }

        static void CopyFile(string original, string targetDir, string newName)
        {
            string destFile = Path.Combine(targetDir, newName);

            try
            {
                File.Copy(original, destFile, false);
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory();

            string targetDir = Path.Combine(currentDir, CreateTargetDir());
            
            string regex = "*_1_800x800.jpg";
            string[] files = SearchFiles(currentDir, regex);

            foreach (string s in files)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(s);
                string newFileName = GetNewFileName(fileName);
                CopyFile(s, targetDir, newFileName);
            }
            
            // for Retail Dir
            string srcDir = Path.Combine(currentDir, "Retail");
            string retailRegex = "*1.jpg";

            string[] retailFiles = SearchFiles(srcDir, retailRegex);

            foreach(string s in retailFiles)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(s);
                string newFileName = GetNewFileName(fileName);
                CopyFile(s, targetDir, newFileName);
            }
        }
    }
}

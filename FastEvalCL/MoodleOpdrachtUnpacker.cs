using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL
{
    public class MoodleOpdrachtUnpacker
    {
        public static void UnpackAllOpdrachten(string zipSourcePath, string folderDestinationPath, string folderName="unzipped")
        {
            //TODO in ui waarcshuwing dat dit bestaande stuff zal verwijderen
            //TODO waarschuwen in UI dat dit enkel werkt als in iedere folder van student exact 1 bestand (zip) staat
            if (!File.Exists(zipSourcePath))
                throw new FileNotFoundException($"{zipSourcePath} not found");
            if(!Directory.Exists(folderDestinationPath))
                throw new DirectoryNotFoundException($"{folderDestinationPath} not found");

            //TODO destination folder deleten

            string dest = Path.Combine(folderDestinationPath, folderName);
            Directory.CreateDirectory(dest);

            System.IO.Compression.ZipFile.ExtractToDirectory(zipSourcePath, dest, true);

            var dirs = Directory.GetDirectories(dest);
            foreach (var dir in dirs)
            {
                
                var file = Directory.GetFiles(dir).FirstOrDefault();
                if(file!=null)
                {
                    FileInfo f = new FileInfo(file);
                    string newName = Path.GetFileName(Path.GetDirectoryName(file)).Split("_")[0] + f.Extension;
                    //TODO voornaam/achternaam verwisselen
                    //TODO idee: mappen aan excel met studenten en ineens missing zips oplijsten?
                    string fullPath = f.Directory.FullName + "\\" + newName;
                    f.MoveTo(fullPath);

                    try
                    {
                        System.IO.Compression.ZipFile.ExtractToDirectory(fullPath, dir);
                    }
                    catch (Exception)
                    {

                        Debug.WriteLine($"{fullPath} not an archive");
                    }
                   

                }

                string newFolder = Path.Combine(dest, Path.GetFileName(Path.GetDirectoryName(file)).Split("_")[0]);
                Directory.Move(dir, newFolder);

            }

        }
    }
}

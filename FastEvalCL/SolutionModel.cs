using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL
{
    public class SolutionModel
    {
        public SolutionModel(string path, bool tryFetchInfo = true)
        {
            Path = path;

            if (tryFetchInfo)
            {
                string code = File.ReadAllText(path);
                Info = FastEvalCL.GetInfoFromCode(code);
            }
            else
                Info = new Info()
                {
                    AchterNaam = "Onbekend",
                    VoorNaam = "Onbekend",
                    Klasgroep = "Onbekend",
                    SNummer = path
                };
        }
        public string Path { get; set; }

        public string FolderPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path);
            }
        }

        public Info Info { get; set; }
    }
}

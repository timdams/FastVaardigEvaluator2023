using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL
{
    public class SolutionHelper
    {
        public static List<SolutionModel> LoadAllSolutionsFromPath(string folderPath, bool tryFetchInfo = true) 
        {
            List<SolutionModel> result = new List<SolutionModel>();
            var allPrograms= Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);
            foreach (var prog in allPrograms)
            {
                result.Add(new SolutionModel(prog, tryFetchInfo));
            }
            return result;
        }
    }
}

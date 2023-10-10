using System.Text.RegularExpressions;

namespace FastEvalCL
{
    public class FastEvalCL
    {
        //https://stackoverflow.com/questions/4766414/parsing-regions-within-code-files 
        public static List<string> GetCodeRegions(string code)
        {
            List<string> codeRegions = new List<string>();

            //Split code into regions
            var matches = Regex.Matches(code, "#region.*?#endregion", RegexOptions.Singleline);

            foreach (var match in matches)
            {
                //split regions into lines
                string[] lines = match.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                if (lines.Length > 2)
                {
                    codeRegions.Add(string.Join("\r\n", lines, 1, lines.Length - 2));
                }

            }

            return codeRegions;
        }

        public static Info GetInfoFromCode(string code)
        {
            var resultInfo = new Info();
            var lines = code.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                if(line.Trim().StartsWith("//S-nummer"))
                {
                   resultInfo.SNummer= line.Split(":").LastOrDefault().Trim();
                }
                else if (line.Trim().StartsWith("//Voornaam"))
                {
                    resultInfo.VoorNaam = line.Split(":").LastOrDefault().Trim();
                }
                else if (line.Trim().StartsWith("//Achternaam"))
                {
                    resultInfo.AchterNaam = line.Split(":").LastOrDefault()?.Trim() ;
                }
                else if (line.Trim().StartsWith("//Klasgroep"))
                {
                    resultInfo.Klasgroep = line.Split(":").LastOrDefault().Trim();
                }
            }
            return resultInfo;
        }
    }
}
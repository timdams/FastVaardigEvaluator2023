using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL
{
    internal class CodeComparerFraudDetector
    {
        public enum DetectorModes { EqualLines }
        public static double SimilarityCompute(string codeSource, string codeTarget, DetectorModes detectorModes )
        {
            var rootThisCode = (CSharpSyntaxTree.ParseText(codeSource)).GetRoot();
            var linesThis = rootThisCode.GetText().Lines.Select(line => line.ToString().Trim()).ToList();
           

            var rootTarget = (CSharpSyntaxTree.ParseText(codeTarget)).GetRoot();
            var linesTarget = rootTarget.GetText().Lines.Select(line => line.ToString().Trim()).ToList();

            if (detectorModes == DetectorModes.EqualLines)
            {

                var map1 = CreateFrequencyMap(linesThis);
                var map2 = CreateFrequencyMap(linesTarget);
                int totalLines = linesThis.Count + linesTarget.Count;
                int matchingLines = CountMatchingLines(map1, map2);

                // Avoid division by zero
                if (totalLines == 0) return 0;

                return Math.Round((matchingLines * 2.0 / totalLines) * 100,0); // Multiply by 2 as matching lines are counted in both lists
            }
            return 0;
        }
        private static Dictionary<string, int> CreateFrequencyMap(List<string> lines)
        {
            var map = new Dictionary<string, int>();
            foreach (var line in lines)
            {
                if (map.ContainsKey(line))
                {
                    map[line]++;
                }
                else
                {
                    map[line] = 1;
                }
            }
            return map;
        }
        private static int CountMatchingLines(Dictionary<string, int> map1, Dictionary<string, int> map2)
        {
            int matchingCount = 0;
            foreach (var pair in map1)
            {
                if (map2.TryGetValue(pair.Key, out int countInMap2))
                {
                    matchingCount += Math.Min(pair.Value, countInMap2);
                }
            }
            return matchingCount;
        }


    }
}

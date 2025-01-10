using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastSLNEvaluator2024.EvaluationLib
{
    internal class Examen
    {
        public Examen()
        {
            Opgaves = new List<Opgave>();
        }
        public List<Opgave> Opgaves { get; set; }

        public int MaxScore
        {
            get
            {
                int totaal = 0;
                foreach (var opgave in Opgaves)
                {
                    totaal += opgave.MaxScore;
                }
                return totaal;
            }
        }

    }
}

namespace FastSLNEvaluator2024.EvaluationLib
{
    public class Opgave
    {
        public Opgave()
        {
            Criterias = new List<Criteria>();
        }
        public string Beschrijving { get; set; }
        public List<Criteria> Criterias { get; set; }

        public int MaxScore
        {
            get
            {
                int totaal = 0;
                foreach (var opgave in Criterias)
                {
                    totaal += opgave.MaxScore;
                }
                return totaal;
            }
        }
    }
}
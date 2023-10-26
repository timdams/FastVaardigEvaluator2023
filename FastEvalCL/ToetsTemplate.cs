namespace FastEvalCL
{
    public class ToetsTemplate
    {
        public string NaamToets { get; set; } = "OefenExamen 2023";
        public int Versie { get; set; } = 1;
        public List<Vraag> Vragen { get; set; } = new List<Vraag>();
    }


    public class Vraag
    {

        public string Categorie { get; set; }
        public  int MaxScore { get; set; } = 1;
        public  string Beschrijving { get; set; }
        public int Score { get; set; }
    }

    

}

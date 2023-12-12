namespace FastEvalCL
{
    
    public class StudentInfo
    {
        private string achterNaam;
        private string sNummer;
        private string voorNaam;
        private string klasgroep;

        public string SNummer {
            get
            {
                if (sNummer == "" || sNummer == null) return "ONBEKENDSNUM";
                return sNummer;
            }
            set => sNummer = value;
        }
        public string VoorNaam {
            get
            {
                if (voorNaam == "" || voorNaam == null) return "ONBEKENDVNAAM";
                return voorNaam;
            }
            set => voorNaam = value;
        }
        public string AchterNaam
        {
            get
            {
                if (achterNaam == "" || achterNaam == null) return "ONBEKENDANAAM";
                return achterNaam;
            }
            set => achterNaam = value;
        }
        public string Klasgroep {
            get
            {
                if (klasgroep == "" || klasgroep == null) return "ONBEKENDKLGROEP";
                return klasgroep;
            }
            set => klasgroep = value;
        }

        public string SorteerNaam
        {
            get
            {
                return $"{AchterNaam} {VoorNaam}";
            }
        }

        public override string ToString()
        {
            return $"{AchterNaam} {VoorNaam} ({SNummer}), {Klasgroep}";
        }
    }
}

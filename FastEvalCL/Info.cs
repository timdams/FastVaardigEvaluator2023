namespace FastEvalCL
{
    public class Info
    {
        private string achterNaam;
        private string sNummer;
        private string voorNaam;
        private string klasgroep;

        public string SNummer {
            get
            {
                if (sNummer == "") return "ONBEKENDSNUM";
                return sNummer;
            }
            set => sNummer = value;
        }
        public string VoorNaam {
            get
            {
                if (voorNaam == "") return "ONBEKENDVNAAM";
                return voorNaam;
            }
            set => voorNaam = value;
        }
        public string AchterNaam
        {
            get
            {
                if (achterNaam == "") return "ONBEKENDANAAM";
                return achterNaam;
            }
            set => achterNaam = value;
        }
        public string Klasgroep {
            get
            {
                if (klasgroep == "") return "ONBEKENDKLGROEP";
                return klasgroep;
            }
            set => klasgroep = value;
        }
        public override string ToString()
        {
            return $"{AchterNaam} {VoorNaam} ({SNummer}), {Klasgroep}";
        }
    }
}

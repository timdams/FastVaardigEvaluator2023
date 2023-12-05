using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FastEvalCL
{
    public enum Severity { Light, Medium, Heavy };
    public enum Tested { NietGetest, CompileertNiet, Compileert}
    public class Boete : INotifyPropertyChanged
    {
        private Tested compileertNiet = Tested.NietGetest;
        private bool klassenNietApart;
        private bool slechteBladSpiegel;
        private bool slechteNaamgevingConventie;
        private bool incosistendeNaamgeving;
        private bool linqGebruikt;
        private int gotoAndFriendsGebruikt;
        private bool methodenInMethoden;
        private bool geenZip;
        private int redundanteCode;
        
        //Todo scriptbased doen en dit laten inladen vanuit json file

        public Tested CompileertNiet
        {
            get => compileertNiet;
            set
            {
                compileertNiet = value;
                OnPropertyChanged();
            }
        } //1
        public bool KlassenNietApart
        {
            get => klassenNietApart;
            set
            {
                klassenNietApart = value;
                OnPropertyChanged();
            }
        }//1
        public bool SlechteBladSpiegel
        {
            get => slechteBladSpiegel;
            set
            {
                slechteBladSpiegel = value;
                OnPropertyChanged();
            }
        }//1
        public bool SlechteNaamgevingConventie
        {
            get => slechteNaamgevingConventie;
            set
            {
                slechteNaamgevingConventie = value;
                OnPropertyChanged();
            }
        }//2
        public bool IncosistendeNaamgeving
        {
            get => incosistendeNaamgeving;
            set
            {
                incosistendeNaamgeving = value;
                OnPropertyChanged();
            }
        }//1
        public bool LinqGebruikt
        {
            get => linqGebruikt;
            set
            {
                linqGebruikt = value;
                OnPropertyChanged();
            }

        }//3
        public int GotoAndFriendsGebruikt
        {
            get => gotoAndFriendsGebruikt;
            set
            {
                gotoAndFriendsGebruikt = value;
                OnPropertyChanged();
            }
        }//3
        public bool MethodenInMethoden
        {
            get => methodenInMethoden;
            set
            {
                methodenInMethoden = value;
                OnPropertyChanged();
            }
        }//3
        public bool GeenZip
        {
            get => geenZip;
            set
            {
                geenZip = value;
                OnPropertyChanged();
            }
        }
        public int RedundanteCode
        {
            get => redundanteCode;
            set
            {
                redundanteCode = value;
                OnPropertyChanged();
            }
        }//3

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string GetCVSLine()
        {
            string res = "";

            //TODO deze klasse vraagt om ambras. Beter scripted/generiek maken
            if(compileertNiet== Tested.NietGetest)
                res += "x" + ";";
            if (compileertNiet == Tested.CompileertNiet)
                res += "1" + ";";
            if (compileertNiet == Tested.Compileert)
                res += "0" + ";";
            res += BoolToString(klassenNietApart, 1) + ";";
            res += BoolToString(slechteBladSpiegel, 1) + ";";
            res += BoolToString(slechteNaamgevingConventie, 2) + ";";
            res += BoolToString(incosistendeNaamgeving, 1) + ";";
            res += BoolToString(linqGebruikt, 3) + ";";
            res += gotoAndFriendsGebruikt.ToString() + ";";
            res += BoolToString(methodenInMethoden, 3) + ";";
            res += BoolToString(geenZip, 1) + ";";
            res += redundanteCode.ToString() + ";";

            return res;


        }
        public static string GetCVSLineHeader()
        {
            string res = "";
            res += "compileertNiet;";
            res += "klassenNietApart;";
            res += "slechteBladSpiegel;";
            res += "slechteNaamgevingConventie;";
            res += "incosistendeNaamgeving;";
            res += "linqGebruikt;";
            res += "gotoAndFriendsGebruikt;";
            res += "methodenInMethoden;";
            res += "geenZip;";
            res += "redundanteCode;";
            return res;
        }

        private string BoolToString(bool inp, int value)
        {
            return inp ? value.ToString() : "0";
        }



    }
}

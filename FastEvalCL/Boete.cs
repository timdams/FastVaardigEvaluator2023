using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FastEvalCL
{
    public enum Severity { Light, Medium, Heavy};
    public class Boete: INotifyPropertyChanged
    {
        private bool compileertNiet;
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

        public bool CompileertNiet
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
    }
}

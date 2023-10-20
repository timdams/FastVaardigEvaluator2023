using FastEvalCL;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text.Json;

namespace ConsoleTester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var code = @"#region ClassName.Test()    //Some method that does stuff
            //some stuff
            #endregion

                //VUL VOLGENDE INFORMATIE HIER IN
    //S-nummer              :  S00478
    //Voornaam              :  Jos   
    //Achternaam            :  Dams         
    //Klasgroep (bv 1IT1b)  :  1IT2b  

            #region ClassCName.Random()
            public static void Test()
            {
int meuh;
string Person;
            goto hell;

                 //Some more stuff

                goto 42;
            }
            #endregion";

            var mijnTest = new FastEvalCL.ToetsTemplate()
            {
                NaamToets = "Eerste poging 2023",
                Versie = 211
            };
            
            mijnTest.Vragen.Add(new Vraag() { MaxScore = 1, Beschrijving = "leuke vraag", Categorie = "H1" });
            mijnTest.Vragen.Add(new Vraag() { MaxScore = 0, Beschrijving = "Opmerkingen", Categorie = "H1" });
            mijnTest.Vragen.Add(new Vraag() { MaxScore = 1, Beschrijving = "goeie vraag2", Categorie = "H2" });
            mijnTest.Vragen.Add(new Vraag() { MaxScore = 5, Beschrijving = "goeie vraag3", Categorie = "H2" });
            mijnTest.Vragen.Add(new Vraag() { MaxScore = 2, Beschrijving = "Opmerkingen", Categorie = "H2" });


            string jsonString = JsonSerializer.Serialize(mijnTest);
            File.WriteAllText("trial.json", jsonString);

        }


    }
}

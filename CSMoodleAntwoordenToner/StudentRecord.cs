using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSMoodleAntwoordenToner
{
    internal class StudentRecord
    {
        public string Achternaam { get; set; }
        public string Voornaam { get; set; }
        public string EmailAdres { get; set; }
        public string Status { get; set; }
        public string GestartOp { get; set; }
        public string VoltooidOp { get; set; }
        public string Antwoord1 { private get; set; }
        public string Antwoord2 { private get; set; }
        public string Antwoord3 { private get; set; }
        public string Antwoord1Clean
        {
            get
            {
                return ParseCode(Antwoord1);

            }
        }
        public string Antwoord2Clean
        {
            get
            {
                return ParseCode(Antwoord2);
            }
        }
        public string Antwoord3Clean
        {
            get
            {
                return ParseCode(Antwoord3);
            }
        }

        private string ParseCode(string input)
        {

            string pattern = @"((\d+°\s.*?:)|(.+?:))(.*?)(?=\d+°|$)";
            var matches = Regex.Matches(input, pattern, RegexOptions.Singleline);
            string cleaned = "";
  
                foreach (Match match in matches)
                {
                string header = match.Groups[2].Success
                                ? match.Groups[2].Value.Trim() // Markering zoals "2° ..."
                                : match.Groups[3].Value.Trim(); // Secties zonder markering

                string content = match.Groups[4].Value.Trim(); // De bijbehorende code

                // Toon de header en laat de code inspringen
                     cleaned += (header) + Environment.NewLine + Environment.NewLine; // De lijn met "1° ..."
                    string[] lines = content.Split(new[] { "      " }, StringSplitOptions.RemoveEmptyEntries);
                    string actualCode = "";
                    foreach (var line in lines)
                    {
                        actualCode += "    " + line.Trim() + Environment.NewLine; // Inspringen van de code
                    }

                    cleaned += FormatCode(actualCode);
                    cleaned += Environment.NewLine;
                    cleaned += Environment.NewLine;

                }
            
            
            return cleaned;
        }

        private string FormatCode(string code)
        {
            string[] lines = code.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
            string indent = "    "; // 4 spaties als standaardinspringing
            int indentLevel = 0;
            string formattedCode = "";

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();

                // Verlaag de inspringingsniveau als de regel een sluitende accolade bevat
                if (trimmedLine.StartsWith("}"))
                {
                    indentLevel = Math.Max(0, indentLevel - 1);
                }

                // Voeg de inspringing toe
                formattedCode += new string(' ', indentLevel * 4) + trimmedLine + Environment.NewLine;

                // Verhoog het inspringingsniveau als de regel een opening bevat
                if (trimmedLine.EndsWith("{"))
                {
                    indentLevel++;
                }
            }

            return formattedCode.TrimEnd(); // Verwijder extra lege regels aan het einde

        }
    }
}

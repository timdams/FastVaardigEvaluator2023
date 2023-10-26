using System.Text.Json;

namespace FastEvalCL
{
    public class SolutionModel
    {
        public SolutionModel(string path, bool tryFetchInfo = true)
        {
            Path = path;

            //TODO kijken of boete file in map staat en inladen
            string boeteFile = System.IO.Path.Combine(FolderPath, "boete.json");
            if (File.Exists(boeteFile))
            {
                string jsonString = File.ReadAllText(boeteFile);
                Boete = JsonSerializer.Deserialize<Boete>(jsonString);
            }
            else
                Boete = new Boete();

            //Vermoedelijk kan dus boetegedoe weg later
            string scoreFile = System.IO.Path.Combine(FolderPath, "score.json");
            if (File.Exists(scoreFile))
            {
                string jsonString = File.ReadAllText(scoreFile);
                ToetsResultaten = JsonSerializer.Deserialize<ToetsTemplate>(jsonString);
            }
            else
                ToetsResultaten = new ToetsTemplate();

            
            if (tryFetchInfo)
            {
                string code = File.ReadAllText(path);
                Info = FastEvalCL.GetInfoFromCode(code);
            }
            else
                Info = new Info()
                {
                    AchterNaam = "Onbekend",
                    VoorNaam = "Onbekend",
                    Klasgroep = "Onbekend",
                    SNummer = path
                };
        }
        public string Path { get; set; }

        public string FolderPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path);
            }
        }

        public Info Info { get; set; }
        public Boete Boete { get; set; }

        public ToetsTemplate ToetsResultaten { get; set; }

        public void SafeBoete()
        {
            if (Boete != null)
            {
                string fileName = System.IO.Path.Combine(FolderPath, "boete.json");
                string jsonString = JsonSerializer.Serialize(Boete);
                File.WriteAllText(fileName, jsonString);
            }
            if (ToetsResultaten != null)
            {
                string fileName = System.IO.Path.Combine(FolderPath, "score.json");
                string jsonString = JsonSerializer.Serialize(ToetsResultaten);
                File.WriteAllText(fileName, jsonString);
            }
        }
    }
}

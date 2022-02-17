using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    internal class ProfileSaver : IProfileSaver
    {
        public Profile Profile { get; set; }
        public string PathToSave { get; set; }

        public ProfileSaver(Profile profile, string pathToSave)
        {
            Profile = profile;
            PathToSave = pathToSave;
        }

        public void Save()
        {
            if (Profile == null)
            {
                Console.WriteLine("Анкета не создана!");
                return;
            }
            Directory.CreateDirectory(PathToSave);
            var path = $"{PathToSave}{Profile.FullName}.txt";
            File.WriteAllText(path, Profile.GetTextOfProfile());
            Console.WriteLine($"Файл {path} был Сохранен!");
        }
    }
}

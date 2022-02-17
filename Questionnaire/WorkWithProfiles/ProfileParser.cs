using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Предназначен для преобразования данных в объект класса Questionnaire
    /// </summary>
    internal class ProfileParser : IProfileParser
    {
        /// <summary>
        /// Преобразует строковое представление анкеты в объект класса Questionnaire
        /// </summary>
        /// <param name="questionnaire"> Строковое представление анкеты </param>
        /// <returns> Объект класса Questionnaire или null</returns>
        public Profile ParseToProfile(string[] questionnaire)
        {
            var name = questionnaire[0].Split(new string[] { "ФИО: " }, StringSplitOptions.None)[1].Trim();
            var dateOfBirth = DateTime.Parse(questionnaire[1].Split(new string[] { "Дата рождения: " }, StringSplitOptions.None)[1].Trim());
            var favoriteLanguage = questionnaire[2].Split(new string[] { "Любимый язык программирования: " }, StringSplitOptions.None)[1].Trim();
            var experience = Int32.Parse(questionnaire[3].Split(new string[] { "Опыт программирования на указанном языке: " }, StringSplitOptions.None)[1].Trim());
            var mobileNumber = questionnaire[4].Split(new string[] { "Мобильный телефон: " }, StringSplitOptions.None)[1].Trim();
            return new Profile(name, dateOfBirth, favoriteLanguage, experience, mobileNumber);
        }

        /// <summary>
        /// Получить набор анкет из указанной директории
        /// </summary>
        /// <param name="questionnaire"> Путь к директории с анкетами </param>
        public Profile[] ParseToProfiles(string questionnaireDirectory)
        {
            var questionnaires = Directory.GetFiles(questionnaireDirectory).Select(k => File.ReadAllLines(k));
            return questionnaires.Select(q => ParseToProfile(q)).ToArray();
        }
    }
}

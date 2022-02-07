using System;

namespace Questionnaire
{
    internal class Questionnaire
    {
        /// <summary>
        /// ФИО
        /// </summary>
        public string fullName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime dateOfBirth { get; private set; }

        /// <summary>
        /// Любимый язык программирования
        /// </summary>
        public string favoriteLanguage { get; set; }

        /// <summary>
        /// Опыт программирования на указанном языке
        /// </summary>
        public int programmingExperience { get; set; }

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// Дата заполнения анкеты
        /// </summary>
        public DateTime DateFilling { get; private set; }

        public Questionnaire(string fullName, DateTime dateOfBirth, string favoriteLanguage, int programmingExperience, string phoneNumber)
        {
            this.fullName = fullName;
            this.dateOfBirth = dateOfBirth;
            this.favoriteLanguage = favoriteLanguage;
            this.programmingExperience = programmingExperience;
            this.phoneNumber = phoneNumber;
            DateFilling = DateTime.Now;
        }

        /// <summary>
        /// Получить данные анкеты в текством представлении
        /// </summary>
        public string GetTextOfQuestionnaire()
        {
            return
                $@"1. ФИО: {fullName}
2. Дата рождения: {dateOfBirth.ToShortDateString()}
3. Любимый язык программирования: {favoriteLanguage}
4. Опыт программирования на указанном языке: {programmingExperience}
5. Мобильный телефон: {phoneNumber}

Анкета заполнена: {DateFilling.ToShortDateString()}";
        }
    }
}

using System;

namespace Questionnaire
{
    internal class Questionnaire
    {
        /// <summary>
        /// ФИО
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Любимый язык программирования
        /// </summary>
        public string FavoriteLanguage { get; set; }

        /// <summary>
        /// Опыт программирования на указанном языке
        /// </summary>
        public int ProgrammingExperience { get; set; }

        /// <summary>
        /// Мобильный телефон
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Дата заполнения анкеты
        /// </summary>
        public DateTime DateFilling { get; private set; }

        public Questionnaire(string fullName, DateTime dateOfBirth, string favoriteLanguage, int programmingExperience, string phoneNumber)
        {
            this.FullName = fullName;
            this.DateOfBirth = dateOfBirth;
            this.FavoriteLanguage = favoriteLanguage;
            this.ProgrammingExperience = programmingExperience;
            this.PhoneNumber = phoneNumber;
            DateFilling = DateTime.Now;
        }

        /// <summary>
        /// Получить данные анкеты в текством представлении
        /// </summary>
        public string GetTextOfQuestionnaire()
        {
            return
                $@"1. ФИО: {FullName}
2. Дата рождения: {DateOfBirth.ToShortDateString()}
3. Любимый язык программирования: {FavoriteLanguage}
4. Опыт программирования на указанном языке: {ProgrammingExperience}
5. Мобильный телефон: {PhoneNumber}

Анкета заполнена: {DateFilling.ToShortDateString()}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Валидация вопросов анкеты
    /// </summary>
    internal static class QuestionValidation
    {
        /// <summary>
        /// Допустимые языки программирования
        /// </summary>
        public static string[] FavoriteLanguages = new[]
        {
            "PHP",
            "JavaScript",
            "C",
            "C++",
            "Java",
            "C#",
            "Python",
            "Ruby"
        };

        /// <summary>
        /// Провалидировать ФИО
        /// </summary>
        /// <param name="fullName"> ФИО </param>
        public static Result ValidateFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return new Result(false, "ФИО обязательно к заполнению! Поворите ввод.");
            }
            else if (fullName.Any(c => char.IsDigit(c)))
            {
                return new Result(false, "В ФИО не могут присутствовать цифры! Поворите ввод.");
            }
            else
            {
                return new Result(true);
            }
        }

        /// <summary>
        /// Провалидировать дату рождения
        /// </summary>
        /// <param name="DateOfBirth"> Дата рождения </param>
        public static Result ValidateDateOfBirth(string DateOfBirth)
        {
            if (!DateTime.TryParse(DateOfBirth, out DateTime date))
            {
                return new Result(false, "Некоректный формат даты! Поворите ввод (формат ДД.ММ.ГГГГ).");
            }
            else if (date > DateTime.Now)
            {
                return new Result(false, "Дата рождения не может быть больше текущей! Поворите ввод.");
            }
            else if (date <= DateTime.MinValue)
            {
                return new Result(false, $"Дата рождения должна быть больше {DateTime.MinValue.ToShortDateString()}! Поворите ввод.");
            }
            else
            {
                return new Result(true);
            }
        }

        /// <summary>
        /// Провалидировать любимый язык программирования
        /// </summary>
        /// <param name="favoriteLanguage"> Язык программирования </param>
        public static Result ValidateFavoriteLanguage(string favoriteLanguage)
        {
            if (string.IsNullOrEmpty(favoriteLanguage))
            {
                return new Result(false, "ФИО обязательно к заполнению! Поворите ввод.");
            }
            else if (!FavoriteLanguages.Contains(favoriteLanguage, StringComparer.OrdinalIgnoreCase))
            {
                return new Result(false, "Можно ввести только указанные языки (PHP, JavaScript, C, C++, Java, C#, Python, Ruby)! Поворите ввод.");
            }
            else
            {
                return new Result(true);
            }
        }

        /// <summary>
        /// Провалидировать опыт программирования
        /// </summary>
        /// <param name="programmingExperience"> Опыт программирования </param>
        /// <returns></returns>
        public static Result ValidateProgrammingExperience(string programmingExperience)
        {
            if (!Int32.TryParse(programmingExperience, out int experience))
            {
                return new Result(false, "Нужно ввести чило! Поворите ввод.");
            }
            if (experience <= 0)
            {
                return new Result(false, "Опыт не может быть отрицательным! Поворите ввод.");
            }
            else
            {
                return new Result(true);
            }
        }

        /// <summary>
        /// Провалидировать номер телефона
        /// </summary>
        /// <param name="phoneNumber"> Номер телефона</param>
        /// <returns></returns>
        public static Result ValidaTephoneNumber(string phoneNumber)
        {
            string patternNumber = @"(^\+\d{1,2})?((\(\d{3}\))|(\-?\d{3}\-)|(\d{3}))((\d{3}\-\d{4})|(\d{3}\-\d\d\
-\d\d)|(\d{7})|(\d{3}\-\d\-\d{3}))";
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return new Result(false, "Номер телефона обязателен к заполнению! Поворите ввод.");
            }
            else if (phoneNumber.Length > 12 || !Regex.IsMatch(phoneNumber, patternNumber))
            {
                return new Result(false, "Некорректный номер телефона! Поворите ввод.");
            }
            else
            {
                return new Result(true);
            }
        }
    }
}

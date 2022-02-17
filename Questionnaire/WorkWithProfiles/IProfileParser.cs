using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Предназначен для преобразования данных в объект класса Questionnaire
    /// </summary>
    internal interface IProfileParser
    {
        /// <summary>
        /// Преобразует строковое представление анкеты в объект класса Questionnaire
        /// </summary>
        /// <param name="questionnaire"> Строковое представление анкеты </param>
        Profile ParseToProfile(string[] profile);

        /// <summary>
        /// Получить набор объектов анкет из указанной директории
        /// </summary>
        /// <param name="questionnaire"> Путь к каталогу с анкетами </param>
        Profile[] ParseToProfiles(string questionnaireDirectory);
    }
}

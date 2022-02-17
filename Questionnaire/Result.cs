using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Результат
    /// </summary>
    internal class Result
    {
        /// <summary>
        /// Успешность исполнения
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение детализирующее результат
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Инициализировать экземпляр результата
        /// </summary>
        /// <param name="success"> Успешность исполнения </param>
        public Result(bool success): this(success, string.Empty)
        {

        }

        /// <summary>
        /// Инициализировать экземпляр результата
        /// </summary>
        /// <param name="success"> Успешность исполнения </param>
        /// <param name="message"> Сообщение детализирующее результат </param>
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}

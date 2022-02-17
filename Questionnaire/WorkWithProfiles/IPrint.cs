using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Интерфейс для печати
    /// </summary>
    internal interface IPrint<T>
    {
        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="message"> Сообщение для печати </param>
        void Print(T message);
    }
}

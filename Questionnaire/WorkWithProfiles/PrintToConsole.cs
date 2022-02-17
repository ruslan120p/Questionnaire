using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Класс для вывода строк на консоль
    /// </summary>
    internal class PrintToConsole : IPrint<string[]>
    {
        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="message"> строки для печати </param>
        public void Print(string[] lines)
        {
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}

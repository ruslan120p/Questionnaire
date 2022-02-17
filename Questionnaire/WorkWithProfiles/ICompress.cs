using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Интерфейс сжатия файла в zip архив
    /// </summary>
    internal interface ICompress
    {
        /// <summary>
        /// Файл для архивации
        /// </summary>
        string SourceFile { get; set; }

        /// <summary>
        /// Путь куда архивировать
        /// </summary>
        string CompressedPath { get; set; }

        /// <summary>
        /// Возможна ли архивация
        /// </summary>
        bool CanCompress();

        /// <summary>
        /// Архивировать
        /// </summary>
        bool Compress();
    }
}

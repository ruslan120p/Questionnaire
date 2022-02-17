using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire
{
    /// <summary>
    /// Интерфейс сжатия файла в zip архив
    /// </summary>
    internal class ZipCompress : ICompress
    {
        /// <summary>
        /// Файл для архивации
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// Путь куда заархивировать
        /// </summary>
        public string CompressedPath { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса для архивации
        /// </summary>
        /// <param name="sourceFile"> Файл для архивации </param>
        /// <param name="compressedFile"> Путь куда архивировать </param>
        public ZipCompress(string sourceFile, string compressedPath)
        {
            SourceFile = sourceFile;
            CompressedPath = compressedPath;
        }

        /// <summary>
        /// Возможна ли архивация
        /// </summary>
        public bool CanCompress()
        {
            if (!File.Exists(SourceFile))
            {
                Console.WriteLine($"Файл {SourceFile} не найден!");
                return false;
            }
            if (!Directory.Exists(CompressedPath))
            {
                Console.WriteLine($"Директория {CompressedPath} не найдена!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Архивировать
        /// </summary>
        bool ICompress.Compress()
        {
            if (CanCompress())
            {
                using (FileStream sourceStream = new FileStream(SourceFile, FileMode.OpenOrCreate))
                {
                    // поток для записи сжатого файла
                    using (FileStream targetStream = File.Create($"{CompressedPath}\\{Path.GetFileNameWithoutExtension(SourceFile)}.zip"))
                    {
                        // поток архивации
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}

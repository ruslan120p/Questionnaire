using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.IO;

namespace Questionnaire
{
    /// <summary>
    /// Команды анкеты
    /// </summary>
    internal static class QuestionnaireCommands
    {
        /// <summary>
        /// Путь к папке с анкетами
        /// </summary>
        private static readonly string directory = $@"{Directory.GetCurrentDirectory()}\Анкеты\";

        /// <summary>
        /// Анкета
        /// </summary>
        private static Questionnaire questionnaire;

        /// <summary>
        /// Запустить считывание команд из консоли
        /// </summary>
        public static void StartReadCommands()
        {
            Help();
            Console.WriteLine("Выберите действие:");
            while (true)
            {
                var command = Console.ReadLine().Trim().Split(' ');
                switch (command[0])
                {
                    case "-new_profile":
                        FillQuestion();
                        Console.WriteLine("Выберите действие:");
                        break;
                    case "-statistics":
                        Statistics();
                        break;
                    case "-save":
                        Save();
                        break;
                    case "-goto_question":
                    case "-goto_prev_question":
                    case "-restart_profile":
                        Console.WriteLine("Команда доступна только при заполнении анкеты!");
                        break;
                    case "-find":
                        if (command.Length > 1 && !string.IsNullOrEmpty(command[1]))
                        {
                            Find(command[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверная параметры команды!");
                        }
                        break;
                    case "-delete":
                        if (command.Length > 1 && !string.IsNullOrEmpty(command[1]))
                        {
                            Delete(command[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверная параметры команды!");
                        }
                        break;
                    case "-list":
                        List();
                        break;
                    case "-list_today":
                        ListToday();
                        break;
                    case "-zip":
                        if (command.Length > 2 && !string.IsNullOrEmpty(command[1]) && !string.IsNullOrEmpty(command[2]))
                        {
                            Zip(command[1], command[2]);
                        }
                        else
                        {
                            Console.WriteLine("Неверная параметры команды!");
                        }
                        break;
                    case "-help":
                        Help();
                        break;
                    case "-exit":
                        Exit();
                        break;
                    default:
                        Console.WriteLine("Неверная команда!");
                        break;
                }
            }
        }

        /// <summary>
        /// Проверить является ли строка коммандой и вернуть номер вопроса
        /// </summary>
        private static int ReadQuestionnaireCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return 0;
            }
            var parameters = command.Split(' ');
            int question = 0;
            // "-1" - нет перехода на другой вопрос
            // "-2" - предыдущий вопрос
            switch (parameters[0])
            {
                case "-goto_question":
                    if (string.IsNullOrEmpty(parameters[1]) || !Int32.TryParse(parameters[1], out question) || question < 1)
                    {
                        Console.WriteLine("Неверные параметры команды");
                    }
                    return question - 1;
                case "-goto_prev_question":
                    return -2;
                case "-restart_profile":
                    return 0;
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Заполнить вопросы анкеты
        /// </summary>
        private static void FillQuestion()
        {
            Result validateResult;
            // Поля анкеты
            string fullName = string.Empty;
            DateTime dateOfBirth = new DateTime();
            string favoriteLanguage = string.Empty;
            int programmingExperience = default(int);
            string phoneNumber = string.Empty;

            for (int i = 1; i <= 5; i++)
            {
                validateResult = new Result(true);
                switch (i)
                {
                    case 1:
                        Console.WriteLine("1. ФИО:");
                        do
                        {
                            fullName = Console.ReadLine();
                            // Проверка на ввод команды вместо данных анкеты
                            if (CheckCommand(fullName, i, out int goToQuestion))
                            {
                                i = goToQuestion;
                                break;
                            }
                            validateResult = QuestionnaireValidation.ValidateFullName(fullName);
                            if (!validateResult.Success)
                            {
                                Console.WriteLine(validateResult.Message);
                            }
                        }
                        while (!validateResult.Success);
                        break;
                    case 2:
                        Console.WriteLine("2. Дата рождения (Формат ДД.ММ.ГГГГ):");
                        do
                        {
                            var command = Console.ReadLine();
                            // Проверка на ввод команды вместо данных анкеты
                            if (CheckCommand(command, i, out int goToQuestion))
                            {
                                i = goToQuestion;
                                break;
                            }
                            validateResult = QuestionnaireValidation.ValidateDateOfBirth(command);
                            if (!validateResult.Success)
                            {
                                Console.WriteLine(validateResult.Message);
                            }
                            else
                            {
                                dateOfBirth = DateTime.Parse(command);
                            }
                        }
                        while (!validateResult.Success);
                        break;
                    case 3:
                        Console.WriteLine("3. Любимый язык программирования (PHP, JavaScript, C, C++, Java, C#, Python, Ruby):");
                        do
                        {
                            favoriteLanguage = Console.ReadLine();
                            // Проверка на ввод команды вместо данных анкеты
                            if (CheckCommand(favoriteLanguage, i, out int goToQuestion))
                            {
                                i = goToQuestion;
                                break;
                            }
                            validateResult = QuestionnaireValidation.ValidateFavoriteLanguage(favoriteLanguage);
                            if (!validateResult.Success)
                            {
                                Console.WriteLine(validateResult.Message);
                            }
                        }
                        while (!validateResult.Success);
                        break;
                    case 4:
                        Console.WriteLine("4. Опыт программирования на указанном языке (Полных лет):");
                        string experience;
                        do
                        {
                            experience = Console.ReadLine();
                            // Проверка на ввод команды вместо данных анкеты
                            if (CheckCommand(experience, i, out int goToQuestion))
                            {
                                i = goToQuestion;
                                break;
                            }
                            validateResult = QuestionnaireValidation.ValidateProgrammingExperience(experience);
                            if (!validateResult.Success)
                            {
                                Console.WriteLine(validateResult.Message);
                            }
                            else
                            {
                                programmingExperience = Convert.ToInt32(experience);
                            }
                        }
                        while (!validateResult.Success);
                        break;
                    case 5:
                        Console.WriteLine("5. Мобильный телефон:");
                        do
                        {
                            phoneNumber = Console.ReadLine();
                            // Проверка на ввод команды вместо данных анкеты
                            if (CheckCommand(phoneNumber, i, out int goToQuestion))
                            {
                                i = goToQuestion;
                                break;
                            }
                            validateResult = QuestionnaireValidation.ValidatephoneNumber(phoneNumber);
                            if (!validateResult.Success)
                            {
                                Console.WriteLine(validateResult.Message);
                            }
                        }
                        while (!validateResult.Success);
                        break;
                }
            }
            // Создание анкеты
            questionnaire = new Questionnaire(fullName, dateOfBirth, favoriteLanguage, programmingExperience, phoneNumber);
        }

        /// <summary>
        /// Проверка на ввод команды вместо данных анкеты
        /// </summary>
        /// <param name="command"> Команда </param>
        /// <param name="questionNumber"> Текущий номер вопроса</param>
        /// <param name="goToQuestion"> Переход на вопрос </param>
        private static bool CheckCommand(string command, int questionNumber, out int goToQuestion)
        {
            goToQuestion = ReadQuestionnaireCommand(command);
            // Если есть переход на другой вопрос или сброс анкеты
            // "-1" - нет перехода на другой вопрос
            // "-2" - предыдущий вопрос
            // "< i" - нельзя перескакивать вопросы 
            if (goToQuestion != -1)
            {
                if (goToQuestion >= questionNumber)
                {
                    Console.WriteLine("Вопросы должны заполняться по порядку!");
                }
                else
                {
                    goToQuestion = goToQuestion == -2 ? questionNumber + goToQuestion : goToQuestion;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Показать статистику всех заполненных анкет
        /// </summary>
        public static void Statistics()
        {
            Console.WriteLine("Статистика анкет:");
            if(!Directory.Exists(directory))
            {
                return;
            }
            var questionnaires = Directory.GetFiles(directory).ToDictionary(k => Path.GetFileName(k), v => File.ReadAllLines(v));
            var years = new List<int>();
            var favoriteLanguage = QuestionnaireValidation.FavoriteLanguages.ToDictionary(k => k.ToUpper(), v => 0);
            var mostExperience = new KeyValuePair<int, string>(0, string.Empty);
            foreach (var questionnaire in questionnaires)
            {
                try
                {
                    years.Add(DateTime.Now.Year - DateTime.Parse(questionnaire.Value[1].Split(new string[] { "Дата рождения: " }, StringSplitOptions.None)[1].Trim()).Year);
                    favoriteLanguage[questionnaire.Value[2].Split(new string[] { "Любимый язык программирования: " }, StringSplitOptions.None)[1].Trim().ToUpper()]++;
                    var experience = Int32.Parse(questionnaire.Value[3].Split(new string[] { "Опыт программирования на указанном языке: " }, StringSplitOptions.None)[1].Trim());
                    if (experience > mostExperience.Key)
                    {
                        mostExperience = new KeyValuePair<int, string>(experience, questionnaire.Value[0].Split(new string[] { "ФИО: " }, StringSplitOptions.None)[1].Trim());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Ошибка чтения анкеты: {questionnaire.Key}");
                }
            }
            var old = (int)years.Average();
            var endNumber = old.ToString().Substring(old.ToString().Length - 1);
            var oldEnd = endNumber == "1" ? "год" : endNumber == "2" || endNumber == "3" || endNumber == "4" ? "года" : "лет";
            Console.WriteLine($@"Средний возраст: { old } { oldEnd }");
            Console.WriteLine($@"Самый популярный язык программирования: { favoriteLanguage.First(l => l.Value == favoriteLanguage.Values.Max()).Key }");
            Console.WriteLine($@"Самый опытный программист: { mostExperience.Value }");
        }

        /// <summary>
        /// Сохранить заполненную анкету
        /// </summary>
        public static void Save()
        {
            if (questionnaire == null)
            {
                Console.WriteLine("Анкета не создана!");
                return;
            }
            Directory.CreateDirectory(directory);
            var path = $"{directory}{questionnaire.fullName}.txt";
            File.WriteAllText(path, questionnaire.GetTextOfQuestionnaire());
            Console.WriteLine($"Файл {path} был Сохранен!");
        }

        /// <summary>
        /// Найти анкету и показать данные анкеты в консоль
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        public static void Find(string questionnaireFileName)
        {
            if (!File.Exists($"{directory}{questionnaireFileName}.txt"))
            {
                Console.WriteLine($"Анкета {questionnaireFileName} не найдена!");
                return;
            }
            var lines = File.ReadAllLines($"{directory}{questionnaireFileName}.txt");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Удалить указанную анкету
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        public static void Delete(string questionnaireFileName)
        {
            if (!File.Exists($"{directory}{questionnaireFileName}.txt"))
            {
                Console.WriteLine($"Анкета {questionnaireFileName} не найдена!");
                return;
            }
            File.Delete($"{directory}{questionnaireFileName}.txt");
            Console.WriteLine($"Анкета {questionnaireFileName} была удалена!");
        }

        /// <summary>
        /// Показать список названий файлов всех сохранённых анкет
        /// </summary>
        public static void List()
        {
            Console.WriteLine("Сохраненные анкеты:");
            foreach (var file in Directory.GetFiles(directory))
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            }
        }

        /// <summary>
        /// Показать список названий файлов всех сохранённых анкет, созданных сегодня
        /// </summary>
        public static void ListToday()
        {
            var directoryInfo = new DirectoryInfo(directory);
            var files = directoryInfo.GetFiles().Where(f => f.CreationTime >= DateTime.Today).Select(f => f.Name);
            Console.WriteLine("Сохраненные анкеты за сегодня:");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }

        /// <summary>
        /// Запаковать указанную анкету в архив и сохранить архив по указанному пути
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        /// <param name="PathToSave"> Путь для сохранения архива </param>
        public static void Zip(string questionnaireFileName, string PathToSave)
        {
            if (!File.Exists($"{directory}{questionnaireFileName}.txt"))
            {
                Console.WriteLine($"Анкета {questionnaireFileName} не найдена!");
                return;
            }
            if (!Directory.Exists(PathToSave))
            {
                Console.WriteLine($"Директория {PathToSave} не найдена!");
                return;
            }
            compress($"{directory}{questionnaireFileName}.txt", $"{PathToSave}\\{questionnaireFileName}.zip");
            Console.WriteLine($"Анкета {questionnaireFileName} была запакована по пути: {PathToSave}\\{questionnaireFileName}.zip");
        }

        /// <summary>
        /// Показать список доступных команд с описанием
        /// </summary>
        public static void Help()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Доступные команды:\n");
            sb.AppendLine("-new_profile - Заполнить новую анкету");
            sb.AppendLine("-statistics - Показать статистику всех заполненных анкет");
            sb.AppendLine("-save - Сохранить заполненную анкету");
            sb.AppendLine("-goto_question <Номер вопроса> -Вернуться к указанному вопросу (Команда доступна только при заполнении анкеты, вводится вместо ответа на любой вопрос)");
            sb.AppendLine("-goto_prev_question - Вернуться к предыдущему вопросу(Команда доступна только при заполнении анкеты, вводится вместо ответа на любой вопрос)");
            sb.AppendLine("-restart_profile - Заполнить анкету заново(Команда доступна только при заполнении анкеты, вводится вместо ответа на любой вопрос)");
            sb.AppendLine("-find <Имя файла анкеты> -Найти анкету и показать данные анкеты в консоль");
            sb.AppendLine("-delete <Имя файла анкеты> -Удалить указанную анкету");
            sb.AppendLine("-list - Показать список названий файлов всех сохранённых анкет");
            sb.AppendLine("-list_today - Показать список названий файлов всех сохранённых анкет, созданных сегодня");
            sb.AppendLine("-zip <Имя файла анкеты> <Путь для сохранения архива> -Запаковать указанную анкету в архив и сохранить архив по указанному пути");
            sb.AppendLine("-help - Показать список доступных команд с описанием");
            sb.AppendLine("-exit - Выйти из приложения");
            Console.WriteLine(sb.ToString());
        }

        /// <summary>
        /// Выйти из приложения
        /// </summary>
        public static void Exit()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Заархивировать
        /// </summary>
        /// <param name="sourceFile"> Файл для архивации </param>
        /// <param name="compressedFile"> Путь куда заархивровать </param>
        private static void compress(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                    }
                }
            }
        }
    }

    /// <summary>
    /// Валидация вопросов анкеты
    /// </summary>
    internal static class QuestionnaireValidation
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
        public static Result ValidatephoneNumber(string phoneNumber)
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

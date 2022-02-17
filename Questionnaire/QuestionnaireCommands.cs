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
    internal class QuestionnaireCommands
    {
        /// <summary>
        /// Путь к папке с анкетами
        /// </summary>
        private readonly string directory = $@"{Directory.GetCurrentDirectory()}\Анкеты\";

        /// <summary>
        /// Анкета
        /// </summary>
        private Profile profile;

        /// <summary>
        /// Запустить считывание команд из консоли
        /// </summary>
        public void StartReadCommands()
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
                            Console.WriteLine("Неверные параметры команды!");
                        }
                        break;
                    case "-delete":
                        if (command.Length > 1 && !string.IsNullOrEmpty(command[1]))
                        {
                            Delete(command[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверные параметры команды!");
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
                            Console.WriteLine("Неверные параметры команды!");
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
        private int ReadQuestionnaireCommand(string command)
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
        public void FillQuestion()
        {
            Result validateResult;
            int goToQuestion = default(int);
            // Поля анкеты
            string fullName = default(string);
            DateTime dateOfBirth = default(DateTime);
            string favoriteLanguage = default(string);
            int programmingExperience = default(int);
            string phoneNumber = default(string);

            for (int i = 1; i <= 5; i++)
            {
                validateResult = new Result(true);
                switch (i)
                {
                    case 1:
                        Console.WriteLine("1. ФИО:");
                        fullName = Console.ReadLine();
                        // Проверка на ввод команды вместо данных анкеты
                        if (CheckCommand(fullName, i, out goToQuestion))
                        {
                            i = goToQuestion;
                            break;
                        }
                        validateResult = QuestionValidation.ValidateFullName(fullName);
                        if (!validateResult.Success)
                        {
                            Console.WriteLine(validateResult.Message);
                        }
                        break;
                    case 2:
                        Console.WriteLine("2. Дата рождения (Формат ДД.ММ.ГГГГ):");
                        var command = Console.ReadLine();
                        // Проверка на ввод команды вместо данных анкеты
                        if (CheckCommand(command, i, out goToQuestion))
                        {
                            i = goToQuestion;
                            break;
                        }
                        validateResult = QuestionValidation.ValidateDateOfBirth(command);
                        if (!validateResult.Success)
                        {
                            Console.WriteLine(validateResult.Message);
                        }
                        else
                        {
                            dateOfBirth = DateTime.Parse(command);
                        }
                        break;
                    case 3:
                        Console.WriteLine("3. Любимый язык программирования (PHP, JavaScript, C, C++, Java, C#, Python, Ruby):");
                        favoriteLanguage = Console.ReadLine();
                        // Проверка на ввод команды вместо данных анкеты
                        if (CheckCommand(favoriteLanguage, i, out goToQuestion))
                        {
                            i = goToQuestion;
                            break;
                        }
                        validateResult = QuestionValidation.ValidateFavoriteLanguage(favoriteLanguage);
                        if (!validateResult.Success)
                        {
                            Console.WriteLine(validateResult.Message);
                        }
                        break;
                    case 4:
                        Console.WriteLine("4. Опыт программирования на указанном языке (Полных лет):");
                        string experience;
                        experience = Console.ReadLine();
                        // Проверка на ввод команды вместо данных анкеты
                        if (CheckCommand(experience, i, out goToQuestion))
                        {
                            i = goToQuestion;
                            break;
                        }
                        validateResult = QuestionValidation.ValidateProgrammingExperience(experience);
                        if (!validateResult.Success)
                        {
                            Console.WriteLine(validateResult.Message);
                        }
                        else
                        {
                            programmingExperience = Convert.ToInt32(experience);
                        }
                        break;
                    case 5:
                        Console.WriteLine("5. Мобильный телефон:");
                        phoneNumber = Console.ReadLine();
                        // Проверка на ввод команды вместо данных анкеты
                        if (CheckCommand(phoneNumber, i, out goToQuestion))
                        {
                            i = goToQuestion;
                            break;
                        }
                        validateResult = QuestionValidation.ValidaTephoneNumber(phoneNumber);
                        if (!validateResult.Success)
                        {
                            Console.WriteLine(validateResult.Message);
                        }
                        break;
                }
                // Если ввели некорректные данные возвращаемся к этому же вопросу
                if (!validateResult.Success)
                {
                    i--;
                }
            }
            // Создание анкеты
            profile = new Profile(fullName, dateOfBirth, favoriteLanguage, programmingExperience, phoneNumber);
        }

        /// <summary>
        /// Проверка на ввод команды вместо данных анкеты
        /// </summary>
        /// <param name="command"> Команда </param>
        /// <param name="questionNumber"> Текущий номер вопроса</param>
        /// <param name="goToQuestion"> Переход на вопрос </param>
        private bool CheckCommand(string command, int questionNumber, out int goToQuestion)
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
        /// Показать статистику всех заполненных анкет.
        /// Анкеты берутся из каталога по-умолчанию "Анкеты"
        /// </summary>
        public void Statistics()
        {
            if (!DirectoryExist())
            {
                return;
            }
            var years = new List<int>();
            var favoriteLanguage = QuestionValidation.FavoriteLanguages.ToDictionary(k => k.ToUpper(), v => 0);
            var mostExperience = new KeyValuePair<int, string>(0, string.Empty);
            // Получение анкет из каталога с анкетами
            IProfileParser parser = new ProfileParser();
            Profile[] profiles = null;
            try
            {
                profiles = parser.ParseToProfiles(directory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"Ошибка чтения анкеты!");
                return;
            }
            if (profiles.Count() == 0)
            {
                Console.WriteLine("Анкеты не созданы!");
                return;
            }
            // Вычисление статистических данных
            foreach (var profile in profiles)
            {
                years.Add(DateTime.Now.Year - profile.DateOfBirth.Year);
                favoriteLanguage[profile.FavoriteLanguage.ToUpper()]++;
                var experience = profile.ProgrammingExperience;
                if (experience > mostExperience.Key)
                {
                    mostExperience = new KeyValuePair<int, string>(experience, profile.FullName);
                }
            }
            var old = (int)years.Average();
            var endNumber = old.ToString().Substring(old.ToString().Length - 1);
            var oldEnd = endNumber == "1" ? "год" : endNumber == "2" || endNumber == "3" || endNumber == "4" ? "года" : "лет";

            Console.WriteLine($@"Статистика анкет:
Средний возраст: { old } { oldEnd }
Самый популярный язык программирования: { favoriteLanguage.First(l => l.Value == favoriteLanguage.Values.Max()).Key }
Самый опытный программист: { mostExperience.Value }");
        }

        /// <summary>
        /// Сохранить заполненную анкету
        /// </summary>
        public void Save()
        {
            IProfileSaver profileSaver = new ProfileSaver(profile, directory);
            profileSaver.Save();
        }

        /// <summary>
        /// Найти анкету и показать данные анкеты в консоль
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        public void Find(string questionnaireFileName)
        {
            string[] lines;
            if (!TryGetQuestionnaireFileName(ref questionnaireFileName))
            {
                Console.WriteLine($"Анкета {questionnaireFileName} не найдена!");
                return;
            }
            lines = File.ReadAllLines(questionnaireFileName);
            IPrint<string[]> printer = new PrintToConsole();
            printer.Print(lines);
        }

        /// <summary>
        /// Удалить указанную анкету
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        public void Delete(string questionnaireFileName)
        {
            if (!TryGetQuestionnaireFileName(ref questionnaireFileName))
            {
                Console.WriteLine($"Анкета {questionnaireFileName} не найдена!");
                return;
            }
            File.Delete(questionnaireFileName);
            Console.WriteLine($"Анкета {questionnaireFileName} была удалена!");
        }

        /// <summary>
        /// Получить путь к файлу анкеты в корректном представлении
        /// </summary>
        /// <param name="questionnaireFileName"></param>
        /// <returns></returns>
        private bool TryGetQuestionnaireFileName(ref string questionnaireFileName)
        {
            questionnaireFileName = Path.ChangeExtension(questionnaireFileName, ".txt");
            // Если задан путь к анкете
            if (File.Exists(questionnaireFileName))
            {
                return true;
            }
            else if (File.Exists(directory + questionnaireFileName))
            {
                questionnaireFileName = directory + questionnaireFileName;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Показать список названий файлов всех сохранённых анкет
        /// </summary>
        public void List()
        {
            if (!DirectoryExist())
            {
                return;
            }
            Console.WriteLine("Сохраненные анкеты:");
            foreach (var file in Directory.GetFiles(directory))
            {
                Console.WriteLine(Path.GetFileNameWithoutExtension(file));
            }
        }

        /// <summary>
        /// Показать список названий файлов всех сохранённых анкет, созданных сегодня
        /// </summary>
        public void ListToday()
        {
            if (!DirectoryExist())
            {
                return;
            }
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
        public void Zip(string questionnaireFileName, string PathToSave)
        {
            ICompress compress = new ZipCompress(questionnaireFileName, PathToSave);
            if (compress.Compress())
            {
                Console.WriteLine($"Анкета {questionnaireFileName} была запакована по пути: {PathToSave}\\{Path.GetFileNameWithoutExtension(questionnaireFileName)}.zip");
            }
        }

        /// <summary>
        /// Показать список доступных команд с описанием
        /// </summary>
        public void Help()
        {
            var message = $@"Доступные команды:
-new_profile - Заполнить новую анкету
-statistics - Показать статистику всех заполненных анкет
-save - Сохранить заполненную анкету
-goto_question <Номер вопроса> -Вернуться к указанному вопросу (Команда доступна только при заполнении анкеты, вводится вместо ответа на любой вопрос)
-goto_prev_question - Вернуться к предыдущему вопросу(Команда доступна только при заполнении анкеты, вводится вместо ответа на любой вопрос)
-restart_profile - Заполнить анкету заново(Команда доступна только при заполнении анкеты, вводится вместо ответа на любой вопрос)
-find <Имя файла анкеты> -Найти анкету и показать данные анкеты в консоль
-delete <Имя файла анкеты> -Удалить указанную анкету
-list - Показать список названий файлов всех сохранённых анкет
-list_today - Показать список названий файлов всех сохранённых анкет, созданных сегодня
-zip <Имя файла анкеты> <Путь для сохранения архива> -Запаковать указанную анкету в архив и сохранить архив по указанному пути
-help - Показать список доступных команд с описанием
-exit - Выйти из приложения
";
            Console.WriteLine(message);
        }

        /// <summary>
        /// Выйти из приложения
        /// </summary>
        public void Exit()
        {
            Environment.Exit(0);
        }

        private bool DirectoryExist()
        {
            if (Directory.Exists(directory))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Не созданно ни одной анкеты!");
                return false;
            }
        }
    }
}

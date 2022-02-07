namespace Questionnaire
{
    /// <summary>
    /// Команды анкеты
    /// </summary>
    internal interface ICommands
    {
        /// <summary>
        /// Запустить считывание команд из консоли
        /// </summary>
        void StartReadCommands();

        /// <summary>
        /// Показать статистику всех заполненных анкет
        /// </summary>
        void Statistics();

        /// <summary>
        /// Сохранить заполненную анкету
        /// </summary>
        void Save();

        /// <summary>
        /// Найти анкету и показать данные анкеты в консоль
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        void Find(string questionnaireFileName);

        /// <summary>
        /// Удалить указанную анкету
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        void Delete(string questionnaireFileName);

        /// <summary>
        /// Показать список названий файлов всех сохранённых анкет
        /// </summary>
        void List();

        /// <summary>
        /// Показать список названий файлов всех сохранённых анкет, созданных сегодня
        /// </summary>
        void List_today();

        /// <summary>
        /// Запаковать указанную анкету в архив и сохранить архив по указанному пути
        /// </summary>
        /// <param name="questionnaireFileName"> Имя файла анкеты </param>
        /// <param name="PathToSave"> Путь для сохранения архива </param>
        void Zip(string questionnaireFileName, string PathToSave);

        /// <summary>
        /// Показать список доступных команд с описанием
        /// </summary>
        void Help();

        /// <summary>
        /// Выйти из приложения
        /// </summary>
        void Exit();
    }
}

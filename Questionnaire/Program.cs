namespace Questionnaire
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var questionnaireProcces = new QuestionnaireCommands();
            questionnaireProcces.StartReadCommands();
        }
    }
}

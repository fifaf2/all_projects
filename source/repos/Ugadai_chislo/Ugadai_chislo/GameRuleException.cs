namespace Ugadai_chislo
{
    // Бросается только когда попытки кончились
    public class GameRuleException : Exception
    {
        public GameRuleException(int secretNumber)
            : base("Попытки закончились. Было загадано число " + secretNumber)
        {
        }
    }
}

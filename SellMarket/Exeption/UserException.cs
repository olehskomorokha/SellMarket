namespace SellMarket.Exeption
{
    public class UserException : Exception
    {
        public UserException(string message): base(message)
        {
            Console.WriteLine(message);
        }
    }
}


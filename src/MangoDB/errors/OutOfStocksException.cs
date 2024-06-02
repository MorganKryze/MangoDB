namespace MangoDB
{
    public class OutOfStockException : Exception
    {
        public OutOfStockException()
            : base("Out of stocks.") { }

        public OutOfStockException(string message)
            : base(message) { }

        public OutOfStockException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

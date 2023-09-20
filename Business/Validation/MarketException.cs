using System;

namespace Business.Validation
{
    public class MarketException : Exception
    {
        public MarketException() : base()
        {
        }

        public MarketException(string message) : base(message)
        {
        }
        public MarketException(string message,Exception innerException) : base(message,innerException)
        {
        }
        
    }
}
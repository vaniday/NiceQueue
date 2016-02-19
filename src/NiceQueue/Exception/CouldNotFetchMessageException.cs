using System;

namespace NiceQueue
{
    public class CouldNotFetchMessageException : Exception
    {
        public CouldNotFetchMessageException(string message) : base(message) { }
    }
}

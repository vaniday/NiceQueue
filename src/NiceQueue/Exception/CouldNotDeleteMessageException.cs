using System;

namespace NiceQueue
{
    public class CouldNotDeleteMessageException : Exception
    {
        public CouldNotDeleteMessageException(string message) : base(message) { }
    }
}

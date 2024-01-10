using System;

namespace Core.Event
{
    public class DispatcherException: Exception
    {
        public DispatcherException(string message) : base(message)
        {
        }
    }
}
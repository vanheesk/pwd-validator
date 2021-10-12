using System;

namespace PwdValidator.Service.Exceptions
{
    
    public class MissingArgumentException : Exception
    {

        public MissingArgumentException(string message)
            : base(message)
        { }

    }
    
}
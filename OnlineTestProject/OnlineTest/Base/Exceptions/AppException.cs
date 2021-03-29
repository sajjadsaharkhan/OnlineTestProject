using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTest.Base.Exceptions
{
    public class AppException : Exception
    {
        public Dictionary<string, string> ValidationExceptions { get; private set; }

        public AppException(string message) : base(message) { }

        public AppException(string key, string value) : base(MessagesDictionary.ValidationErrorMessage)
        {
            this.ValidationExceptions = new Dictionary<string, string>() { { key.ToLower(), value } };
        }
    }
}

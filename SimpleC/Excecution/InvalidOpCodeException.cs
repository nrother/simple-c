using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Excecution
{
    [Serializable]
    public class InvalidOpCodeException : Exception
    {
        public InvalidOpCodeException() { }
        public InvalidOpCodeException(string message) : base(message) { }
        public InvalidOpCodeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidOpCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

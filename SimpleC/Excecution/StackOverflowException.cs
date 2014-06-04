using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Excecution
{
    [Serializable]
    public class StackOverflowException : Exception
    {
        public StackOverflowException() { }
        public StackOverflowException(string message) : base(message) { }
        public StackOverflowException(string message, Exception inner) : base(message, inner) { }
        protected StackOverflowException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

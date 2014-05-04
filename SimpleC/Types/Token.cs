using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types
{
    abstract class Token
    {
        public string Content { get; private set; }

        public Token(string content)
        {
            this.Content = content;
        }

        public override string ToString()
        {
            return string.Format("[{0}] - {1}", this.GetType().Name, Content); //TODO: Remove the last "Token" in Name
        }
    }
}

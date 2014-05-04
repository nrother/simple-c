using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.Tokens
{
    class OpenBraceToken : BraceToken
    {
        public OpenBraceToken(string content)
            : base(content)
        {
            if (content != "(")
                throw new ArgumentException("The content is no opening brace.", "content");
        }
    }
}

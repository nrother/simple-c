using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.Tokens
{
    class IdentifierToken : Token
    {
        public IdentifierToken(string content)
            :base(content)
        { }
    }
}

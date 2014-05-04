using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.Tokens
{
    class CloseBraceToken : BraceToken
    {
        public CloseBraceToken(string content)
            : base(content)
        {
            switch (content)
            {
                case ")":
                    BraceType = BraceType.Round;
                    break;
                case "]":
                    BraceType = BraceType.Square;
                    break;
                case "}":
                    BraceType = BraceType.Curly;
                    break;
                default:
                    throw new ArgumentException("The content is no closing brace.", "content");
            }
        }
    }
}

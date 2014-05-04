using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class ExpressionNode
    {
        protected ExpressionNode()
        { }

        static ExpressionNode CreateFromTokens(IEnumerable<Token> tokens)
        {
            return null;
        }
    }
}

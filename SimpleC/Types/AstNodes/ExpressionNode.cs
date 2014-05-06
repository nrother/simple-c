using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    abstract class ExpressionNode
    {
        protected ExpressionNode()
        { }

        public static ExpressionNode CreateFromTokens(IEnumerable<Token> tokens)
        {
            if (tokens.Count() == 0)
                return null;

            //Now we need to parse the given tokens into a expression tree.
        }

        public static ExpressionNode CreateConstantExpression(int value)
        {
            return new NumberLiteralNode(value);
        }
    }
}

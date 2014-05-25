using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class ReturnStatementNode : AstNode
    {
        /// <summary>
        /// A expression for the value returned. Might be null, if
        /// no value is returned.
        /// </summary>
        public ExpressionNode ValueExpression { get; private set; }

        public ReturnStatementNode(ExpressionNode valueExpr)
        {
            ValueExpression = valueExpr;
        }
    }
}

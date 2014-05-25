using SimpleC.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class IfStatementNode : StatementSequenceNode
    {
        public ExpressionNode Condition { get; private set; }

        public IfStatementNode(ExpressionNode cond)
        {
            if (cond == null)
                throw new ParsingException("Parser: An If statmentent must conatain a condition!");

            Condition = cond;
        }
    }
}

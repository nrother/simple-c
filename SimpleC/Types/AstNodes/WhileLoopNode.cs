using SimpleC.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class WhileLoopNode : LoopStatementNode
    {
        public ExpressionNode Condition { get; private set; }

        public WhileLoopNode(ExpressionNode cond)
        {
            if (cond == null)
                throw new ParsingException("Parser: An while loop must conatain a condition!");
        }
    }
}

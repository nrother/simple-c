using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class NumberLiteralNode : ExpressionNode
    {
        public int Value { get; private set; }

        public NumberLiteralNode(int value)
        {
            Value = value;
        }
    }
}

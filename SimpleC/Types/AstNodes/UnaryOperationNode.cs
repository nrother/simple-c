using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class UnaryOperationNode : ExpressionNode
    {
        public ExpressionOperationType OperationType { get; private set; }
        public ExpressionNode Operand { get; private set; }

        private static readonly ExpressionOperationType[] validOperators = { ExpressionOperationType.Negate, ExpressionOperationType.Not };

        public UnaryOperationNode(ExpressionOperationType opType, ExpressionNode operand)
        {
            if (!validOperators.Contains(opType))
                throw new ArgumentException("Invalid unary operator given!", "opType");

            OperationType = opType;
            Operand = operand;
        }
    }
}

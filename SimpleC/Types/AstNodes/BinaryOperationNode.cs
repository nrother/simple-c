using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class BinaryOperationNode : ExpressionNode
    {
        public ExpressionOperationType OperationType { get; private set; }
        public ExpressionNode OperandA { get; private set; }
        public ExpressionNode OperandB { get; private set; }

        private static readonly ExpressionOperationType[] validOperators = 
        {
            ExpressionOperationType.Add,
            ExpressionOperationType.Substract,
            ExpressionOperationType.Multiply,
            ExpressionOperationType.Divide,
            ExpressionOperationType.Modulo,
            ExpressionOperationType.Assignment,
            ExpressionOperationType.Equals,
            ExpressionOperationType.GreaterThan,
            ExpressionOperationType.LessThan,
            ExpressionOperationType.GreaterEquals,
            ExpressionOperationType.LessEquals,
            ExpressionOperationType.NotEquals,
            ExpressionOperationType.Not,
            ExpressionOperationType.And,
            ExpressionOperationType.Or,
        };

        public BinaryOperationNode(ExpressionOperationType opType, ExpressionNode operandA, ExpressionNode operandB)
        {
            if (!validOperators.Contains(opType))
                throw new ArgumentException("Invalid binary operator given!", "opType");

            OperationType = opType;
            OperandA = operandA;
            OperandB = operandB;
        }
    }
}

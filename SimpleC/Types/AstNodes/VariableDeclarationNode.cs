using SimpleC.Types.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class VariableDeclarationNode : AstNode
    {
        public ExpressionNode InitialValueExpression { get; private set; }
        public VariableType Type { get; private set; }

        public string Name { get; private set; }

        private static readonly ExpressionNode DefaultIntValueExpression = ExpressionNode.CreateConstantExpression(0); //the default value for an int is zero (0).
        
        /// <summary>
        /// Creates a new instance of the VariableDeclarationNode class.
        /// </summary>
        /// <param name="type">The type of the variable.</param>
        /// <param name="name">The name of the variable.</param>
        /// <param name="initialValue">A expression used to initialise the variable initially or null to use the default value.</param>
        public VariableDeclarationNode(VariableType type, string name, ExpressionNode initialValue)
        {
            Type = type;
            Name = name;

            initialValue = initialValue ?? DefaultIntValueExpression;
            InitialValueExpression = initialValue;
        }
    }
}

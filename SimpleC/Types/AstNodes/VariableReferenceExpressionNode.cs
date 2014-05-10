using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class VariableReferenceExpressionNode : ExpressionNode
    {
        public string VariableName { get; private set; }

        public VariableReferenceExpressionNode(string varName)
        {
            VariableName = varName;
        }
    }
}

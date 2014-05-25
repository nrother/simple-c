using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class FunctionCallExpressionNode : ExpressionNode
    {
        public string FunctionName { get; private set; }

        public IEnumerable<ExpressionNode> Arguments
        {
            get
            {
                return arguments;
            }
        }

        public int ArgumentCount
        {
            get { return arguments.Count; }
        }

        private List<ExpressionNode> arguments;

        public FunctionCallExpressionNode(string functionName)
        {
            FunctionName = functionName;

            arguments = new List<ExpressionNode>();
        }

        public FunctionCallExpressionNode(string functionName, params ExpressionNode[] args)
            : this(functionName)
        {
            AddArguments(args);
        }

        public void AddArgument(ExpressionNode arg)
        {
            arguments.Add(arg);
        }

        public void AddArguments(IEnumerable<ExpressionNode> args)
        {
            arguments.AddRange(args);
        }
    }
}

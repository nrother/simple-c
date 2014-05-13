using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class FunctionDeclarationNode : StatementSequenceNode
    {
        public string FunctionName { get; private set; }

        public FunctionDeclarationNode(string name)
        {
            FunctionName = name;
        }
    }
}

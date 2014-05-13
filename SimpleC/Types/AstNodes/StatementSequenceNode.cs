using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleC.Types.AstNodes
{
    class StatementSequenceNode : AstNode
    {
        public IEnumerable<AstNode> SubNodes
        {
            get
            {
                return subNodes;
            }
        }

        List<AstNode> subNodes;

        public StatementSequenceNode()
        {
            subNodes = new List<AstNode>();
        }

        public StatementSequenceNode(IEnumerable<AstNode> subNodes)
        {
            this.subNodes.AddRange(subNodes);
        }

        public void AddStatement(AstNode node)
        {
            subNodes.Add(node);
        }
    }
}

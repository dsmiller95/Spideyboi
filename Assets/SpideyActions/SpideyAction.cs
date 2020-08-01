using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SpideyActions
{
    public interface ISpideyAction
    {
        void DoAction(GraphManager graph, INode<NodeBehavior> currentNode, INode<NodeBehavior> lastNode);
    }
}

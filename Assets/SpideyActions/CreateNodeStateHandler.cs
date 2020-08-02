using QuikGraph;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Utilities
{
    public class CreateNodeStateHandler : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public CreateNodeStateHandler(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly crawly)
        {
            var graph = crawly.graphManager;
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnection.GetOtherVertex(lastNode);
            var tentativeNext = crawly.PickNextConnection().GetOtherVertex(currentNode);
            var newNode = graph.CreateNewNodeWithConnectionBetweenTwoConnectionsAtBase(
                currentNode,
                lastNode,
                tentativeNext,
                graph.initialConnectionLength,
                crawly.whichSide == TraversalSide.LEFTHAND);

            var rigidBody = newNode.GetComponent<Rigidbody2D>();
            rigidBody.mass = rigidBody.mass / 10;
            var connection = graph.CreateConnection(currentNode, newNode, graph.initialConnectionLength);

            var totalDelay = 700;
            var lengthSteps = 10;

            for (var i = 0; i <= lengthSteps; i++)
            {
                await Task.Delay(totalDelay / lengthSteps);
                connection.targetDistance = Mathf.Lerp(graph.initialConnectionLength, graph.defaultConnectionLength, (float)i / lengthSteps);
            }

            rigidBody.mass = rigidBody.mass * 10;
            return returnToOnsuccess;
        }


        public void TransitionIntoState(SpiderCrawly data)
        {
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}
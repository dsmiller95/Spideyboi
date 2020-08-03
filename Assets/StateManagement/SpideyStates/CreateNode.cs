using QuikGraph;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SpideyActions.SpideyStates
{
    public class CreateNode : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public CreateNode(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
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
            rigidBody.mass = rigidBody.mass / 20;
            rigidBody.velocity *= 20;
            var connection = graph.CreateConnection(currentNode, newNode, graph.initialConnectionLength);

            connection.targetDistance = graph.defaultConnectionLength;

            var spring = connection.GetComponentInChildren<RealSpring>();
            spring.springConstant /= 5;

            var totalDelay = 700;
            var lengthSteps = 10;
            await Task.Delay(totalDelay/2);
            await Task.Delay(totalDelay / 2);


            //for (var i = 0; i <= lengthSteps; i++)
            //{
            //    await Task.Delay(totalDelay / lengthSteps);
            //    connection.targetDistance = Mathf.Lerp(graph.initialConnectionLength, graph.defaultConnectionLength, (float)i / lengthSteps);
            //}

            spring.springConstant *= 5;
            rigidBody.mass = rigidBody.mass * 20;
            rigidBody.velocity /= 30;

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
using QuikGraph;
using System.Collections.Generic;
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
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);
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

            connection.connectionRenderer.GetComponent<Collider2D>().isTrigger = true;
            connection.connectionRenderer.OnCollided = (otherCollider) =>
            {
                if (IsBreakingCollision(otherCollider, crawly))
                {
                    Debug.Log("Broke!");
                    crawly.graphManager.DestroyConnection(connection);
                }
            };


            spring.springConstant /= 2;

            var totalDelay = (int)(700 / Time.timeScale);
            var lengthSteps = 10;
            await Task.Delay(totalDelay / 2);
            if (connection != null)
            {
                //spring.springConstant *= 5;
                rigidBody.mass = rigidBody.mass * 20;
                rigidBody.velocity /= 30;
            }

            await Task.Delay(totalDelay / 2);


            //for (var i = 0; i <= lengthSteps; i++)
            //{
            //    await Task.Delay(totalDelay / lengthSteps);
            //    connection.targetDistance = Mathf.Lerp(graph.initialConnectionLength, graph.defaultConnectionLength, (float)i / lengthSteps);
            //}
            if (connection != null)
            {
                // connection might have gotten destroyed
                connection.connectionRenderer.GetComponent<Collider2D>().isTrigger = false;
                connection.connectionRenderer.OnCollided = null;

                spring.springConstant *= 2;
                //rigidBody.mass = rigidBody.mass * 20;
                //rigidBody.velocity /= 30;
            }

            return returnToOnsuccess;
        }
        private bool IsBreakingCollision(Collider2D other, SpiderCrawly spidey)
        {
            if (!spidey.CanBreakWeb(other))
            {
                return false;
            }

            var connection = other.GetComponentInParent<Connection>();
            var node = other.GetComponentInParent<NodeBehavior>();
            if (connection == null && node == null)
            {
                return true;
            }
            if (connection != null)
            {
                var exceptedConnections = new List<Connection>() { spidey.CurrentConnection };
                //exceptedConnections.AddRange(spidey.graph.AdjacentEdges(spidey.currentDraggingNode));
                if (spidey.distanceAlongConnectionForInspector >= .9)
                {
                    var otherNode = spidey.CurrentConnection.GetOtherVertex(spidey.lastNode);
                    exceptedConnections.AddRange(spidey.graph.AdjacentEdges(otherNode));
                }
                else if (spidey.DistanceAlongConnection <= .1)
                {
                    exceptedConnections.AddRange(spidey.graph.AdjacentEdges(spidey.lastNode));
                }
                return !exceptedConnections.Contains(connection);
            }
            if (node != null)
            {
                var exceptedNodes = new List<NodeBehavior>() { };
                var connectionPair = spidey.CurrentConnection.ToVertexPair();
                exceptedNodes.Add(connectionPair.Source);
                exceptedNodes.Add(connectionPair.Target);

                return !exceptedNodes.Contains(node);
            }
            return true;
        }


        public void TransitionIntoState(SpiderCrawly data)
        {
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}
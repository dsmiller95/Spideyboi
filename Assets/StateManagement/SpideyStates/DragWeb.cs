using QuikGraph;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SpideyActions.SpideyStates
{
    public class DragWeb : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public DragWeb(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public GenericStateHandler<SpiderCrawly> HandleState(SpiderCrawly crawly)
        {
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);
            crawly.draggingLineRenderer.InstantSetConnection(crawly.gameObject, currentNode.gameObject);

            crawly.currentDraggingNode = currentNode;

            crawly.draggingLineRenderer.OnCollided = (otherCollider) =>
            {
                if (IsBreakingCollision(otherCollider, crawly))
                {
                    Debug.Log("Broke!");
                    crawly.draggingLineRenderer.InstantClearConnection();
                }
            };

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
                exceptedConnections.AddRange(spidey.graph.AdjacentEdges(spidey.currentDraggingNode));
                if (spidey.distanceAlongConnectionForInspector >= .9)
                {
                    var otherNode = spidey.CurrentConnection.GetOtherVertex(spidey.lastNode);
                    exceptedConnections.AddRange(spidey.graph.AdjacentEdges(otherNode));
                }
                return !exceptedConnections.Contains(connection);
            }
            if (node != null)
            {
                var exceptedNodes = new List<NodeBehavior>() { spidey.currentDraggingNode };
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
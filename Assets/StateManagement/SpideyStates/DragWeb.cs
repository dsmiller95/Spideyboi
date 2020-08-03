using QuikGraph;
using System.Threading.Tasks;
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

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly crawly)
        {
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);
            crawly.draggingLineRenderer.gameObject.SetActive(true);
            crawly.draggingLineRenderer.Source = crawly.gameObject;
            crawly.draggingLineRenderer.Target = currentNode.gameObject;

            crawly.currentDraggingConnection = currentNode;

            crawly.draggingLineRenderer.OnCollided = (otherCollider) =>
            {
                if (IsBreakingCollision(otherCollider, crawly))
                {
                    Debug.Log("Broke!");
                    //crawly.draggingLineRenderer.gameObject.SetActive(false);
                }
            };

            return returnToOnsuccess;
        }

        private bool IsBreakingCollision(Collider2D other, SpiderCrawly spidey)
        {
            var connection = other.GetComponentInParent<Connection>();
            var node = other.GetComponentInParent<NodeBehavior>();
            if (connection == null && node == null)
            {
                return true;
            }
            if(connection != null)
            {
                // will only break if colliding with a connection not connected to the origin node, or one we are currently on
                if (connection == spidey.currentConnectionForInspector)
                {
                    return false;
                }
                var connectionPair = connection.ToVertexPair();
                if (connectionPair.Source == spidey.currentDraggingConnection || connectionPair.Target == spidey.currentDraggingConnection)
                {
                    return false;
                }
            }
            if(node != null)
            {
                if(node == spidey.currentDraggingConnection)
                {
                    return false;
                }
                var connectionPair = spidey.currentConnectionForInspector.ToVertexPair();
                if (connectionPair.Source == node || connectionPair.Target == node)
                {
                    return false;
                }
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
using QuikGraph;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class SpiderCrawly : MonoBehaviour
    {
        public GraphManager graphManager;

        public float movementSpeed = 1;

        private UndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>> graph => graphManager.Graph;

        private INode<NodeBehavior> lastNode;
        private Connection<NodeBehavior> currentConnection;
        private float distanceAlongConnection = 0f;


        private void Start()
        {
            var vertices = graph.Vertices.ToList();
            lastNode = vertices[Random.Range(0, vertices.Count)];
            PickRandomConnection();
        }

        private bool PickRandomConnection()
        {
            var connections = graph.AdjacentEdges(lastNode).ToList();
            if (connections.Count > 0)
            {
                currentConnection = connections[Random.Range(0, connections.Count)];
                return true;
            }
            return false;
        }

        private void TraverseNext()
        {
            lastNode = currentConnection.GetOtherVertex(lastNode);
            PickRandomConnection();
        }

        public void Update()
        {
            distanceAlongConnection += Time.deltaTime * movementSpeed;

            var a = lastNode.GetData().transform.position;
            var b = currentConnection.GetOtherVertex(lastNode).GetData().transform.position;
            var diff = b - a;

            if (diff.magnitude <= distanceAlongConnection)
            {
                TraverseNext();
                distanceAlongConnection = 0;
                transform.position = b;
                return;
            }

            var scaledDiff = diff.normalized * distanceAlongConnection;
            this.transform.position = a + scaledDiff;
        }

    }
}

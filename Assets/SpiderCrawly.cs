using Assets.SpideyActions;
using QuikGraph;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{


    public class SpiderCrawly : MonoBehaviour
    {
        public GraphManager graphManager;

        public float movementSpeed = 1;
        public float sideOffset = 1;

        public UndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>> graph => graphManager.Graph;

        private INode<NodeBehavior> lastNode;
        private Connection<NodeBehavior> currentConnection;
        private float distanceAlongConnection = 0f;
        private TraversalSide whichSide = TraversalSide.INSIDE;

        private IList<ISpideyAction> actions;
        private int indexInSpideyActions;

        enum TraversalSide
        {
            INSIDE,
            OUTSIDE
        }

        private void Start()
        {
            actions = new ISpideyAction[]
            {
                new CreateNodeSpideyAction(),
                new DoNothingSpideyAction(),
            };
            indexInSpideyActions = 0;

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

        private void AdvanceSpideyActions(INode<NodeBehavior> current, INode<NodeBehavior> previous)
        {
            actions[indexInSpideyActions].DoAction(graphManager, current, previous);
            indexInSpideyActions = (indexInSpideyActions + 1) % actions.Count;
        }

        private Connection<NodeBehavior> PickNextConnection()
        {
            var currentNode = currentConnection.GetOtherVertex(lastNode);
            var connections = graph.AdjacentEdges(currentNode).ToList();
            if (connections.Count == 0)
            {
                return null;
            }
            var lastConnectionAngle = GetAngleOfConnection(currentNode, lastNode);

            var otherConnections = connections
                .Where(connection => connection != currentConnection) //!connection.IsAdjacent(lastNode))
                .Select(connection =>
                {
                    float angle = GetAngleOfConnection(currentNode, connection.GetOtherVertex(currentNode));
                    angle = angle - lastConnectionAngle;
                    if (whichSide == TraversalSide.INSIDE)
                    {
                        angle = -angle;
                    }
                    angle = (angle + (Mathf.PI * 2)) % (Mathf.PI * 2);
                    return new
                    {
                        angle,
                        connection
                    };
                })
                .OrderBy(data => data.angle)
                .Select(data => data.connection);

            return otherConnections.FirstOrDefault() ?? currentConnection;
            //lastNode = currentNode;
        }

        /// <summary>
        /// Gets the angle of a connection if it runs from origin to other
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="otherNode"></param>
        /// <returns></returns>
        private float GetAngleOfConnection(INode<NodeBehavior> originNode, INode<NodeBehavior> otherNode)
        {
            Vector2 delta = originNode.GetData().transform.position - otherNode.GetData().transform.position;

            return (Mathf.Atan2(delta.y, delta.x) + Mathf.PI * 2) % (Mathf.PI * 2);
        }


        private void TraverseNext()
        {
            var currentNode = currentConnection.GetOtherVertex(lastNode);
            AdvanceSpideyActions(currentNode, lastNode);

            var next = PickNextConnection();

            lastNode = currentNode;
            currentConnection = next;
        }

        public void Update()
        {
            distanceAlongConnection += Time.deltaTime * movementSpeed;

            Vector2 a = lastNode.GetData().transform.position;
            Vector2 b = currentConnection.GetOtherVertex(lastNode).GetData().transform.position;
            var diff = b - a;

            if (diff.magnitude <= distanceAlongConnection)
            {
                TraverseNext();
                distanceAlongConnection = 0;
                transform.position = b;
                return;
            }

            var scaledDiff = diff.normalized * distanceAlongConnection;
            transform.position = a + scaledDiff;
            switch (whichSide)
            {
                case TraversalSide.INSIDE:
                    transform.position = (Vector2)transform.position + (sideOffset * diff.normalized.Rotate(90));
                    break;
                case TraversalSide.OUTSIDE:
                    transform.position = (Vector2)transform.position + (-sideOffset * diff.normalized.Rotate(90));
                    break;
            }
        }

    }
}

using Assets.SpideyActions;
using QuikGraph;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public enum TraversalSide
    {
        LEFTHAND,
        RIGHTHAND
    }
    public class SpiderCrawly : MonoBehaviour
    {
        public GraphManager graphManager;

        public float movementSpeed = 1;
        public float sideOffset = 1;

        public UndirectedGraph<NodeBehavior, Connection<NodeBehavior>> graph => graphManager.Graph;

        private NodeBehavior lastNode;
        private Connection<NodeBehavior> currentConnection;
        private float distanceAlongConnection = 0f;
        public TraversalSide whichSide { get; private set; } = TraversalSide.LEFTHAND;

        private IList<ISpideyAction> actions;
        private int indexInSpideyActions;


        private void Start()
        {
            actions = new ISpideyAction[]
            {
                new CreateNodeSpideyAction(),
                new DoNothingSpideyAction(),
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

        private void AdvanceSpideyActions(NodeBehavior current, NodeBehavior previous, NodeBehavior tentativeNext)
        {
            actions[indexInSpideyActions].DoAction(this, current, previous, tentativeNext);
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
            var lastConnectionAngle = currentNode.GetRadianAngleOfConnectionTo(lastNode);

            var otherConnections = connections
                .Where(connection => connection != currentConnection) //!connection.IsAdjacent(lastNode))
                .Select(connection =>
                {
                    float angle = currentNode.GetRadianAngleOfConnectionTo(connection.GetOtherVertex(currentNode));
                    angle = angle - lastConnectionAngle;
                    if (whichSide == TraversalSide.LEFTHAND)
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
        }


        private void TraverseNext()
        {
            var tentativeNext = PickNextConnection();

            var currentNode = currentConnection.GetOtherVertex(lastNode);
            AdvanceSpideyActions(currentNode, lastNode, tentativeNext.GetOtherVertex(currentNode));

            tentativeNext = PickNextConnection();

            lastNode = currentNode;
            currentConnection = tentativeNext;
        }

        public void Update()
        {
            distanceAlongConnection += Time.deltaTime * movementSpeed;

            Vector2 a = lastNode.transform.position;
            Vector2 b = currentConnection.GetOtherVertex(lastNode).transform.position;
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
                case TraversalSide.LEFTHAND:
                    transform.position = (Vector2)transform.position + (sideOffset * diff.normalized.Rotate(90));
                    break;
                case TraversalSide.RIGHTHAND:
                    transform.position = (Vector2)transform.position + (-sideOffset * diff.normalized.Rotate(90));
                    break;
            }
        }

    }
}

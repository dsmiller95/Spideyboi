using Assets.SpideyActions;
using QuikGraph;
using System;
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
    [ExecuteInEditMode]
    public class SpiderCrawly : MonoBehaviour
    {
        public GraphManager graphManager => GetComponentInParent<GraphManager>();

        public float movementSpeed = 1;
        public float sideOffset = 1;

        public bool isMoving = false;
        public NodeBehavior lastNode;
        public Connection currentConnection;
        public TraversalSide whichSide = TraversalSide.LEFTHAND;
        public float distanceAlongConnection = 0f;


        public UndirectedGraph<NodeBehavior, Connection> graph => graphManager.Graph;


        private IList<ISpideyAction> actions;
        private int indexInSpideyActions;

        private IList<WinZone> winZones;


        private void Start()
        {
            winZones = FindObjectsOfType<WinZone>();
        }


        public void Update()
        {
            if (isMoving)
            {
                foreach(var winZone in winZones)
                {
                    if (winZone.TryTriggerwin(this))
                    {
                        this.StopMoving();
                        return;
                    }
                }

                distanceAlongConnection += Time.deltaTime * movementSpeed;
            }

            if(lastNode == null || currentConnection == null)
            {
                return;
            }
            Vector2 b = currentConnection.GetOtherVertex(lastNode).transform.position;

            if(distanceAlongConnection >= 1)
            {
                TraverseNext();
                distanceAlongConnection = 0;
                transform.position = b;
                return;
            }
            Vector2 a = lastNode.transform.position;
            var diff = b - a;

            var scaledDiff = diff * distanceAlongConnection;
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

        public void StartMoving(IEnumerable<ISpideyAction> actions)
        {
            this.actions = actions.ToList();
            indexInSpideyActions = 0;

            isMoving = true;
        }
        public void StopMoving()
        {
            isMoving = false;
        }

        public void ResetToOrigin()
        {
            throw new NotImplementedException();
        }

        private bool PickRandomConnection()
        {
            var connections = graph.AdjacentEdges(lastNode).ToList();
            if (connections.Count > 0)
            {
                currentConnection = connections[UnityEngine.Random.Range(0, connections.Count)];
                return true;
            }
            return false;
        }

        private void AdvanceSpideyActions(NodeBehavior current, NodeBehavior previous, NodeBehavior tentativeNext)
        {
            actions[indexInSpideyActions].DoAction(this, current, previous, tentativeNext);
            indexInSpideyActions = (indexInSpideyActions + 1) % actions.Count;
        }

        private Connection PickNextConnection()
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


    }
}

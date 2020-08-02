using Assets.SpideyActions;
using Assets.SpideyActions.SpideyStates;
using Assets.SpideyWeb;
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
        [Range(0, 1)]
        public float distanceAlongConnection = 0f;

        public NodeBehavior currentDraggingConnection;
        public ConnectionRenderer draggingLineRenderer;

        public UndirectedGraph<NodeBehavior, Connection> graph => graphManager.Graph;


        private IList<ISpideyAction> actions;
        private int indexInSpideyActions;
        /// <summary>
        /// A list of connections which should be ignored when choosing the next connection to travel on
        /// Used when attaching a web, it makes more intutive sense to keep moving after connecting
        /// </summary>
        public IList<Connection> extraIgnoreConnections;

        public IList<WinZone> winZones;

        private AsyncStateMachine<SpiderCrawly> stateMachine;


        public BasicConnection[] goalTopology;
        public int originGoalVertex;
        public NodeBehavior originActual;
        private GraphTopologyEquator topologyEquator;

        private void Awake()
        {
            extraIgnoreConnections = new List<Connection>();
            stateMachine = new AsyncStateMachine<SpiderCrawly>(new Moving());

            topologyEquator = new GraphTopologyEquator(goalTopology, originGoalVertex);
        }

        private void Start()
        {
            winZones = FindObjectsOfType<WinZone>();
        }

        public void CheckIfWin()
        {
            return;
            if (this.MatchesTopologyTarget(originActual))
            {
                Debug.Log("yes win");
                CustomEventSystem.instance.Dispatch(EVENT_TYPE.WIN, this);
            }
            else
            {
                Debug.Log("No win");
            }
        }

        private bool MatchesTopologyTarget(NodeBehavior originVertexInActual)
        {
            return this.topologyEquator.GraphMatches(graph, originVertexInActual);
        }

        private bool defaultPositioning = true;
        public void Update()
        {
            myUpdate();

            if (defaultPositioning)
            {
                var myDistance = Math.Min(distanceAlongConnection, 1);

                Vector2 b = currentConnection.GetOtherVertex(lastNode).transform.position;
                Vector2 a = lastNode.transform.position;
                var diff = b - a;

                var scaledDiff = diff * myDistance;
                transform.position = a + scaledDiff;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                switch (whichSide)
                {
                    case TraversalSide.LEFTHAND:
                        transform.localScale = new Vector3(1, 1, 1);
                        //transform.position = (Vector2)transform.position + (sideOffset * diff.normalized.Rotate(90));
                        break;
                    case TraversalSide.RIGHTHAND:
                        transform.localScale = new Vector3(1, -1, 1);
                        //transform.position = (Vector2)transform.position + (-sideOffset * diff.normalized.Rotate(90));
                        break;
                }
            }
        }
        async void myUpdate()
        {
            try
            {
                await stateMachine.update(this);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                throw;
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

        public ISpideyAction GetNextAction()
        {
            var nextAction = actions[indexInSpideyActions];
            indexInSpideyActions = (indexInSpideyActions + 1) % actions.Count;
            return nextAction;
        }

        public void SwitchSide()
        {
            switch (whichSide)
            {
                case TraversalSide.LEFTHAND:
                    whichSide = TraversalSide.RIGHTHAND;
                    break;
                case TraversalSide.RIGHTHAND:
                    whichSide = TraversalSide.LEFTHAND;
                    break;
            }
        }

        public Connection PickNextConnection(IList<Connection> extraExcludedConnections = null)
        {
            var currentNode = currentConnection.GetOtherVertex(lastNode);
            var connections = graph.AdjacentEdges(currentNode).ToList();
            if (connections.Count == 0)
            {
                return null;
            }
            var lastConnectionAngle = currentNode.GetRadianAngleOfConnectionTo(lastNode);

            var otherConnections = connections
                .Where(connection => connection != currentConnection);
            if (extraExcludedConnections != null)
            {
                otherConnections = otherConnections
                    .Where(connecton => !extraExcludedConnections.Contains(connecton));
            }
            otherConnections = otherConnections
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
    }
}

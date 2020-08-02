using Assets.SpideyActions;
using Assets.SpideyActions.SpideyStates;
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
        public LineRenderer draggingLineRenderer;

        public UndirectedGraph<NodeBehavior, Connection> graph => graphManager.Graph;


        private IList<ISpideyAction> actions;
        private int indexInSpideyActions;

        public IList<WinZone> winZones;

        private AsyncStateMachine<SpiderCrawly> stateMachine;

        private void Awake()
        {
            stateMachine = new AsyncStateMachine<SpiderCrawly>(new Moving());
        }

        private void Start()
        {
            winZones = FindObjectsOfType<WinZone>();
        }

        public void Update()
        {
            myUpdate();
        }
        async void myUpdate()
        {
            try
            {
                await stateMachine.update(this);
            }
            catch(System.Exception e)
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

        public Connection PickNextConnection()
        {
            var currentNode = currentConnection.GetOtherVertex(lastNode);
            var connections = graph.AdjacentEdges(currentNode).ToList();
            if (connections.Count == 0)
            {
                return null;
            }
            var lastConnectionAngle = currentNode.GetRadianAngleOfConnectionTo(lastNode);

            var otherConnections = connections
                .Where(connection => connection != currentConnection)
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

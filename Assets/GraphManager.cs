using Assets;
using QuikGraph;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public UndirectedGraph<NodeBehavior, Connection<NodeBehavior>> Graph {
        get; private set;
    }

    public NodeBehavior defaultNodePrefab;
    public LineRenderer edgeRendererPrefab;

    public float edgeColliderOffsetInEdges = 0.7f;
    public float defaultConnectionLength = 10f;

    private IList<LineRenderer> connectionRenderers;

    private void Awake()
    {
        connectionRenderers = new List<LineRenderer>();
        Graph = new UndirectedGraph<NodeBehavior, Connection<NodeBehavior>>();
        var v1 = CreateNewNodeWithWobble("one");
        var v2 = CreateNewNodeWithWobble("two");
        //var v3 = CreateNewNode("three");
        var v4 = CreateNewNodeWithWobble("four");

        var e1 = new Connection<NodeBehavior>(v1, v2);
        var e2 = new Connection<NodeBehavior>(v1, v4);
        var e3 = new Connection<NodeBehavior>(v2, v4);
        //var e4 = new Connection<NodeBehavior>(v2, v3);

        Graph.AddVertexRange(new[] { v1, v2, v4 });
        Graph.AddEdgeRange(new[] { e1, e2, e3, });
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public NodeBehavior CreateNewNode(string extraName = "")
    {
        var behavior = Instantiate(defaultNodePrefab, transform).GetComponent<NodeBehavior>();
        behavior.name += extraName;
        return behavior;
    }
    private NodeBehavior CreateNewNodeWithWobble(string extraName = "", Vector3 averageCenter = default, float variance = 3f)
    {
        var newNode = CreateNewNode(extraName);
        newNode.transform.position = averageCenter + new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance));
        return newNode;
    }
    public NodeBehavior CreateNewNodeBetweenTwoConnectionsAtBase(NodeBehavior origin, NodeBehavior a, NodeBehavior b, float distanceFromOrigin, bool isLeftHandSide)
    {
        Vector2 originVector = origin.transform.position;
        Vector2 aVect = a.transform.position;
        Vector2 trueDirection;
        if(a == b)
        {
            trueDirection = (originVector - aVect).normalized * distanceFromOrigin;
        }else
        {
            Vector2 bVect = b.transform.position;
            aVect -= originVector;
            bVect -= originVector;

            var diff = Vector2.SignedAngle(aVect, bVect);
            if (isLeftHandSide && diff > 0)
            {
                diff = diff - 360;
            }
            else if (!isLeftHandSide && diff < 0)
            {
                diff = diff + 360;
            }
            var halfway = diff / 2;

            trueDirection = aVect.normalized.Rotate(halfway) * distanceFromOrigin;
        }
        var newLocation = originVector + trueDirection;

        var behavior = CreateNewNode();
        behavior.transform.position = newLocation;
        return behavior;
    }

    private LineRenderer CreateConnection()
    {
        return Instantiate(edgeRendererPrefab, transform).GetComponent<LineRenderer>();
    }

    private void AlignLineToConnection(Connection<NodeBehavior> connection, LineRenderer lineRenderer)
    {
        var source = connection.Source;
        var target = connection.Target;

        Vector2 sourceVect = source.transform.position;
        Vector2 targetVect = target.transform.position;

        lineRenderer.SetPosition(0, sourceVect);
        lineRenderer.SetPosition(1, targetVect);

        var edge = lineRenderer.GetComponent<EdgeCollider2D>();
        if(edge != null)
        {
            var offset = edgeColliderOffsetInEdges * (sourceVect - targetVect).normalized;

            var targetWithOffset = targetVect + offset;
            var sourceWithOffset = sourceVect - offset;

            edge.points = new[] { sourceWithOffset, targetWithOffset };
        }
    }

    private void UpdateConnections()
    {
        var connections = Graph.Edges.ToList();
        while(connectionRenderers.Count < connections.Count)
        {
            connectionRenderers.Add(CreateConnection());
        }
        while(connectionRenderers.Count > connections.Count)
        {
            Destroy(connectionRenderers[0]);
            connectionRenderers.RemoveAt(0);
        }

        for(var i = 0; i < connections.Count; i++)
        {
            AlignLineToConnection(connections[i], connectionRenderers[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateConnections();
    }
}

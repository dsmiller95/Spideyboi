using Assets;
using QuikGraph;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public UndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>> Graph {
        get; private set;
    }

    public NodeBehavior defaultNodePrefab;
    public LineRenderer edgeRendererPrefab;

    public float edgeColliderOffsetInEdges = 0.7f;

    private IList<LineRenderer> connectionRenderers;

    private void Awake()
    {
        connectionRenderers = new List<LineRenderer>();
        Graph = new UndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>>();
        var v1 = CreateNewNode("one");
        var v2 = CreateNewNode("two");
        //var v3 = CreateNewNode("three");
        var v4 = CreateNewNode("four");

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

    public INode<NodeBehavior> CreateNewNode(string extraName = "", Vector3 averageCenter = default, float variance = 3f)
    {
        var behavior = Instantiate(defaultNodePrefab, transform).GetComponent<NodeBehavior>();
        behavior.name += extraName;
        behavior.transform.position = averageCenter + new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance));
        return behavior;
    }
    private LineRenderer CreateConnection()
    {
        return Instantiate(edgeRendererPrefab, transform).GetComponent<LineRenderer>();
    }

    private void AlignLineToConnection(Connection<NodeBehavior> connection, LineRenderer lineRenderer)
    {
        var source = connection.Source.GetData();
        var target = connection.Target.GetData();

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

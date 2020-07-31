using Assets;
using QuikGraph;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public IMutableUndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>> Graph {
        get; private set;
    }

    public NodeBehavior defaultNodePrefab;
    public LineRenderer edgeRendererPrefab;
    
    private IList<LineRenderer> connectionRenderers;

    private void Awake()
    {
        connectionRenderers = new List<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Graph = new UndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>>();
        var v1 = CreateNewNode();
        var v2 = CreateNewNode();
        var v3 = CreateNewNode();
        var v4 = CreateNewNode();

        var e1 = new Connection<NodeBehavior>(v1, v2);
        var e2 = new Connection<NodeBehavior>(v1, v4);
        var e3 = new Connection<NodeBehavior>(v2, v4);
        var e4 = new Connection<NodeBehavior>(v2, v3);

        Graph.AddVertexRange(new[] { v1, v2, v3, v4 });
        Graph.AddEdgeRange(new[] { e1, e2, e3, e4 });
    }

    private INode<NodeBehavior> CreateNewNode()
    {
        var behavior = Instantiate(defaultNodePrefab, transform).GetComponent<NodeBehavior>();
        behavior.transform.position += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
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

        lineRenderer.SetPosition(0, source.transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
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

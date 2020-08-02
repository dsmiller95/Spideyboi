using Assets;
using QuikGraph;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class GraphManager : MonoBehaviour
{
    public UndirectedGraph<NodeBehavior, Connection> Graph
    {
        get; private set;
    }

    public NodeBehavior defaultNodePrefab;
    public Connection edgePrefab;

    public float defaultConnectionLength = 10f;

    private void Awake()
    {
        Debug.Log("awakening");
        Graph = new UndirectedGraph<NodeBehavior, Connection>();

        Graph.AddVertexRange(GetComponentsInChildren<NodeBehavior>());
        Graph.AddEdgeRange(GetComponentsInChildren<Connection>());

        //var v1 = CreateNewNodeWithWobble("one");
        //var v2 = CreateNewNodeWithWobble("two");
        ////var v3 = CreateNewNode("three");
        //var v4 = CreateNewNodeWithWobble("four");

        //var e1 = new Connection<NodeBehavior>(v1, v2);
        //var e2 = new Connection<NodeBehavior>(v1, v4);
        //var e3 = new Connection<NodeBehavior>(v2, v4);
        ////var e4 = new Connection<NodeBehavior>(v2, v3);

        //Graph.AddVertexRange(new[] { v1, v2, v4 });
        //Graph.AddEdgeRange(new[] { e1, e2, e3, });
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public NodeBehavior CreateNewNode(string extraName = "")
    {
        var behavior = Instantiate(defaultNodePrefab, transform).GetComponent<NodeBehavior>();
        behavior.name += extraName;
        Graph.AddVertex(behavior);
        return behavior;
    }
    public NodeBehavior CreateNewNodeWithWobble(string extraName = "", Vector3 averageCenter = default, float variance = 3f)
    {
        var newNode = CreateNewNode(extraName);
        newNode.transform.position = averageCenter + new Vector3(Random.Range(-variance, variance), Random.Range(-variance, variance));
        return newNode;
    }
    public NodeBehavior CreateNewNodeWithConnectionBetweenTwoConnectionsAtBase(NodeBehavior origin, NodeBehavior a, NodeBehavior b, float distanceFromOrigin, bool isLeftHandSide)
    {
        Vector2 originVector = origin.transform.position;
        Vector2 aVect = a.transform.position;
        Vector2 trueDirection;
        if (a == b)
        {
            trueDirection = (originVector - aVect).normalized * distanceFromOrigin;
        }
        else
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

    public Connection CreatRandomConnection()
    {
        var verteces = Graph.Vertices.ToList();
        var a = verteces[Random.Range(0, verteces.Count)];
        var otherVerts = verteces.Where(v => v != a);
        var b = otherVerts.Skip(Random.Range(0, verteces.Count - 1)).First();
        return CreateConnection(a, b);
    }

    public Connection CreateConnection(NodeBehavior a, NodeBehavior b)
    {
        var connection = Instantiate(edgePrefab, transform).GetComponent<Connection>();
        connection.Source = a;
        connection.Target = b;
        Graph.AddEdge(connection);
        return connection;
    }


    // Update is called once per frame
    void Update()
    {
        //UpdateConnections();
    }
}

using QuikGraph;
using QuikGraph.Algorithms.VertexColoring;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NodeBehavior: MonoBehaviour, INode<NodeBehavior>
    {
        public float targetConnectionLength = 5f;
        public float connectionSpringConstant = 1f;
        public float repulsionConstant = -1f;

        private IMutableUndirectedGraph<INode<NodeBehavior>, Connection<NodeBehavior>> graph { get => GetComponentInParent<GraphManager>().Graph; }


        private void Update()
        {
            var myPos = transform.position;
            var allOtherNodePositionsDiffs = transform.parent.gameObject
                .GetComponentsInChildren<NodeBehavior>()
                .Where(x => x != this)
                .Select(x => x.transform.position - myPos);
            var allConnectedNodes = graph.AdjacentEdges(this)
                .Select(connection => connection.GetOtherVertex(this).GetData())
                .Select(x => x.transform.position - myPos);

            var repulsionForce = GetInverseSquaredForce(allOtherNodePositionsDiffs, repulsionConstant);
            var attractionForce = GetSpringForce(allConnectedNodes, connectionSpringConstant, targetConnectionLength);

            var body = GetComponent<Rigidbody2D>();
            Vector2 force = repulsionForce + attractionForce;
            body.AddForce(force);
        }

        private Vector3 GetInverseSquaredForce(IEnumerable<Vector3> positionDelta, float attractionConstant)
        {
            return GetForce(positionDelta, diff => attractionConstant / diff.sqrMagnitude);
        }

        private Vector3 GetSpringForce(IEnumerable<Vector3> positionDelta, float springConstant, float defaultSpringLength)
        {
            return GetForce(positionDelta, diff => springConstant * (diff.magnitude - defaultSpringLength));
        }

        private Vector3 GetForce(IEnumerable<Vector3> positionDeltas, Func<Vector3, float> forceCalc)
        {
            return positionDeltas
                .Select(vectorDiff => vectorDiff.normalized * forceCalc(vectorDiff))
                .Aggregate((force1, force2) => force1 + force2);
        }

        public NodeBehavior GetData()
        {
            return this;
        }
    }
}

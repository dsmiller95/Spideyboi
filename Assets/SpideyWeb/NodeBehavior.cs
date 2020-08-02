using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NodeBehavior : MonoBehaviour
    {
        public float targetConnectionLength = 5f;
        public float connectionSpringConstant = 1f;
        public float repulsionConstant = -1f;

        private IMutableUndirectedGraph<NodeBehavior, Connection> graph => GetComponentInParent<GraphManager>().Graph;


        /// <summary>
        /// Gets the angle of a connection if it runs from origin to other
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="otherNode"></param>
        /// <returns></returns>
        public float GetRadianAngleOfConnectionTo(NodeBehavior otherNode)
        {
            Vector2 delta = transform.position - otherNode.transform.position;

            return (Mathf.Atan2(delta.y, delta.x) + Mathf.PI * 2) % (Mathf.PI * 2);
        }

        private void Update()
        {
            var myPos = transform.position;
            var allOtherNodePositionsDiffs = transform.parent.gameObject
                .GetComponentsInChildren<NodeBehavior>()
                .Where(x => x != this)
                .Select(x => x.transform.position - myPos);
            var allConnectedNodes = graph.AdjacentEdges(this)
                .Select(connection => connection.GetOtherVertex(this))
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
    }
}

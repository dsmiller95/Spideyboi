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
        public float repulsionConstant = -1f;

        private GraphManager GraphManager => GetComponentInParent<GraphManager>();
        private IMutableUndirectedGraph<NodeBehavior, Connection> graph => GraphManager.Graph;


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

        public SpringJoint2D AddSpringJoint(NodeBehavior other)
        {
            var joint = gameObject.AddComponent<SpringJoint2D>();
            joint.connectedBody = other.GetComponent<Rigidbody2D>();
            joint.autoConfigureDistance = false;

            return joint;
        }

        private void FixedUpdate()
        {
            var myPos = transform.position;
            var allOtherNodes = transform.parent.gameObject
                .GetComponentsInChildren<NodeBehavior>()
                .Where(x => x != this)
                .Select(x => x.GetComponent<Rigidbody2D>());

            var repulsionForce = GetInverseSquaredForce(allOtherNodes, repulsionConstant, myPos);

            var body = GetComponent<Rigidbody2D>();
            Vector2 force = repulsionForce;
            body.AddForce(force, ForceMode2D.Force);
        }

        private Vector3 GetInverseSquaredForce(IEnumerable<Rigidbody2D> bodies, float attractionConstant, Vector3 originPoint)
        {
            return GetForce(bodies, (diff, body) => attractionConstant * body.mass / diff.sqrMagnitude, originPoint);
        }

        private Vector3 GetForce(IEnumerable<Rigidbody2D> bodies, Func<Vector3, Rigidbody2D, float> forceCalc, Vector3 originPoint)
        {
            return bodies
                .Select(x =>
                    new
                    {
                        posDiff = (Vector3)x.position - originPoint,
                        body = x
                    })
                .Select(data => data.posDiff.normalized * forceCalc(data.posDiff, data.body))
                .Aggregate((force1, force2) => force1 + force2);
        }
    }
}

﻿using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
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
            var allOtherNodePositionsDiffs = transform.parent.gameObject
                .GetComponentsInChildren<NodeBehavior>()
                .Where(x => x != this)
                .Select(x => x.transform.position - myPos);

            var repulsionForce = GetInverseSquaredForce(allOtherNodePositionsDiffs, repulsionConstant);

            var body = GetComponent<Rigidbody2D>();
            Vector2 force = repulsionForce;
            body.AddForce(force, ForceMode2D.Force);
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

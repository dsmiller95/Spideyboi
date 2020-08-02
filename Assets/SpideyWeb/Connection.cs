using QuikGraph;
using System;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class Connection : MonoBehaviour, IEdge<NodeBehavior>
    {
        public NodeBehavior sourceForInspector;
        public NodeBehavior targetForInspector;
        public float edgeColliderOffsetInEdges = 0.7f;

        public NodeBehavior Source { get => sourceForInspector; set => sourceForInspector = value; }
        public NodeBehavior Target { get => targetForInspector; set => targetForInspector = value; }

        private void Start()
        {
            AlignLineToConnection();
        }


        private void Update()
        {
            this.AlignLineToConnection();
        }

        private void AlignLineToConnection()
        {
            Vector2 sourceVect;
            Vector2 targetVect;
            var lineRenderer = GetComponent<LineRenderer>();
            if (Source != null && Target != null)
            {
                sourceVect = Source.transform.position;
                targetVect = Target.transform.position;

                lineRenderer.SetPosition(0, sourceVect);
                lineRenderer.SetPosition(1, targetVect);
            }
            else
            {
                sourceVect = lineRenderer.GetPosition(0);
                targetVect = lineRenderer.GetPosition(1);
            }

            var edge = GetComponent<EdgeCollider2D>();
            if (edge != null)
            {
                var offset = edgeColliderOffsetInEdges * (sourceVect - targetVect).normalized;

                var targetWithOffset = targetVect + offset;
                var sourceWithOffset = sourceVect - offset;

                edge.points = new[] { sourceWithOffset, targetWithOffset };
            }
        }
    }
}

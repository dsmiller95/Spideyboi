using QuikGraph;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class ConnectionRenderer : MonoBehaviour
    {
        public GameObject Source;
        public GameObject Target;
        public float edgeColliderOffsetInEdges = 0.7f;

        private void Start()
        {
            AlignLineToConnection();
        }

        private void Update()
        {
            AlignLineToConnection();
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

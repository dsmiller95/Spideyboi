using QuikGraph;
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


        public float dampingRatio = 0.1f;
        public float frequency = 3f;

        private float _targetDistance = -1;
        public float targetDistance
        {
            get => _targetDistance; set
            {
                _targetDistance = value;
                if (managedSpringJoint)
                {
                    managedSpringJoint.distance = _targetDistance;
                }
            }
        }

        private GraphManager GraphManager => GetComponentInParent<GraphManager>();
        public NodeBehavior Source
        {
            get => sourceForInspector; set
            {
                sourceForInspector = value;
                ConfigureSpringJoint();
            }
        }
        public NodeBehavior Target
        {
            get => targetForInspector; set
            {
                targetForInspector = value;
                ConfigureSpringJoint();
            }
        }

        private void Awake()
        {
            ConfigureSpringJoint();
        }

        private void Start()
        {
            AlignLineToConnection();
        }

        public SpringJoint2D managedSpringJoint;

        private void ConfigureSpringJoint()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            DestroyImmediate(managedSpringJoint);
            if (Source != null && Target != null)
            {
                managedSpringJoint = Source.AddSpringJoint(Target);
                managedSpringJoint.dampingRatio = dampingRatio;
                managedSpringJoint.frequency = frequency;
                managedSpringJoint.distance = (targetDistance < 0) ? GraphManager.defaultConnectionLength : targetDistance;
            }
        }

        private void OnDestroy()
        {
            if(managedSpringJoint != null)
            {
                DestroyImmediate(managedSpringJoint);
            }
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

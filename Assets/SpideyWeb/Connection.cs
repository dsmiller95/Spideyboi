using QuikGraph;
using UnityEngine;

namespace Assets
{
    [ExecuteInEditMode]
    public class Connection : MonoBehaviour, IEdge<NodeBehavior>
    {
        public NodeBehavior sourceForInspector;
        public NodeBehavior targetForInspector;

        public ConnectionRenderer connectionRenderer;

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
                UpdateObjectConnections();
            }
        }
        public NodeBehavior Target
        {
            get => targetForInspector; set
            {
                targetForInspector = value;
                UpdateObjectConnections();
            }
        }

        private void Awake()
        {
            UpdateObjectConnections();
        }

        private void Start()
        {
        }


        private void UpdateObjectConnections()
        {
            if (Source != null && Target != null)
            {
                var renderer = connectionRenderer;
                renderer.Source = Source.gameObject;
                renderer.Target = Target.gameObject;
            }
            UpdateSpring();
        }

        public SpringJoint2D managedSpringJoint;

        private void UpdateSpring()
        {
            if (managedSpringJoint != null)
            {
                DestroyImmediate(managedSpringJoint);
            }
            if (Source != null && Target != null)
            {
                if (!Application.isPlaying)
                {
                    return;
                }
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
        }
    }
}

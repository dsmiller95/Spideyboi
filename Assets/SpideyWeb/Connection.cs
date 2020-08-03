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

        private float _targetDistance = -1;
        public float targetDistance
        {
            get => _targetDistance; set
            {
                _targetDistance = value;
                if (connectionRenderer)
                {
                    connectionRenderer.SetSpringDistance(_targetDistance);
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
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
        }
    }
}

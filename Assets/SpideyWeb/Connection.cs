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
        public ConnectionRenderer connectionRendererWhenSplit;

        public float _targetDistanceForInspector = 1;
        public float targetDistance
        {
            get => _targetDistanceForInspector; set
            {
                _targetDistanceForInspector = value;
                UpdateTargetDistances();
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

        public Rigidbody2D _connectionSplitBodyForInspector = null;
        public Rigidbody2D SplitBody
        {
            get => _connectionSplitBodyForInspector;
            set
            {
                _connectionSplitBodyForInspector = value;
                UpdateObjectConnections();
            }
        }

        private float _splitRatio = 0;
        public float SplitRatio
        {
            get => _splitRatio;
            set
            {
                _splitRatio = value;
                UpdateTargetDistances();
            }
        }


        private void Awake()
        {
            UpdateObjectConnections();
            UpdateTargetDistances();
        }

        private void Start()
        {
        }


        private void UpdateObjectConnections()
        {
            if (Source != null && Target != null)
            {
                if(SplitBody == null)
                {
                    connectionRenderer.Source = Source.gameObject;
                    connectionRenderer.Target = Target.gameObject;
                    connectionRendererWhenSplit.gameObject.SetActive(false);
                }
                else
                {
                    connectionRendererWhenSplit.gameObject.SetActive(true);

                    connectionRenderer.Source = Source.gameObject;
                    connectionRenderer.Target = SplitBody.gameObject;

                    connectionRendererWhenSplit.Source = SplitBody.gameObject;
                    connectionRendererWhenSplit.Target = Target.gameObject;
                }
            }
        }

        private void UpdateTargetDistances()
        {
            if (connectionRenderer)
            {
                if (SplitBody == null)
                {
                    connectionRenderer.SetSpringDistance(_targetDistanceForInspector);
                }
                else
                {
                    connectionRenderer.SetSpringDistance(_targetDistanceForInspector * SplitRatio);
                    connectionRendererWhenSplit.SetSpringDistance(_targetDistanceForInspector * (1 - SplitRatio));
                }
            }
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateObjectConnections();
                UpdateTargetDistances();
            }
        }
    }
}

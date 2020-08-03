using System;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(LineRenderer))]
    [ExecuteInEditMode]
    public class ConnectionRenderer : MonoBehaviour
    {
        private GameObject _source;
        private GameObject _target;
        public GameObject Source
        {
            get => _source; set
            {
                Collider2D sourceCollider;
                if (_source != null && (sourceCollider = _source.GetComponent<Collider2D>()) != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), sourceCollider, false);
                }
                _source = value;
                if (_source != null && (sourceCollider = _source.GetComponent<Collider2D>()) != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), sourceCollider, true);
                }
                UpdateSpringConnections();
            }
        }
        public GameObject Target
        {
            get => _target; set
            {
                Collider2D targetCollider;
                if (_target != null && (targetCollider = _target.GetComponent<Collider2D>()) != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), targetCollider, false);
                }
                _target = value;
                if (_target != null && (targetCollider = _target.GetComponent<Collider2D>()) != null)
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), targetCollider, true);
                }
                UpdateSpringConnections();
            }
        }

        public Action<Collider2D> OnCollided;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnCollided?.Invoke(collision);
        }


        public float edgeColliderOffsetInEdges = 0.7f;

        public RealSpring managedSpringJoint;

        private void UpdateSpringConnections()
        {
            if (managedSpringJoint)
            {
                managedSpringJoint.a = Source;//?.GetComponent<Rigidbody2D>();
                managedSpringJoint.b = Target;//?.GetComponent<Rigidbody2D>();
            }
        }

        public void SetSpringDistance(float newDistance)
        {
            if (managedSpringJoint)
            {
                managedSpringJoint.targetDistance = newDistance;
            }
        }

        private void Awake()
        {
            UpdateSpringConnections();
        }

        private void Start()
        {
            UpdateSpringConnections();
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

            var edge = GetComponent<CapsuleCollider2D>();
            if (edge != null)
            {
                var midpoint = (sourceVect + targetVect) / 2;
                this.transform.position = midpoint;
                var diff = sourceVect - targetVect;

                this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

                var size = diff.magnitude - edgeColliderOffsetInEdges * 2;
                edge.size = new Vector2(size, edge.size.y);
            }
        }
    }
}

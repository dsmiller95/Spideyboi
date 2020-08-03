using QuikGraph;
using UnityEngine;

namespace Assets
{
    public class RealSpring : MonoBehaviour
    {
        public Rigidbody2D a;
        public Rigidbody2D b;

        public float springConstant = 1f;
        public float targetDistance = 10f;


        private void Awake()
        {
        }

        private void Start()
        {
        }

        private void Update()
        {
            if(a == null || b == null)
            {
                return;
            }
            var diff = a.position - b.position;
            var distDiff = diff.magnitude - targetDistance;
            var force = springConstant * distDiff;
            var normal = diff.normalized;

            a.AddForce(normal * -force);
            b.AddForce(normal * force);
        }
    }
}

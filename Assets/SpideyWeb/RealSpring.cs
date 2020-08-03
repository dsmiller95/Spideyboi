using QuikGraph;
using System;
using UnityEngine;

namespace Assets
{
    public class RealSpring : MonoBehaviour
    {
        public GameObject a;
        public Action<Vector2> onAForceAdded;
        public GameObject b;
        public Action<Vector2> onBForceAdded;

        public float springConstant = 1f;
        public float targetDistance = 10f;

        public bool waitTillNextPull = false;

        private void Awake()
        {
        }

        private void Start()
        {
        }

        private float impulseStrength = 1;
        public void ImpulseOnNext(float impulseStrength)
        {
            this.impulseStrength = impulseStrength;
        }

        private void FixedUpdate()
        {
            try
            {
                if(a == null || b == null || !a || !b || !a.activeInHierarchy || !b.activeInHierarchy)
                {
                    return;
                }
            }catch(Exception e)
            {
                throw;
            }
            var rigidA = a.GetComponent<Rigidbody2D>() ?? null;
            var rigidB = b.GetComponent<Rigidbody2D>() ?? null;
            if(!(rigidA || rigidB))
            {
                return;
            }

            var posA = (rigidA == null) ? (Vector2)a.transform.position : rigidA.position;
            var posB = (rigidB == null) ? (Vector2)b.transform.position : rigidB.position;

            var diff = posA - posB;
            var distDiff = diff.magnitude - targetDistance;
            var force = springConstant * distDiff * impulseStrength;
            if (waitTillNextPull && force < 0)
            {
                return;
            }
            waitTillNextPull = false;

            if(force < 0)
            {
                Debug.Log($"Force: {(int)force}");
            }

            var normal = diff.normalized;
            var forceMode = impulseStrength == 1 ? ForceMode2D.Force : ForceMode2D.Impulse;
            if(forceMode == ForceMode2D.Impulse)
            {
                Debug.Log("Applying impulse");
            }
            if (rigidA != null)
            {
                rigidA.AddForce(normal * -force, forceMode);
            }
            if (rigidB != null)
            {
                rigidB.AddForce(normal * force, forceMode);
            }
            onAForceAdded?.Invoke(normal * -force);
            onAForceAdded?.Invoke(normal * force);

            impulseStrength = 1;
        }
    }
}

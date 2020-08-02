using UnityEngine;

namespace Assets.SpideyWeb
{
    public class Resetable : MonoBehaviour
    {
        public GameObject childPrefab;

        public GameObject currentChild;

        public void Reset()
        {
            Destroy(currentChild);
            currentChild = Instantiate(childPrefab, transform);
        }
    }
}

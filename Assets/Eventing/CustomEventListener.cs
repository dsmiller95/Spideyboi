using UnityEngine;

namespace Assets
{

    public class CustomEventListener : MonoBehaviour
    {
        public EVENT_TYPE type;
        public ObjectEvent onEvent;

        private Listener myListener;
        private void Awake()
        {
            myListener = new Listener(type, (data) =>
            {
                onEvent.Invoke(data);
            });
        }

        private void OnDestroy()
        {
            CustomEventSystem.instance?.RemoveListener(myListener);
        }

        // Start is called before the first frame update
        void Start()
        {
            CustomEventSystem.instance?.RegisterListener(myListener);
        }
    }
}

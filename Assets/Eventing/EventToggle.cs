using Assets;
using UnityEngine;

public class EventToggle: MonoBehaviour
{
    public EVENT_TYPE EventName = EVENT_TYPE.WIN;
    public GameObject toggleObject;

    private Listener myListener;
    private void Awake()
    {
        myListener = new Listener(EventName, (_) =>
        {
            toggleObject.SetActive(!toggleObject.activeSelf);
        });
    }

    private void OnDestroy()
    {
        CustomEventSystem.instance.RemoveListener(myListener);
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomEventSystem.instance.RegisterListener(myListener);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

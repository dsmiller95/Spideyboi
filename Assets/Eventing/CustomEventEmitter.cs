using Assets;
using UnityEngine;

public class CustomEventEmitter : MonoBehaviour
{
    public EVENT_TYPE eventType;
    public void EmitEvent()
    {
        CustomEventSystem.instance.Dispatch(eventType, this);
    }
}

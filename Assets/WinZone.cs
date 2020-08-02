using Assets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WinZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void TriggerWin()
    {
        CustomEventSystem.instance.Dispatch(EVENT_TYPE.WIN, null);
    }

    public bool TryTriggerwin(SpiderCrawly crawly)
    {
        var position = crawly.transform.position;

        var collider = GetComponent<BoxCollider2D>();
        var isWin = collider.OverlapPoint(position);
        if (isWin)
        {
            this.TriggerWin();
        }
        return isWin;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    public TutorialBox nextBox;

    public void AdvanceToNextBox()
    {
        gameObject.SetActive(false);
        nextBox?.gameObject.SetActive(true);
    }

    public void SkipTutorial()
    {
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

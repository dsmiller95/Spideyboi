using Assets;
using UnityEngine;

public class LevelCompletedTrigger: MonoBehaviour
{
    public void TriggerLevelCompleted()
    {
        LevelSelections.CompletedLevel();
    }
}

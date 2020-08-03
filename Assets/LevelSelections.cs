using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Level
{
    public string buttonNumber;
    public string sceneName;
    public bool isUnlocked;
    public bool isCompleted;
    public GameObject instance;
    public int bestScore = -1;
}

public class LevelSelections : MonoBehaviour
{
    public static Level[] globalLevels;

    public Level[] levels;
    public GameObject levelPrefab;

    private void Awake()
    {
        if(globalLevels == null)
        {
            globalLevels = levels;
        }else
        {
            levels = globalLevels;
        }
    }

    public static void CompletedLevel()
    {
        var level = CurrentLevel();
        if (level != null)
        {
            level.isCompleted = true;
        }
        var completedTotal = globalLevels.Where(x => x.isCompleted).Count();
        foreach (var levelToEnable in globalLevels.Take(completedTotal + 1))
        {
            levelToEnable.isUnlocked = true;
        }
    }

    public static Level CurrentLevel()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        return globalLevels.Where(x => x.sceneName == sceneName).FirstOrDefault();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var level in levels){
            SetupLevelButton(level);
        }
    }

    private void SetupLevelButton(Level level)
    {
        level.instance = Instantiate(levelPrefab, transform);
        level.instance.GetComponentInChildren<TextMeshProUGUI>().text = level.buttonNumber;
        level.instance.GetComponentInChildren<Button>().interactable = level.isUnlocked;
        level.instance.GetComponentInChildren<SceneRestore>().SceneToRestore = level.sceneName;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}

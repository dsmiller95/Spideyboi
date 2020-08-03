using Assets.SpideyActions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public GameObject spideyActionsParent;

    // Start is called before the first frame update
    void Start()
    {
    }

    public float currentSimStartTime;
    public float currentSimEndTime;

    public void BeginTiming()
    {
        currentSimStartTime = Time.time;
    }
    public void StopTiming()
    {
        currentSimEndTime = Time.time;
    }

    public void UpdateScoreText()
    {
        var graph = FindObjectOfType<GraphManager>();
        var actions = spideyActionsParent.GetComponentsInChildren<ISpideyAction>();

        var graphConnections = graph.Graph.EdgeCount;
        var actionCount = actions.Length;
        var score = graphConnections + actionCount * 10;
        var time = this.currentSimEndTime - this.currentSimStartTime;
        
        var bestScore = LevelSelections.CurrentLevel().bestScore;
        var bestTime = LevelSelections.CurrentLevel().bestTime;

        var text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = GetScoreSummary(graphConnections, actionCount, score, time, bestScore, bestTime);


        var currentLevel = LevelSelections.CurrentLevel();
        if (bestScore >= 0)
        {
            currentLevel.bestScore = Math.Min(score, bestScore);    
        }
        else
        {
            currentLevel.bestScore = score;
        }
        if (bestTime >= 0)
        {
            currentLevel.bestTime = Math.Min(time, bestTime);
        }
        else
        {
            currentLevel.bestTime = time;
        }
    }

    public string GetScoreSummary(int connections, int actions, int score, float time, int bestScore, float bestTime)
    {
        var bestScoreText = (bestScore == -1 ? "" : bestScore.ToString());
        var bestTimeText = (bestTime < 0 ? "" : bestTime.ToString("F1"));
        var primary = $"\t\t\tCurrent\tBest\nWebs:\t\t{connections}\nCommands:\t\t{actions}\nScore:\t\t{score}\t\t{bestScoreText}\nTime:\t\t\t{time:F1}\t\t{bestTimeText}";
        return primary;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

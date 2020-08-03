using Assets.SpideyActions;
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

    public void UpdateScoreText()
    {
        var graph = FindObjectOfType<GraphManager>();
        var actions = spideyActionsParent.GetComponentsInChildren<ISpideyAction>();

        var graphConnections = graph.Graph.EdgeCount;
        var actionCount = actions.Length;
        var score = graphConnections + actionCount * 10;
        var bestScore = LevelSelections.CurrentLevel().bestScore;

        var text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = GetScoreSummary(graphConnections, actionCount, score, bestScore);
        LevelSelections.CurrentLevel().bestScore = score;
    }

    public string GetScoreSummary(int connections, int actions, int score, int bestScore)
    {
        var primary = $"Webs:\t\t{connections}\nCommands:\t\t{actions}\nScore:\t\t{score}";
        if(bestScore >= 0)
        {
            primary += $"\nBest\t\t\t{bestScore}";
        }
        return primary;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

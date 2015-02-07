using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public void Update()
    {
        var players = new Dictionary<int, PlayerScore>();

        foreach (var score in ScoreTracker.Instance.Scores)
        {
            players[score.playerId] = score;
        }

        for (int i = 0; i < 10; i++)
        {
            if (!players.ContainsKey(i)) continue;

            var killText = transform.FindChild("Player " + (i) + " Kills") == null
                ? null
                : transform.FindChild("Player " + (i) + " Kills").GetComponent<Text>();
            var deathText = transform.FindChild("Player " + (i) + " Deaths") == null
                ? null
                : transform.FindChild("Player " + (i) + " Deaths").GetComponent<Text>();
            var hpText = transform.FindChild("Player " + (i) + " Health") == null
                ? null
                : transform.FindChild("Player " + (i) + " Health").GetComponent<Text>();

            if (killText != null)
                killText.text = players[i].kills.ToString();
        }
    }
}

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
            var killText = transform.FindChild("Player " + (i) + " Kills") == null
                ? null
                : transform.FindChild("Player " + (i) + " Kills").GetComponent<Text>();
            var deathText = transform.FindChild("Player " + (i) + " Deaths") == null
                ? null
                : transform.FindChild("Player " + (i) + " Deaths").GetComponent<Text>();
            var hpText = transform.FindChild("Player " + (i) + " Health") == null
                ? null
                : transform.FindChild("Player " + (i) + " Health").GetComponent<Text>();
            var join = transform.FindChild("Player " + (i) + " Join") == null
                ? null
                : transform.FindChild("Player " + (i) + " Join").GetComponent<Text>();

            if (players.ContainsKey(i))
            {
                if (killText != null)
                {
                    killText.text = players[i].kills.ToString();
                }
                if (deathText != null)
                {
                    deathText.text = players[i].deaths.ToString();
                }
                if (hpText != null)
                {
                    hpText.text = players[i].core != null ? (players[i].core.CurrentHealth/10).ToString() : "0";
                }
            }
            if (killText != null)
            {
                killText.enabled = players.ContainsKey(i);
            }
            if (deathText != null)
            {
                deathText.enabled = players.ContainsKey(i);
            }
            if (hpText != null)
            {
                hpText.enabled = players.ContainsKey(i);
            }
            if (join != null)
            {
                join.enabled = !players.ContainsKey(i) || players[i].core == null;
            }
        }
    }
}

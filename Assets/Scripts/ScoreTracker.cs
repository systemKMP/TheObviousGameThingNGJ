using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreTracker
{

    private static ScoreTracker _instance;
    private List<PlayerScore> playerScores;

    public List<PlayerScore> Scores { get { return _instance.playerScores; } }

    public static ScoreTracker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScoreTracker();
            }
            return _instance;
        }
    }

    private ScoreTracker()
    {
        playerScores = new List<PlayerScore>();
    }

    public void RegisterPlayer(int id, int actualId, PlayerCore core)
    {
        var scoreBoard = playerScores.SingleOrDefault(score => score.playerId == id);
        if (scoreBoard == null)
        {
            scoreBoard = new PlayerScore() { playerId = id, actualId = actualId, core = core };
            playerScores.Add(scoreBoard);
        }
        else
        {
            scoreBoard.core = core;
        }
    }

    public void RecordKill(int killerId, int casualtyId)
    {
        foreach (var player in playerScores)
        {
            if (player.playerId == killerId)
            {
                if (casualtyId != killerId)
                {
                    player.kills++;
                }
                else
                {
                    player.kills--;
                }
            }
            if (player.playerId == casualtyId)
            {
                player.deaths++;
            }
        }
    }

    public void Leave(int id)
    {
        playerScores.RemoveAll(score => score.playerId == id);
    }
}

public class PlayerScore
{
    public int playerId;
    public int actualId;
    public PlayerCore core;
    public int deaths;
    public int kills;
}
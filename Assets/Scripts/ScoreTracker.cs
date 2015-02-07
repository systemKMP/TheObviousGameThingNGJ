using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreTracker {

    private static ScoreTracker _instance;
    private List<PlayerScore> playerScores;
    
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

    public void RecordKill(int killerId, int casualtyId)
    {
        foreach (var player in playerScores)
        {
            if (player.playerId == killerId)
            {
                player.kills++;
            }
            else if (player.playerId == killerId)
            {
                player.deaths++;
            }

        }

        Debug.Log(killerId + " kills " + casualtyId);
    }
    
}

public class PlayerScore {
    public int playerId;
    public int deaths;
    public int kills;
}
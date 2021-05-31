using UnityEngine;
using Mirror;

// This class holds the class information for matches
[System.Serializable]
public class Match
{
    // matches need to hold a match ID, and a port number
    public string matchID;
    public int pNum;

    // matches also hold reference to the players within the match
    public SyncListGameObject players = new SyncListGameObject();

    // When a match is created, set the information for the match
    public Match(string id, GameObject player, int serverNum)
    {
        this.matchID = id;
        pNum = serverNum;
        players.Add(player);
        Debug.Log("Match Created");
    }

    public Match() { }
}

[System.Serializable]
public class SyncListGameObject: SyncList<GameObject> { }

[System.Serializable]
public class SyncListMatch: SyncList<Match> { }
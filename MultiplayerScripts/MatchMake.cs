using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MatchMake : NetworkBehaviour
{
    // Set a singleton for this class
    public static MatchMake instance;

    // Create a set of matches that will be synced across
    public SyncList<Match> matches = new SyncList<Match>();

    // Get a reference to the network manager
    public NetworkManager networkManager;

    // On start, we need a reference to this instance, and reference to the network manager
    void Start()
    {
        instance = this;
        networkManager = GameObject.Find("NetworkManager").GetComponent<TestingClientJoin>();
    }

    // Host game function called by the player
    // Function needs to create a new match in the set of matches
    // Then add the player to it, and return the port number that the player moves to
    // Return 0 if it fails
    public int HostGame(int id, GameObject p)
    {
        matches.Add(new Match(id.ToString(), p, matches.Count + 7778));
        Debug.Log($"Match ID: {matches[0].matchID.ToString()}");
        return matches.Count + 7777;
    }

    // Generate a random GUID to create a game 
    public static Guid GetRandomID()
    {
        Guid g = Guid.NewGuid();
        return g;
    }

    // Join game function called by the player
    // Function needs to check each of the matches in the list
    // If a match is found that is equal to the id of the player input, join
    // Send the port number
    // If there is no match, send a 0
    public int JoinGame(string id, GameObject p)
    {
        foreach(Match x in matches)
        {
            if (x.matchID == id)
            {
                x.players.Add(p);
                return x.pNum;
            }
        }
        return 0;
    }
}

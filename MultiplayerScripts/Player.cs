using UnityEngine;
using Mirror;


public class Player : NetworkBehaviour
{
    // Reference to the current player
    public static Player localP;
    
    // Reference to this player's current match ID it occupies
    [SyncVar]
    public string matchID;

    // Reference to the Network Match Checker to access so functions
    NetworkMatchChecker nmc;

    // Reference to the network manager currently in the scene
    GameObject nm;

    // On Start, set a singleton to the player, and get references to the network manager and network match checker
    void Start()
    {
        if (isLocalPlayer)
        {
            localP = this;
        }
        nmc = GetComponent<NetworkMatchChecker>();
        nm = GameObject.Find("NetworkManager");

    }

    // When the player chooses to host a game, create a new match in the set of matches
    public void HostGame()
    {
        // Due to server constrictions, we limit game instances to 2
        int matchID = MatchMake.instance.matches.Count;

        if (matchID > 2)
        {
            Debug.Log("Max matches");
            return;
        }

        // Send a command to the server to create the game using the randomly generated match ID
        CmdHostGame(matchID, this);
    }

    // When the player chooses to join a game, the player types an ID into the input
    // This input is checked across the matches in the set of matches
    // If the match is found, then add this player into that match
    public void JoinGame()
    {
        // Get a reference to the Lobby to access the input
        GameObject L = GameObject.Find("Lobby");
        string jID = L.GetComponent<Lobby>().GetJoinID();
        Debug.Log(jID);

        // Send the command to the server to join game
        CmdJoinGame(jID, this);
    }

    // Server command that adds the player to the game
    [Command]
    void CmdJoinGame(string id, Player p)
    {
        // Get the port number for the game matching the input ID
        // If there is no game with that ID, deal with 0
        int nPort = MatchMake.instance.JoinGame(id, p.gameObject);
        Debug.Log(nPort);
        if (nPort != 0)
        {
            TargetMoveToServer(nPort);
        }
    }

    // Server command that creates a new game, and adds the player to it
    [Command]
    void CmdHostGame(int id, Player p)
    {
        // Get reference to the port that will host this game
        int serverLink = MatchMake.instance.HostGame(id, p.gameObject);

        // If the reference is not 0, then the game host succeeded
        if (serverLink != 0)
        {
            Debug.Log("Game Hosted Successfully");
            // Begin moving the player to the port
            TargetMoveToServer(serverLink);
        }
    }

    // The server calls this function on the local player
    [TargetRpc]
    void TargetMoveToServer(int serverNumber)
    {
        Debug.Log("ttt");
        // Get reference to the server holder, which holds the port to go to
        GameObject.Find("ServerHolder").GetComponent<ServerHold>().serverDest = serverNumber;

        // Disconnect from the current port 7777
        nm.GetComponent<TestingClientJoin>().StopClient();
        // Connect to the port required
        GameObject.Find("ServerHolder").GetComponent<ServerHold>().Test();
    }
}

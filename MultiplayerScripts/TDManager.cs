using UnityEngine;
using Mirror;

public class TDManager : NetworkManager
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform lSpawn;

    [SerializeField]
    private Transform rSpawn;

    private GameObject spawner;

    // When the player connects, this function is called
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // Check if this is the first or second player to connect, give them corresponding spawn points
        Debug.Log("Connected, spawning grid");
        Transform p = numPlayers == 0 ? lSpawn : rSpawn;

        // Instantiate the grid for the player at their position
        GameObject pGrid = Instantiate(playerPrefab, p.position, p.rotation);
        NetworkServer.AddPlayerForConnection(conn, pGrid);

        Debug.Log($"Spawn Prefabs Count: {spawnPrefabs.Count}");


        // If the player is the first one there, get reference to the spawner
        // Begin setting up cameras
        if (numPlayers == 1)
        {
            pGrid.GetComponent<GeneratePCam>().SetCamera(GameObject.Find("LCamera"));
            GameObject.Find("P1UI").GetComponent<RectTransform>().SetParent(pGrid.transform, false);
            //foreach (GameObject x in spawnPrefabs)
            //{
            //    NetworkServer.Spawn(x);
            //}

            spawner = GameObject.Find("DualSpawn");
        }

        // If the second player connected, the game can start
        // Set the second players camera, start the waves
        if (numPlayers == 2)
        {
            pGrid.GetComponent<GeneratePCam>().SetCamera(GameObject.Find("RCamera"));
            GameObject.Find("P2UI").GetComponent<RectTransform>().SetParent(pGrid.transform, false);
            Debug.Log("2 players");
            NetworkServer.Destroy(GameObject.Find("JoinCode"));
            //MultiWave.instance.GetReferences();
            //spawner.GetComponent<MultiWave>().ServerSpawnEnemies();
        }
    }
    public void DisconnectPlayer()
    {
        Debug.Log("Stopping Client");
        StopClient();
    }
}

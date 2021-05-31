using System.Collections;
using UnityEngine;
using Mirror;

// This class deals with the enemy spawns, and mainly runs server side
public class MultiWave : NetworkBehaviour
{
    // Create a static instance that can be referenced anywhere and only one exists
    public static MultiWave instance;
    public NetworkManager nm;

    // Static binding 
    private void Start()
    {
        nm = GameObject.Find("NetworkManager").GetComponent<TDManager>();
        instance = this;
    }

    // Keep reference of both spawns for both players
    [SerializeField]
    private Vector3 lSpawn;

    [SerializeField]
    private Vector3 rSpawn;

    // Reference to both players, in order to manage credits and such
    public GameObject p1;

    public GameObject p2;

    // Information regarding both players being ready to start game, if 2 then both are ready
    public short r = 0;

    // List of waves for enemy spawns
    [SerializeField]
    private Wave[] sharedWaves;

    // The wave currently spawning
    private int waveIndex = 0;

    [Server]
    // The server will start a coroutine that spawns enemies
    public void SpawnEnemies()
    {
        StartCoroutine(Spawn());
    }

    [Server]
    // The multiwave instance gets reference to both players
    public void GetReferences()
    {
        InputManage x = GameObject.Find("InputManager").GetComponent<InputManage>();
        p1 = x.leftPlayer;
        p2 = x.rightPlayer;
        r++;
    }

    // The server spawn enemy function
    private IEnumerator Spawn()
    {
        // While there are waves, spawn enemies
        while (waveIndex < sharedWaves.Length)
        {
            // For each enemy listed in the wave, spawn them for both players
            for (int i = 0; i < sharedWaves[waveIndex].maxEnemyCount; i++)
            {
                // When creating an enemy, set for 1 player then the other to keep reference to give credits
                GameObject clone = Instantiate(sharedWaves[waveIndex].enemyPrefabs[0], lSpawn, Quaternion.identity);
                clone.GetComponent<EnemyLogistic>().SetPlayer(1);
                NetworkServer.Spawn(clone);
                clone = Instantiate(sharedWaves[waveIndex].enemyPrefabs[0], rSpawn, Quaternion.identity);
                clone.GetComponent<EnemyLogistic>().SetPlayer(2);
                NetworkServer.Spawn(clone);
                yield return new WaitForSeconds(sharedWaves[waveIndex].spawnTime);
            }
            // Wait a bit before sending the next wave
            yield return new WaitForSeconds(20f);
            waveIndex++;
        }
    }

    // Called when the right player spawns an enemy to the left
    public void LeftSpawn(int enemy)
    {
        GameObject clone = Instantiate(nm.spawnPrefabs[enemy], lSpawn, Quaternion.identity);
        clone.GetComponent<EnemyLogistic>().SetPlayer(1);
        NetworkServer.Spawn(clone);
    }

    // Called when the left player spawns an enemy to the right
    public void RightSpawn(int enemy)
    {
        GameObject clone = Instantiate(nm.spawnPrefabs[enemy], rSpawn, Quaternion.identity);
        clone.GetComponent<EnemyLogistic>().SetPlayer(2);
        NetworkServer.Spawn(clone);
    }

    // When an enemy is defeated, give credits to the player it was sent to
    public void KillCredit(short p, short c)
    {
        if (p == 1)
        {
            PController x = p1.GetComponent<PController>();
            x.CreditTransaction(c);
            x.TargetCreditUpdate(x.credits);
        }
        else
        {
            PController x = p2.GetComponent<PController>();
            x.CreditTransaction(c);
            x.TargetCreditUpdate(x.credits);
        }
    }

    // When an enemy reaches the goal, deal damage to the player that let the enemy through
    public void PlayerDamage(short p)
    {
        if (p == 1)
        {
            PController x = InputManage.instance.leftPlayer.GetComponent<PController>();
            x.CmdPlayerDamage();
            x.TargetHealthUpdate(x.pHealth);
        }
        else
        {
            PController x = InputManage.instance.rightPlayer.GetComponent<PController>();
            x.CmdPlayerDamage();
            x.TargetHealthUpdate(x.pHealth);
        }
    }

}

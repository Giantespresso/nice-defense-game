using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This class is to deal with enemy health behvaior
public class EnemyLogistic : NetworkBehaviour
{
    // Reference to which player this enemy is for
    public short pReference;

    // Value holds the number of credits that this enemy is worth
    [SerializeField]
    public short creditAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On start, set the player reference for this enemy
    public void SetPlayer(short p)
    {
        pReference = p;
    }

    // When the enemy is reduced to 0 health, destroy this object and reward credits
    public void DestroyEnemy()
    {
        MultiWave.instance.KillCredit(pReference, creditAmount);
        NetworkServer.Destroy(gameObject);
    }

    
}

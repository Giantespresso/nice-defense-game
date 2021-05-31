using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class deals with the enemy information 
public class EnemyButtonLogic : MonoBehaviour
{
    // Value holds what enemy this is
    [SerializeField]
    private int EType;

    // Value holds whether this enemy is on the left or right board
    [SerializeField]
    private bool left;

    // The cost of the enemy corresponding to this button
    [SerializeField]
    private short eCost;

    // Returns the enemy type
    public int GetEnemy()
    {
        return EType;
    }

    // Returns whether this enemy is on the left or right
    public bool IsLeft()
    {
        return left;
    }
    
    // Return the cost of this enemy
    public short GetCost()
    {
        return eCost;
    }
}

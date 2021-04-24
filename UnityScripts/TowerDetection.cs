using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class is used for towers to detect enemies,
 * and prioritize which enemies to attack */
public class TowerDetection : MonoBehaviour
{
    // A list of enemies within range
    private List<GameObject> targets = new List<GameObject>();

    // The tower's attack speed
    [SerializeField]
    private float attackSpeed;

    // The damage this tower will do
    [SerializeField]
    private int Damage;

    // When this tower can attack again
    private float nextAttack = 0;

    /* This function is called every fixed amount of frames
     * to ensure more consistent timings*/ 
    private void FixedUpdate()
    {
        /* If the the current time meets the old time plus the attack
         * timer, then this tower can attack again */
        if (Time.fixedTime >= nextAttack && targets.Count > 0)
        {
            Attack();
            nextAttack += attackSpeed;
        }
    }

    /* Whenever an object with a collider enters this tower's range
     * call this function to check if it is an enemy */
    private void OnTriggerEnter2D(Collider2D collision) {
        /* If the object is an enemy, then add it to the list */
        if (collision.tag == "Enemy") {
            targets.Add(collision.gameObject);
        }

        /* Sort the list to prioritize the enemy that has travelled
         * the most distance */
        targets.Sort((x, y) => y.GetComponent<Movement2D>().GetDistanceTravelled().CompareTo(x.GetComponent<Movement2D>().GetDistanceTravelled()));
    }

    /* When an enemy leaves this unit's range, remove it from
     * this tower's list of enemies */
    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(collision.gameObject);
    }

    /* Call the most forward enemy's recieve damage
     * function */
    private void Attack()
    {
        targets[0].GetComponent<EnemyHealth>().RecieveDamage(Damage);
    }

}

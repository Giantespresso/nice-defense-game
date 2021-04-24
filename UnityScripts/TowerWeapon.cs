using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget}

public class TowerWeapon : MonoBehaviour {
    [SerializeField]
    private GameObject projectilePrefab; //Prefab for projectile
    [SerializeField]
    private Transform spawnPoint; //The location that the projectile will spawn
    [SerializeField]
    private float attackRate = 0.5f; //Tower's attack rate
    [SerializeField]
    private float attackRange = 2.0f; //Tower's attack range
    [SerializeField]
    private int attackDamage = 1; //Tower's attack damage
    [SerializeField]
    public GameObject tempTowerPrefab;
    private int level = 0; //Tower's level
    public Projectile projectile;

    // List of targets within range
    private List<GameObject> targets = new List<GameObject>();

    private WeaponState weaponState = WeaponState.SearchTarget; //The state for the tower's weapon
    private Transform attackTarget = null; //The target to attack

    //Variables to display on tower data viewer panel
    public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;
    public int Level => level + 1;

    // If the enemy enters this tower's range, add the enemy to list of targets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* If the object is an enemy, then add it to the list */
        if (collision.tag == "Enemy")
        {
            targets.Add(collision.gameObject);
        }
    }

    // If an enemy comes into range, add the enemy to the list of targets
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            targets.Remove(collision.gameObject);
        }
    }

    private void Start()
    {
        Setup();
    }
    public void Setup() {
        //this.gameObject.GetComponent<CircleCollider2D>().radius = attackRange;

        //Set the first state as WeaponState.SearchTarget
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState (WeaponState newState) {
        //Stop the previous state
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        //Starting the new state
        StartCoroutine(weaponState.ToString());
    }

    private void FixedUpdate() {
        if (attackTarget != null) {
            RotateToTarget();
        }
    }

    private void RotateToTarget() {
        //Getting the location/degree of the target's position by this formula: degree = arctan(y/x)
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        //Getting the degree with the difference of x and y 
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    // Search Target is the standby mode for towers, when nothing is in range, the tower is in this state
    private IEnumerator SearchTarget() { 
        while (true) {
            ////Setting the initial distance as big as possible to start finding the enemy that's the closest
            //float closestDistSqr = Mathf.Infinity;

            ////Checking the existing enemies that's in the EnemySpawner's EnemyList
            //for (int i = 0; i < enemySpawner.EnemyList.Count; ++ i) {
            //    float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

            //    //If the enemy's within the range and the closest amongst all of the enemies that's been checked
            //    if (distance <= attackRange && distance <= closestDistSqr) {
            //        closestDistSqr = distance;
            //        attackTarget = enemySpawner.EnemyList[i].transform;
            //    }
            //}

            ////Attack the target if the attackTarget is not null
            //if (attackTarget != null) {
            //    ChangeState(WeaponState.AttackToTarget);
            //}

            if (targets.Count > 0)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    // Attack to target is the state in which there is an enemy in range of the tower
    private IEnumerator AttackToTarget() { 
        while (true) {
            ////1. Checking if there's a target
            //if (attackTarget == null) {
            //    ChangeState(WeaponState.SearchTarget);
            //    break;
            //}

            ////2. Checking if there's a target within a distance
            //float distance = Vector3.Distance(attackTarget.position, transform.position);
            //if (distance > attackRange) {
            //    attackTarget = null;
            //    ChangeState(WeaponState.SearchTarget);
            //    break;
            //}

            // Reset the target to null every run, to again find the furthest enemy in range
            attackTarget = null;


            // Find the furthest enemy in range iterating through the enemies in range
            float furthest = 0;
            foreach(GameObject x in targets)
            {
                if (x.GetComponent<Movement2D>().GetDistanceTravelled() > furthest)
                {
                    attackTarget = x.transform;
                    furthest = attackTarget.gameObject.GetComponent<Movement2D>().GetDistanceTravelled();
                }
            }

            // If we could not find an enemy, return to standby
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
            }
            //3. Waiting for another attack as long as attackRate value
            yield return new WaitForSeconds(attackRate);

            //4. Attack by spawning a projectile
            SpawnProjectile();
        }
    }

    private void SpawnProjectile() {
        //GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        //clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage);

        // Instantiate an object of projectile
        Projectile clone = Instantiate(projectile, spawnPoint.position, Quaternion.identity);

        // Adjust the projectile's parameters here
        clone.movement2D = clone.GetComponent<Movement2D>();
        clone.target = attackTarget;
        clone.damage = attackDamage;

    }
}

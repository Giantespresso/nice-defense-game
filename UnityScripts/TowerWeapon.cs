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

    [SerializeField]
    public int towerCost;

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
    private void OnTriggerEnter2D(Collider2D collision) {
        /* If the object is an enemy, then add it to the list */
        if (collision.tag == "Enemy") {
            Debug.Log("Enemy Found");
            targets.Add(collision.gameObject);
        }
    }

    // If an enemy comes into range, add the enemy to the list of targets
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Enemy")
        {
            targets.Remove(collision.gameObject);
        }
    }

    private void Start() {
        Setup();
    }

    public void Setup() {
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
            if (targets.Count > 0) {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }

    // Attack to target is the state in which there is an enemy in range of the tower
    private IEnumerator AttackToTarget() { 
        while (true) {
            // Reset the target to null every run, to again find the furthest enemy in range
            attackTarget = null;


            // Find the furthest enemy in range iterating through the enemies in range
            float furthest = 0;
            foreach(GameObject x in targets) {
                if (x.GetComponent<Movement2D>().GetDistanceTravelled() > furthest) {
                    attackTarget = x.transform;
                    furthest = attackTarget.gameObject.GetComponent<Movement2D>().GetDistanceTravelled();
                }
            }

            // If we could not find an enemy, return to standby
            if (attackTarget == null) {
                ChangeState(WeaponState.SearchTarget);
            }
            //3. Waiting for another attack as long as attackRate value
            yield return new WaitForSeconds(attackRate);

            //4. Attack by spawning a projectile
            SpawnProjectile();
        }
    }

    private void SpawnProjectile() {
        // Instantiate an object of projectile
        Projectile clone = Instantiate(projectile, spawnPoint.position, Quaternion.identity);

        // Adjust the projectile's parameters here
        clone.movement2D = clone.GetComponent<Movement2D>();
        clone.target = attackTarget;
        clone.damage = attackDamage;
        Debug.Log(attackDamage);
    }
}

using System.Collections;
using UnityEngine;

/* This script deals with enemy interaction with player towers, taking hits, and death */
public class EnemyHealth : MonoBehaviour {
    [SerializeField]
    private float maxHealth; //The max health points that this enemy has
    private float currentHealth; //Current health
    private bool isDead = false; //If the enemy is dead, set this to 'true'
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /* This function is exposed to enemy towers.
     * When a tower attacks an enemy, this function
     * reduces the health of this enemy*/
    public void RecieveDamage(float damage) {
        //if (isDead == true) return;

        // Subtract health from enemy
        currentHealth -= damage;

        //StopCoroutine("HitAlphaAnimation");
        //StartCoroutine("HitAlphaAnimation");

        /*If the enemy's health goes to 0, destroy it */
        if (currentHealth <= 0) {
            // Kill this unit
            //Debug.Log("test");
            enemy.OnDeath(EnemyDestroyType.kill);
        }
    }

    private IEnumerator HitAlphaAnimation() {
        //Saving the current enemy's color to 'color'
        Color color = spriteRenderer.color;

        //Changing the opacity to 50%
        color.a = 0.5f;
        spriteRenderer.color = color;

        //Wait for 0.05 seconds
        yield return new WaitForSeconds(0.05f);

        //Changing the opacity to 100%
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}

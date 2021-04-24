using UnityEngine;

public class SplashProjectile : Projectile
{
    // Gameobject that holds the splash effect
    [SerializeField]
    private GameObject Splash;
    private void Update()
    {
        //If there's a target
        if (target != null)
        {
            //Move the projectile object to the target's location
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            //Get rid of the projectile object
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If it collides with an object that's not an enemy
        if (!collision.CompareTag("Enemy")) return;
        //If it collides with an enemy that's not targetted
        if (collision.transform != target) return;
        
        // Create the splash effect here
        Instantiate(Splash, target.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

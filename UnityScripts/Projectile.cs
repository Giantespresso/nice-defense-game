using UnityEngine;

public class Projectile : MonoBehaviour {
    //private Movement2D movement2D;
    //private Transform target;
    //private int damage;
    public Movement2D movement2D;
    public Transform target;
    public float damage;

    //public void Setup(Transform target, int damage) {
    //    movement2D = GetComponent<Movement2D>();
    //    this.target = target;  //Target that the tower selected
    //    this.damage = damage; //Damage that the tower has
    //}

    private void Update() {
        //If there's a target
        if (target != null) {
            //Move the projectile object to the target's location
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else {
            //Get rid of the projectile object
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //If it collides with an object that's not an enemy
        if (!collision.CompareTag("Enemy")) return;
        //If it collides with an enemy that's not targetted
        if (collision.transform != target) return;

        collision.GetComponent<EnemyHealth>().RecieveDamage(damage);
        Destroy(gameObject);
    }
}

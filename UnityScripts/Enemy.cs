using System.Collections;
using UnityEngine;

public enum EnemyDestroyType { kill = 0, Arrive }
public class Enemy : MonoBehaviour {
    private int wayPointCount; //To store how many waypoints there are
    private Transform[] wayPoints; //To store the location of waypoints
    private int currentIndex = 0; //To store the index for the target waypoint
    private Movement2D movement2D; //To cntrol the object's movement
    private EnemySpawner enemySpawner; //Deleting the enemy through EnemySpawner.cs
    [SerializeField]
    private int credit = 10; //Setting the credit that the enemy drops when they die

    public void Setup(EnemySpawner enemySpawner, Transform[] wayPoints) {
        //Getting and storing a reference to the Movement2D component to access it
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //Setting the informations of waypoints
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount]; 
        this.wayPoints = wayPoints;

        /*Changing the enemy's location to the first wayPoint location (ex. curr index of 0 means
        the enemy's on the start)*/
        transform.position = wayPoints[currentIndex].position;

        /*Coroutine is a function that allows pausing its execution and resuming from the same
        point after a condition is met*/
        //Setting the enemy's target movement
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove() {
        NextMoveTo(); //Setting the next direction that the enemy has to move to

        while (true) {
            //Enemy object's rotation (just to make it look a little more dynamic)
            transform.Rotate(Vector3.forward * 10); 

            /*If the enemy is 0.02 * movement2D.MoveSpeed away from the next way point (right next
            to the next WayPoint), set the next target wayPoint for the enemy to move to*/
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed) {
                NextMoveTo();
            }

            /*Yield return null is the point at which execution will pause and be resumed after
            the following frame*/
            yield return null;
        }
    }

    private void NextMoveTo() { 
        //If there's still a way point left for the enemy to move to
        if (currentIndex < wayPointCount - 1) {
            //Set the enemy right on top of the way point
            transform.position = wayPoints[currentIndex].position;
            //To next way point
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        //If there's no way point left for the enemy
        else {
            //To ensure that the enemy doesn't give credit to the player when the enemy dies in base
            credit = 0;
            //Remove the enemy object
            OnDeath(EnemyDestroyType.Arrive);
        }
    }

    public void OnDeath(EnemyDestroyType type) {
        enemySpawner.DestroyEnemy(type, this, credit);
    }
}

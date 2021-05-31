using UnityEngine;
using Mirror;

// This class allows enemies to move 
public class EnemyAutoMove : NetworkBehaviour
{
    public float mSpeed;
    // The direction the enemy is currently moving
    private Vector3 currD;
    // Start is called before the first frame update
    void Start()
    {
        // All enemies start moving right
        currD = Vector3.right;
    }

    // The enemy will move at constant speed in their direction until they hit a waypoint
    private void FixedUpdate()
    {
        // When the enemy hits a waypoint, the waypoint will change the direction moving
        transform.position += mSpeed * currD * Time.deltaTime;
        if (currD == Vector3.zero)
        {
            MultiWave.instance.PlayerDamage(gameObject.GetComponent<EnemyLogistic>().pReference);
            NetworkServer.Destroy(gameObject);
        }
    }
    
    // Called when in contact with a waypoint, changes the direction
    public void SetDirection(Vector3 dir)
    {
        currD = dir;
    }
}

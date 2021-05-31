using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is for the waypoints on the map to tell the enemmies where to go
public class WayPointDirection : MonoBehaviour
{
    // The direction that this waypoint will push enemies
    public Vector3 dir;
    
    // On collision with enemies, access their direction value and change it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
            return;

        collision.gameObject.GetComponent<EnemyAutoMove>().SetDirection(dir);
    }
}

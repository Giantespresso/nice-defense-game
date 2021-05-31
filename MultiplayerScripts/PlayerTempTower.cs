using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that deals with the tower preview
public class PlayerTempTower : MonoBehaviour
{
    // Reference to the player's camera
    public Camera playerCam;    

    // Update is called once per frame
    void Update()
    {
        // If the player is not building a tower, not needed
        if (!transform.parent.GetComponent<PController>().activeTower)
        {
            Destroy(gameObject);
        }

        // Follow the player's cursor, get the mouse's position and translate it to world coordinates

        Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);

        transform.position = playerCam.ScreenToWorldPoint(mPosition);

    }
}

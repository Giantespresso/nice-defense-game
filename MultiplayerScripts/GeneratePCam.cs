using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This class is used to set up each player's camera
public class GeneratePCam : NetworkBehaviour
{
    
    // This function is called on each client
    // The first client disables all cameras, and then sets theirs to be enabled
    [TargetRpc]
    public void SetCamera(GameObject cam)
    {
        // Adjust Canvas overlays to not block client buttons
        if (cam.name == "LCamera")
        {
            GameObject.Find("P2UI").GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GameObject.Find("P1UI").GetComponent<Canvas>().enabled = false;
        }

        // Disable all cameras, and get the camera for this player
        if (isLocalPlayer)
        {
            Debug.Log("Set Cam");
            foreach(Camera x in Camera.allCameras)
            {
                Debug.Log("Disable Camera");
                x.enabled = false;
                x.gameObject.GetComponent<AudioListener>().enabled = false;
            }

            cam.GetComponent<Camera>().enabled = true;
            cam.GetComponent<AudioListener>().enabled = true;
        }
        gameObject.GetComponent<PController>().CameraAssist(cam);
    }
}

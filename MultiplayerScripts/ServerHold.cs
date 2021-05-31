using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents an object that will be used to hold the port number
// that his player should move to if they play multiplayer
public class ServerHold : MonoBehaviour
{
    // Server Destination for the player, holds the port number
    public int serverDest;
    // Start is called before the first frame update

    // Do not destroy this object on load, since we need it later
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Function that starts the connection process to the new port
    public void Test()
    {
        // Start a coroutine that connects to the new port
        StartCoroutine(NewConnect());
    }

    // Connect to the new port
    IEnumerator NewConnect()
    {
        // I found that waiting for a bit makes this process work
        yield return new WaitForSeconds(3);
        Debug.Log("Connect");

        // Change the player prefab for the network manager so there is no issue
        GameObject.Find("NetworkManager").GetComponent<TestingClientJoin>().ChangeObject();
        // Change the port number for the connection object
        GameObject.Find("NetworkManager").GetComponent<Mirror.SimpleWeb.SimpleWebTransport>().port = (ushort)serverDest;
        // Connect to the client using the start client function of the network manager
        GameObject.Find("NetworkManager").GetComponent<TestingClientJoin>().StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

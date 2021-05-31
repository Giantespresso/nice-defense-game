using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

// Represents game object that holds the UI elements
public class Lobby : NetworkBehaviour
{
    // All the UI elements used
    [SerializeField] InputField joinID;
    [SerializeField] Canvas HJInterface;
    [SerializeField] Canvas HInterface;
    [SerializeField] Text idDisplay;

    // Calls the player join game function
    public void JoinGame()
    {
        Player.localP.JoinGame();
    }

    // Call the player host game function
    public void HostGame()
    {
        HJInterface.enabled = false;
        HInterface.enabled = true;
        Player.localP.HostGame();

        StartCoroutine(SetDisplayID());
    }

    private IEnumerator SetDisplayID()
    {
        yield return new WaitForSeconds(3);
    }

    public string GetJoinID()
    {
        return joinID.text;
    }


}

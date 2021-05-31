using UnityEngine;
using Mirror;

// Deals with Button logic of clients
public class InputManage : NetworkBehaviour
{
    public static InputManage instance;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject leftPlayer = null;

    public GameObject rightPlayer = null;

    // Dealing with enemy spawn buttons
    // If the player clicks spawn button, decide which player it is
    // Then pass to the server the command to spawn
    public void SpawnEnemyBtn(GameObject e)
    {
        if (e.GetComponent<EnemyButtonLogic>().IsLeft())
        {
            if (leftPlayer.GetComponent<PController>().credits < e.GetComponent<EnemyButtonLogic>().GetCost())
                return;

            leftPlayer.GetComponent<PController>().CmdBtnSpawn(e);
        }
        else
        {
            if (rightPlayer.GetComponent<PController>().credits < e.GetComponent<EnemyButtonLogic>().GetCost())
                return;

            rightPlayer.GetComponent<PController>().CmdBtnSpawn(e);
        }
    }

    // Deal with tower button spawning
    public void SpawnTowerBtn(GameObject btn)
    {
        if (btn.GetComponent<TowerButtonLogic>().isLeft())
        {
            // Leftplayer is spawning tower
            leftPlayer.GetComponent<PController>().selectedTower = btn.GetComponent<TowerButtonLogic>().GetTower();
            leftPlayer.GetComponent<PController>().activeTower = true;
            leftPlayer.GetComponent<PController>().CreatePreview(btn.GetComponent<TowerButtonLogic>().GetPreview());
        }
        else
        {
            // Right Player is spawning tower
            rightPlayer.GetComponent<PController>().selectedTower = btn.GetComponent<TowerButtonLogic>().GetTower();
            rightPlayer.GetComponent<PController>().activeTower = true;
            rightPlayer.GetComponent<PController>().CreatePreview(btn.GetComponent<TowerButtonLogic>().GetPreview());
        }
    }

    // When the start button is hit, call ready on that player
    public void StartButton(bool left)
    {
        if (left)
        {
            leftPlayer.GetComponent<PController>().CmdReady();
        }
        else
        {
            rightPlayer.GetComponent<PController>().CmdReady();

        }
    }
}

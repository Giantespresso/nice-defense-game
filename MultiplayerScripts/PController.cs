using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

// This class deals with controlling the player, and keeping track of player information
public class PController : NetworkBehaviour
{
    // This is a reference to the networkmanager being used, inorder to access spawnable prefabs
    [SerializeField]
    NetworkManager nm;

    // The currently selected tower for the player
    public int selectedTower = -1;

    // Whether the player has an active tower selected or not
    public bool activeTower = false;

    // A raycast to keep track of what the player is clicking on
    private RaycastHit hit;

    // The camera that this player is looking through
    private Camera pCam;

    // The player information for health, credit, and their displays
    public int credits = 100;
    public short pHealth = 1;
    public GameObject towerPanel;
    public Text cDisplay;
    public Text hDisplay;
    public Text towerName;
    public Text towerRange;
    public Text towerDamage;
    public Text gResult;
    // Start is called before the first frame update

    // At start, get references to each of the player's UI elements, and give reference to the input manager
    private void Start()
    {
        Debug.Log($"Starting Health {pHealth}");
        nm = GameObject.Find("NetworkManager").GetComponent<TDManager>();
           
        if (GameObject.Find("InputManager").GetComponent<InputManage>().leftPlayer == null)
        {
            GameObject.Find("InputManager").GetComponent<InputManage>().leftPlayer = gameObject;
            cDisplay = GameObject.Find("P1C").GetComponent<Text>();
            cDisplay.text = credits.ToString();
            hDisplay = GameObject.Find("P1H").GetComponent<Text>();
            hDisplay.text = pHealth.ToString();
            towerName = GameObject.Find("P1TN").GetComponent<Text>();
            towerRange = GameObject.Find("P1TR").GetComponent<Text>();
            towerDamage = GameObject.Find("P1TD").GetComponent<Text>();
            towerPanel = GameObject.Find("P1TP");
            gResult = GameObject.Find("P1Result").GetComponent<Text>();
        }
        else
        {
            GameObject.Find("InputManager").GetComponent<InputManage>().rightPlayer = gameObject;
            cDisplay = GameObject.Find("P2C").GetComponent<Text>();
            cDisplay.text = credits.ToString();
            hDisplay = GameObject.Find("P2H").GetComponent<Text>();
            hDisplay.text = pHealth.ToString();
            towerName = GameObject.Find("P2TN").GetComponent<Text>();
            towerRange = GameObject.Find("P2TR").GetComponent<Text>();
            towerDamage = GameObject.Find("P2TD").GetComponent<Text>();
            towerPanel = GameObject.Find("P2TP");
            gResult = GameObject.Find("P2Result").GetComponent<Text>();

        }
        gResult.gameObject.SetActive(false);
    }

    // Keep track of what the player clicks
    private void Update()
    {
        // If not the current player, return
        if (!isLocalPlayer)
            return;

        // If you click, check what is clicked on
        if (Input.GetMouseButtonDown(0))
        {
            // Get the raycast of where the player clicked, so we can process it
            Vector3 pos = pCam.ScreenToWorldPoint(Input.mousePosition);
            if (Physics.Raycast(pos, Vector3.forward, out hit, 11f))
            {
                // If the player's click hits a tower, then change the tower data panel
                if (hit.collider.gameObject.tag == "Tower")
                {
                    Debug.Log(hit.collider.gameObject);
                    //towerPanel.SetActive(true);
                    towerName.text = hit.collider.gameObject.name.Substring(0, hit.collider.gameObject.name.Length - 7);
                    towerRange.text = $"Tower Range: {hit.collider.gameObject.GetComponent<TowerWeapon>().Range}";
                    towerDamage.text = $"Tower DamageL {hit.collider.gameObject.GetComponent<TowerWeapon>().Damage}";
                }
                // If the player clicks on a tile, then if the player has a selected tower, put the tower there
                if (hit.collider.gameObject.transform.parent.GetComponentInParent<PController>().isLocalPlayer)
                {
                    if (selectedTower != -1)
                    {
                        if (hit.collider.gameObject.tag == "Tile")
                        {
                            CmdTowerSpawn(selectedTower, hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                            selectedTower = -1;
                            activeTower = false;
                        }
                    }
                }
            }
        }
    }

    // Set this player's camera
    public void CameraAssist(GameObject cam)
    {
        pCam = cam.GetComponent<Camera>();
    }

    // Do credit transactions for this player
    public bool CreditTransaction(int amount)
    {
        if (credits + amount < 0)
        {
            return false;
        }
        else
        {
            credits += amount;
            return true;
        }
    }

    [Command]
    // Call on the server to spawn an enemy to the enemy side
    public void CmdBtnSpawn(GameObject btn)
    {
        Debug.Log($"Button Spawn {btn}");
        EnemyButtonLogic x = btn.GetComponent<EnemyButtonLogic>();
        if (x.IsLeft())
        {
            credits -= x.GetCost();
            TargetCreditUpdate(credits);
            MultiWave.instance.RightSpawn(x.GetEnemy());
        }
        else
        {
            credits -= x.GetCost();
            TargetCreditUpdate(credits);
            MultiWave.instance.LeftSpawn(x.GetEnemy());
        }
    }

    [Command]
    // Call from the server to spawn a tower on the selected coordinates
    public void CmdTowerSpawn(int tower, float x, float y, float z)
    {
        // If the tower's cost is in the player's budget, not then reject
        if (nm.spawnPrefabs[tower].GetComponent<TowerWeapon>().towerCost > credits)
        {
            Debug.Log("Tower is too expensive");
            return;
        }

        // Reduce the player's credits
        credits -= nm.spawnPrefabs[tower].GetComponent<TowerWeapon>().towerCost;
        TargetCreditUpdate(credits);

        // Spawn the tower calling the server spawn method
        Vector3 position = new Vector3(x, y, z) + Vector3.back;
        GameObject tClone = Instantiate(nm.spawnPrefabs[tower], position, Quaternion.identity);
        NetworkServer.Spawn(tClone);
        tClone.GetComponent<TowerWeapon>().Setup();
    }

    // When the tower button is selected, generate a preview for the local client
    public void CreatePreview(GameObject preview)
    {
        // If not the local player, then this client does not need the preview
        if (!isLocalPlayer)
            return;

        // Instantiate the tower preview game object locally
        Debug.Log("Creating Preview");
        GameObject tClone = Instantiate(preview, Input.mousePosition, Quaternion.identity);
        tClone.GetComponent<PlayerTempTower>().playerCam = pCam;
        tClone.transform.SetParent(transform);
    }

    [Command]
    // Send on the server that this player is ready
    public void CmdReady()
    {
        MultiWave.instance.GetReferences();

        // If both players are ready, then spawn enemies
        if (MultiWave.instance.r == 2)
        {
            MultiWave.instance.SpawnEnemies();
        }
    }

    [TargetRpc]
    // On the specific client, update the player's credits
    public void TargetCreditUpdate(int update)
    {
        if (!isLocalPlayer)
            return;

        credits = update;
        cDisplay.text = credits.ToString();
    }

    // The player takes damage on server, send the update to clients later
    public void CmdPlayerDamage()
    {
        pHealth--;
    }

    [TargetRpc]
    // On the specific client, reduce their HP 
    public void TargetHealthUpdate(short h)
    {
        if (!isLocalPlayer)
            return;

        pHealth = h;
        hDisplay.text = pHealth.ToString();
        if (pHealth <= 0)
        {
            Debug.Log("No Health");
            CmdEndGame();
        }
    }

    [Command]
    public void CmdEndGame()
    {
        InputManage.instance.leftPlayer.GetComponent<PController>().TargetEndGame();
        InputManage.instance.rightPlayer.GetComponent<PController>().TargetEndGame();
        Time.timeScale = 0f;
    }

    [TargetRpc]
    public void TargetEndGame()
    {
        Time.timeScale = 0f;
        if (pHealth <= 0)
        {
            gResult.gameObject.SetActive(true);
            gResult.text = "You Lost";
            Debug.Log("Lose");
        }
        else
        {
            gResult.gameObject.SetActive(true);
            gResult.text = "You Win";
        }

        StartCoroutine(DisconnectPlayer());
    }

    IEnumerator DisconnectPlayer()
    {
        Debug.Log("Waiting for 2");
        yield return new WaitForSecondsRealtime(2f);

        Debug.Log("Ending Client");
        GameObject.Find("NetworkManager").GetComponent<TestingClientJoin>().DisconnectPlayer();
    }

}

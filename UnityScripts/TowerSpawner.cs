using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildCredit = 50;
    [SerializeField]
    private EnemySpawner enemySpawner; //To get the list of enemies
    [SerializeField]
    private CreditManager credit;
    [SerializeField]
    public GameObject tempTowerPrefab;
    private bool isOnTowerButton = false; //To check if the tower button is clicked
    private GameObject tempTowerClone = null; //Variable for deleting the temp tower after its usage
    public void ReadyToSpawnTower()
    {
        if (isOnTowerButton == true)
        {
            return;
        }

        if (SelectedTower.towerInstance.GetCost() > credit.Credits)
        {
            return;
        }

        isOnTowerButton = true;
        tempTowerClone = Instantiate(tempTowerPrefab);
    }

    public void SpawnTower(Transform tileTransform)
    {
        if (SelectedTower.towerInstance.GetTower() == null)
        {
            return;
        }

        if (SelectedTower.towerInstance.GetCost() > CreditManager.CreditInstance.GetCredits())
        {
            Debug.Log("Not Enough Credits");
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        //Check if it is possible to build the tower
        if (tile.IsBuildTower == true)
        {
            return;
        }

        //Setting the isOnTowerButton false so that the user can select another tower
        isOnTowerButton = false;

        //Set the IsBuildTower to true to prevent additional towers from being spawned on the same tile
        tile.IsBuildTower = true;

        //Subtracting the gold from the credit
        credit.Credits -= SelectedTower.towerInstance.GetCost();

        //Build the tower at the selected tile
        Vector3 position = tileTransform.position + Vector3.back;
        GameObject clone = Instantiate(SelectedTower.towerInstance.GetTower(), position, Quaternion.identity);
        Debug.Log("test");
        //Sending enemySpawner to towerWeapon
        clone.GetComponent<TowerWeapon>().Setup();

        SelectedTower.towerInstance.ResetTower();

        //Destroying the tempTowerClone when the user builds a tower
        Destroy(tempTowerClone);
    }
}
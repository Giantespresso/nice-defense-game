using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image towerImage;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;

    private TowerWeapon currentTower;
    private void Awake() {
        PanelOff();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelOff();
        }
    }

    public void PanelOn(Transform towerWeapon) {
        //Get the data for the tower stats
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();

    }

    public void PanelOff() {
        gameObject.SetActive(false);
    }

    private void UpdateTowerData() {
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;
    }
}

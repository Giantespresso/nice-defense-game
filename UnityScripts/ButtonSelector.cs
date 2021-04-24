using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* This class is put on every button for tower selection
 * Whenever the player selects the button, the set tower changes */
public class ButtonSelector : MonoBehaviour
{
    // This buttons corresponding tower
    [SerializeField]
    private GameObject towerPrefab;

    // The tower's corresponding preview
    [SerializeField]
    private GameObject towerPreview;

    // A reference to the button
    [SerializeField]
    private Button button;

    // The tower's corresponding cost
    [SerializeField]
    private int towerCost;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(SelectTower);
    }

    // On button click, set the player's selected tower
    private void SelectTower()
    {
        SelectedTower.towerInstance.SetTower(towerPrefab, towerCost);
        Instantiate(towerPreview);
    }
}

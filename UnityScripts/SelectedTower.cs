using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This class deals with the player's selected tower currently
The class manages what tower is selected, and the cost */
public class SelectedTower : MonoBehaviour {
    // Create a static instance of the class to be referenced anywhere
    public static SelectedTower towerInstance;

    // Variable holding selected tower
    private GameObject towerP = null;

    // The cost of the selected tower
    private int tCost;

    // Ensure there is always 1 instance of the selected tower
    private void Awake() {
        if (towerInstance == null)
        {
            towerInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Whenever a tower button is selected, set the selected tower
    public void SetTower(GameObject t, int cost) {
        towerP = t;
        tCost = cost;
        Debug.Log(towerP);
    }
    
    // Getter for the selected tower
    public GameObject GetTower() {
        return towerP;
    }

    // Getter for the cost of the selected tower
    public int GetCost() {
        return tCost;
    }

    // Setter to reset the tower
    public void ResetTower() {
        towerP = null;
        tCost = 0;
    }
}

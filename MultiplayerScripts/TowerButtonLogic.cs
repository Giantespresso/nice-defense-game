using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This button is linked to the information regarding the tower buttons
public class TowerButtonLogic : MonoBehaviour
{
    // Number corresponding to the index of the spawnable prefabs
    [SerializeField]
    private int tower;

    // The gameobject representing the tower preview
    [SerializeField]
    private GameObject tPreview;

    // The bool representing if this button is left or right player
    [SerializeField]
    private bool left;
    
    // Returns the Tower's index in the spawnable prefabs list
    public int GetTower()
    {
        return tower;
    }

    // Returns the tower preview object
    public GameObject GetPreview()
    {
        return tPreview;
    }

    // Returns whether this button belongs to the left or right player
    public bool isLeft()
    {
        return left;
    }
}

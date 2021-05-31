using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// This class holds infomation on the tile of the player grid
public class NetworkTile : NetworkBehaviour
{
    // Value that holds if there is a tower on this tile
    public bool IsBuildTower { set; get; }

    // Start is called before the first frame update
    // All towers at start are not present, so all tiles are empty
    private void Start()
    {
        IsBuildTower = false;
    }
}

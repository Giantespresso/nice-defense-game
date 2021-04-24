using UnityEngine;

public class Tile : MonoBehaviour
{
    //To check if there's a tower that's already existing in a tile that's clicked
    public bool IsBuildTower { set; get; }

    private void Awake() {
        IsBuildTower = false;
    }
}

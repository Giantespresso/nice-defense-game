using UnityEngine;

// Allows the temp tower to follow the mouse before the user places the tower on the tile
public class ObjectFollowMousePosition : MonoBehaviour {
    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void Update() {
        // If the user clicks, the preivew tower is removed
        if (Input.GetMouseButtonDown(0)) {
            Destroy(this.gameObject);
        }

        //Getting the game scene's position through the mouse's position
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        //Making the z's position 0
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}

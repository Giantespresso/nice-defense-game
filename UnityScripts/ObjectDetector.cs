using UnityEngine;

public class ObjectDetector : MonoBehaviour {
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake() {
        /*After searching for the object that has the "MainCamera" tag, sends the Camera component
         info to it (This is the exact same as this:
         GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();)*/
        mainCamera = Camera.main;
    }

    private void Update() {
        //If the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) {
            //Creating the ray that goes between the position of the camera and the mouse position
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            //Enables us to select the objects from the 3D world from 2D world
            //Saving the objects that is detected by the ray to "hit"
            if (Physics.Raycast(pos, Vector3.forward, out hit, 11f)) { 
                //If the object that is detected by the ray has "tile" as a tag
                if (hit.transform.CompareTag("Tile")) {
                    //Call SpawnTower()
                    towerSpawner.SpawnTower(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower")) {
                    Debug.Log(hit.transform.gameObject);
                    towerDataViewer.PanelOn(hit.transform);
                }
                Debug.Log(hit.transform.gameObject);
            }
        }
    }
}

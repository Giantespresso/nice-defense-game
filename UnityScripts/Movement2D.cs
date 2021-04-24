using UnityEngine;

/*The classe's name has to match the file's name and MonoBehaviour is just the base class that
most Unity script utilizes in order to make things easier by adding some life cycle functions*/
public class Movement2D : MonoBehaviour {
    /*SerializeField allows the private variables to be converted into bytes to store objects
    into memory so that it shows up in the Unity Editor even if it's private*/
    [SerializeField]
    private float moveSpeed = 0.0f; //Setting the move speed for the enemies

    [SerializeField]
    private Vector3 moveDirection = Vector3.zero; //Setting the enemy's move direction

    public float MoveSpeed => moveSpeed;
    private float distanceTravelled = 0;

    /*The Update function constantly updates the game (once per frame), updating the position
    of the enemies*/
    private void Update() {
        distanceTravelled += moveSpeed * Time.deltaTime;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    /*The MoveTo function takes in the information and sets the enemy's moveDirection to the
    new one*/
    public void MoveTo(Vector3 direction) {
        moveDirection = direction;
    }

    /* Returns the distance that this enemy travelled, this allows us to decide which enemy
    towers should prioritize*/
    public float GetDistanceTravelled()
    {
        return distanceTravelled;
    }
}
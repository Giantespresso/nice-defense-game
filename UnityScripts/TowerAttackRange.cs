using UnityEngine;

public class TowerAttackRange : MonoBehaviour {
    private void Awake() {
        AttackRangeOff();
    }

    public void AttackRangeOn(Vector3 position, float range) {
        gameObject.SetActive(true);

        //Attack range
        float diameter = range * 2.0f;
        //Attack range position
        transform.position = position;
    }

    public void AttackRangeOff() {
        gameObject.SetActive(false);
    }
}

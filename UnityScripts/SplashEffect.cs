using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashEffect : MonoBehaviour {
    public float damage;
    private float timeOut = 0.1f;
    // Start is called before the first frame update
    void Start() {
        Destroy(this.gameObject, timeOut);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            collision.GetComponent<EnemyHealth>().RecieveDamage(damage);
        }
    }
}

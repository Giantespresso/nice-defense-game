using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 20;
    private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage) {
        currentHealth -= damage;

        //If the current health <= 0, game is over
        if (currentHealth <= 0) {
            SceneManager.LoadScene("SinglePlayerGameOver");
        }
    }
}

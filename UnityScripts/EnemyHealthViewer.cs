using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthViewer : MonoBehaviour {
    private EnemyHealth enemyHealth;
    private Slider healthSlider;


    //Saving the values from the enemyHP component
    public void Setup(EnemyHealth enemyHealth) {
        this.enemyHealth = enemyHealth;
        healthSlider = GetComponent<Slider>();
    }

    //Setting the value for the slider
    private void Update() {
        healthSlider.value = enemyHealth.CurrentHealth / enemyHealth.MaxHealth;    
    }
}

using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI textPlayerHealth;
    [SerializeField]
    private TextMeshProUGUI textPlayerCredit;
    [SerializeField]
    private TextMeshProUGUI textWave;
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private CreditManager credit;
    [SerializeField]
    private EnemyWave enemyWave;

    private void Update() {
        textPlayerHealth.text = playerHealth.CurrentHealth + "/" + playerHealth.MaxHealth;
        textPlayerCredit.text = credit.Credits.ToString();
        textWave.text = enemyWave.CurrentWave + "/" + enemyWave.MaxWave;
    }
}

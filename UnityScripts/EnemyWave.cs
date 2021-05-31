using UnityEngine;

public class EnemyWave : MonoBehaviour {
    [SerializeField]
    private Wave[] waves; //Information of all of the current stage's waves
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1; //Current wave index

    //Wave inforrmation
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;

    public void StartWave() {
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1) {
            currentWaveIndex++;
            //Call the StartWave() and provide the current wave info
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

[System.Serializable]
public struct Wave {
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    private GameObject enemyHealthSliderPrefab; //Slider UI prefab that displays the enemy Health
    [SerializeField]
    private Transform canvasTransform; //Canvas object's transform
    [SerializeField]
    private Transform[] wayPoints; //Current stage's enemy path
    [SerializeField]
    private PlayerHealth playerHealth;
    [SerializeField]
    private CreditManager creditManager;
    private Wave currentWave;
    private List<Enemy> enemyList; //Info about all of the enemies that exist on the current map
    public List<Enemy> EnemyList => enemyList;

    private void Awake() {
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wave wave) {
        currentWave = wave;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy() {
        //To keep the number of the enemies that's been spawned on this wave
        int spawnEnemyCount = 0;
        
        //Stop the coroutine after the spawnEnemyCount reaches the maxEnemyCount
        while (spawnEnemyCount < currentWave.maxEnemyCount) {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>(); //The component of the enemy that was just created

            //Calling Setup() with wayPoints value
            enemy.Setup(this, wayPoints); 
            //Saving the enemy that's been just created to a list
            enemyList.Add(enemy);

            SpawnEnemyHealthSlider(clone);

            //To keep track of how many enemies has been spawned
            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime); //Pause for the amount of spawnTime
        }
    }

    public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, int credit) {
        //If the enemy has arrived to the base, take off 1 from playerHealth
        if (type == EnemyDestroyType.Arrive) {
            playerHealth.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.kill) {
            creditManager.Credits += credit;
            //Debug.Log(creditManager.Credits);
        }
        //Deleting the enemy from the list when it dies
        enemyList.Remove(enemy);
        //Deleting the enemy object
        Destroy(enemy.gameObject);
    }

    private void SpawnEnemyHealthSlider(GameObject enemy) {
        //Creating a UI that shows enemy's health by utilizing the slider
        GameObject sliderClone = Instantiate(enemyHealthSliderPrefab);

        sliderClone.transform.SetParent(canvasTransform);
        sliderClone.transform.localScale = Vector3.one;

        //Set the target for Slider UI to be the enemy
        sliderClone.GetComponent<SliderPositionAutoSettler>().Setup(enemy.transform);
        //To show the current health through Slider UI
        sliderClone.GetComponent<EnemyHealthViewer>().Setup(enemy.GetComponent<EnemyHealth>());
    }
}

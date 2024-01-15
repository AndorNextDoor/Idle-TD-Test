using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner instance;

    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves;
    private float waveTimer;

    [SerializeField] private Wave[] waves;
    private int waveIndex;

    [HideInInspector]
    public bool needToSpawnWave;

    private int enemiesSpawnedAmount;

    [SerializeField] private Transform[] spawnPos;


    [System.Serializable]
    public class Wave
    {
        [Serializable]
        public class EnemyInfo
        {
            public GameObject enemyPrefab;
            public int quantity;
            public float spawnDelay = 0.3f;
        }

        public EnemyInfo[] enemies;
    }


    private void Awake()
    {
        instance = this;
        needToSpawnWave = true;
        waveTimer = timeBetweenWaves;
    }
    private void Update()
    {
        if (!needToSpawnWave) return;

        waveTimer -= Time.deltaTime;

        UiManager.instance.SetWaveTimer((int)waveTimer);


        if (waveTimer <= 0 && needToSpawnWave)
        {
            needToSpawnWave = false;
            enemiesSpawnedAmount = 0;
            waveTimer = timeBetweenWaves;

            if (waveIndex > waves.Length - 1)
            {
                Debug.Log("YOU WIN");
            }
            else
            {
                needToSpawnWave = false;
                StartCoroutine(Spawn());
            }
        }

        
    }

    IEnumerator Spawn()
    {

        foreach(Wave.EnemyInfo enemy in waves[waveIndex].enemies)
        {
            enemiesSpawnedAmount += enemy.quantity;
        }
        GameManager.Instance.SetEnemiesAmount(enemiesSpawnedAmount);

        foreach (Wave.EnemyInfo enemy in waves[waveIndex].enemies)
        {
            for (int i = 0; i < enemy.quantity; i++)
            {
                int lane = (i + 1) % spawnPos.Length;
                GameObject spawnedEnemy = Instantiate(enemy.enemyPrefab, spawnPos[lane].transform.position,enemy.enemyPrefab.transform.rotation);
                spawnedEnemy.GetComponent<Enemy>().SetLane(lane);
                yield return new WaitForSeconds(enemy.spawnDelay);
            }
        }
        waveIndex++;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int lives;

    private int enemiesAmount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UiManager.instance.SetLivesText(lives);
    }

    public void SetEnemiesAmount(int amount)
    {
        enemiesAmount = amount;
    }

    public void TakeDamage(int damage)
    {
        lives -= damage;
        UiManager.instance.SetLivesText(lives);

        if(lives <= 0)
        {
            // Game over
        }
    }

    public void OnEmemyDestroyed()
    {
        enemiesAmount--;
        if (enemiesAmount <= 0)
        {
            WaveSpawner.instance.needToSpawnWave = true;
        }
    }
}
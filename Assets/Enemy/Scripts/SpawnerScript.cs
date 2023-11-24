using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab;

    private float timeSpawn;

    [SerializeField]
    private float timeCharacterSpawn;

    [SerializeField]
    private GameObject character;

    [SerializeField]
    float distanceFromPlayer;

    public static int waveNumber = 0;

    [SerializeField]
    private int initialEnemyCount;

    [SerializeField]
    private float enemyMultiplier;

    public static int currentEnemies = 0;

    [SerializeField]
    //private TextMeshProUGUI textTimer;

    private bool isWaveStarting = false; 
    
    void Start()
    {
        timeSpawn = 5;
    }

    void Update()
    {
        timeSpawn -= Time.deltaTime;

        //if (!isWaveStarting)
        //{
            //textTimer.text = "Wave time left: " + timeSpawn.ToString("0.00");
        //}

        if ((timeSpawn < 0 || currentEnemies == 0) && !isWaveStarting)
        {
            isWaveStarting = true;
            //textTimer.text = "Next wave starting...";
            Invoke("StartWave", 5); // Wywo�aj funkcj� StartWave po 5 sekundach
        }
    }

    void StartWave()
    {
        waveNumber++;
        int enemyCountForThisWave = Mathf.FloorToInt(initialEnemyCount * Mathf.Pow(enemyMultiplier, waveNumber));
        for (int i = 0; i < enemyCountForThisWave; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Vector3 desiredPosition = character.transform.position + (randomRotation * Vector3.forward * distanceFromPlayer);

            int enemyIndex = 0;
            if (waveNumber >= 7)
            {
                enemyIndex = Random.Range(0, 4); // Losuje mi�dzy 0 a 3 (w��cznie z 0, wy��cznie 4)
            }
            else if (waveNumber >= 5)
            {
                enemyIndex = Random.Range(0, 3); // Losuje mi�dzy 0 a 2
            }
            else if (waveNumber >= 2)
            {
                enemyIndex = Random.Range(0, 2); // Losuje mi�dzy 0 a 1
            }
            else
            {
                enemyIndex = 0; // Tylko moby z indeksem 0
            }

            Instantiate(enemyPrefab[enemyIndex], desiredPosition, randomRotation);
            currentEnemies++; // Inkrementuj liczb� wrog�w
        }
        timeSpawn = timeCharacterSpawn * Mathf.Pow(enemyMultiplier, waveNumber) * 1.3f;
        isWaveStarting = false; // Resetuj flag�
    }
}
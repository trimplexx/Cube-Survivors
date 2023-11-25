using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshProUGUI scoreText; // U¿ywamy TextMeshProUGUI

    private Shooting shootingScript; // Referencja do skryptu Shooting
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        shootingScript = FindObjectOfType<Shooting>(); // Szukamy skryptu Shooting
        if (shootingScript == null)
        {
            Debug.LogError("Shooting script not found on any GameObject in the scene.");
        }
    }

    void Update()
    {
        if (shootingScript != null)
        {
            UpdateScoreText(shootingScript.points);
        }
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = "Punkty: " + score;
    }
}

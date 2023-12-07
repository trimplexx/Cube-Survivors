using UnityEngine;
using TMPro;

public class HealthScreen : MonoBehaviour
{
    public TextMeshProUGUI health; // Używamy TextMeshProUGUI
    int oldhealth = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Pobierz aktualne życie z obiektu Character_Male_1 i zaktualizuj tekst
        PlayerHealth health = FindObjectOfType<PlayerHealth>();
        if(oldhealth != health.currentHealth)
        {
            oldhealth = health.currentHealth;
            UpdateScoreText(health.currentHealth);
        }
        
    }

    public void UpdateScoreText(int score)
    {
        health.text = score.ToString();
    }
}

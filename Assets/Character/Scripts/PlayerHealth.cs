using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHealth; // Maksymalne zdrowie gracza
    private int currentHealth; // Aktualne zdrowie gracza
    private Animator animator; // Animator moba
    public bool isAlive = true;

    void Start()
    {
        currentHealth = maxHealth; // Ustaw aktualne zdrowie na maksymalne zdrowie na poczπtku gry
        animator = GetComponent<Animator>(); // Pobierz komponent Animator
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Zredukuj aktualne zdrowie o zadane obraøenia

        // Sprawdü, czy zdrowie gracza spad≥o do 0 lub poniøej
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        if (animator != null)
        {
            animator.Play("Death"); // Uruchom animacjÍ úmierci
        }
        Invoke("EndGame", 2f); // ZakoÒcz grÍ po 2 sekundach
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MenuScene"); // Za≥aduj scenÍ "GameOver"
    }
}
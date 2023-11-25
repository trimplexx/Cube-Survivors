using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject DamageTextPrefab;
    [SerializeField]
    private float health; // Zdrowie moba
    private Animator animator; // Animator moba
    public bool isDead = false; // Dodajemy now¹ zmienn¹ boolowsk¹
    private GameObject currentDamageText; // Aktualny tekst obra¿eñ
    private float damageTextOffset = 0.5f; // Przesuniêcie tekstu obra¿eñ
    [SerializeField]
    private float baseTextHeight;
    private float damageTextHeight; // Pocz¹tkowa wysokoœæ tekstu obra¿eñ
    int AddHeight = 0;

    private void Start()
    {
        animator = GetComponent<Animator>(); // Pobierz komponent Animator
        damageTextHeight = baseTextHeight;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null && !isDead) // Sprawdzamy, czy mob nie jest martwy
        {
            TakeDamage(bullet.damage);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage; // Zmniejsz zdrowie o wartoœæ obra¿eñ
        ShowDamageText();

        if (health <= 0 && !isDead) // Sprawdzamy, czy mob nie jest martwy
        {
            Die(); // Jeœli zdrowie <= 0, mob umiera
        }
    }

    void ShowDamageText()
    {
        Vector3 textPosition = new Vector3(transform.position.x, transform.position.y + damageTextHeight, transform.position.z); // Pozycja tekstu nad g³ow¹
        currentDamageText = Instantiate(DamageTextPrefab, textPosition, Quaternion.identity, transform);
        currentDamageText.transform.LookAt(Camera.main.transform); // Skieruj tekst w stronê kamery
        currentDamageText.transform.rotation = Quaternion.Euler(60, 0, 0); // Obróæ tekst, aby by³ równoleg³y do kamery
        currentDamageText.GetComponent<TextMeshPro>().text = health < 0 ? "0" : health.ToString(); // Ustaw tekst obra¿eñ
        currentDamageText.GetComponent<TextMeshPro>().color = Color.red; // Ustaw kolor na czerwony
        if (AddHeight == 3)
        {
            damageTextHeight = baseTextHeight;
            AddHeight = 0;
        }
        else
        {
            damageTextHeight += damageTextOffset;
            AddHeight++;
        }
        Destroy(currentDamageText, 1f); // Zniszcz tekst obra¿eñ po 1 sekundzie
    }

    private void Die()
    {
        isDead = true; // Ustawiamy isDead na true, gdy mob umiera
        if (animator != null)
        {
            animator.Play("Death"); // Uruchom animacjê œmierci
        }
        Invoke("DestroyObject", 1f); // Zniszcz obiekt po 2 sekundach
        SpawnerScript.currentEnemies--; // Dekrementuj liczbê wrogów

        Shooting shootingScript = FindObjectOfType<Shooting>();
        if (shootingScript != null)
        {
            shootingScript.pointCounter += 1; // Zaktualizuj licznik punktów
            if(shootingScript.pointCounter >= 5)
            {
                shootingScript.pointCounter = 0;
                shootingScript.points += 1;
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject); // Zniszcz obiekt
    }
}
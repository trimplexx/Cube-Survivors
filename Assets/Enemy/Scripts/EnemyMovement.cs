using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance; // Odleg�o��, na kt�rej przeciwnik przestaje si� porusza�
    [SerializeField] private float normalSpeed;
    [SerializeField] private float slowDuration = 5f;
    public ParticleSystem frostEffect;
    private ParticleSystem currentFrostEffect;
    private float slowTimer = 0f;
    private bool isSlowed = false;

    private Transform playerTransform;
    private Animator animator;
    private EnemyAttack enemyAttack;
    private EnemyHealth enemyHealth; // Dodajemy referencj� do skryptu EnemyHealth

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        // Pobierz komponent Animator z obiektu 
        animator = GetComponent<Animator>();
        // Pobierz skrypt EnemyAttack
        enemyAttack = GetComponent<EnemyAttack>();
        // Pobierz skrypt EnemyHealth
        enemyHealth = GetComponent<EnemyHealth>(); // Pobieramy komponent EnemyHealth
        normalSpeed = speed; //Przypisanie normalnej prędkości na początku
    }

    /*Funkcja sprawdzająca czy przeciwnik dostał pociskiem spowalniającym jego ruch*/
    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null && enemyHealth.shootingScript.isFrost)
        {
            ApplySlow();
        }
    }

    /*Metoda nakładajaca efekt spowolnienia*/
    private void ApplySlow()
    {
        currentFrostEffect = Instantiate(frostEffect, transform.position, Quaternion.identity, transform);
        currentFrostEffect.Play();
        speed = normalSpeed / 2f;
        isSlowed = true;
    }

    void Update()
    {
        /*Sprawdzanie czy jest nałożony efekt spowolnienia*/
        if (isSlowed)
        {
            slowTimer += Time.deltaTime; //Aktualizuj licznik czasu spowolnienia

            /*Sprawdź, czy czas spowolnienia minął*/
            if (slowTimer >= slowDuration)
            {
                Destroy(currentFrostEffect.gameObject); //Usuń efekt podpalenia
                speed = normalSpeed; //Przywróć normalną prędkość
                isSlowed = false;
                slowTimer = 0f;
            }
        }
        
        // Je�li wr�g jest martwy, nie kontroluj jego ruchu
        if (enemyHealth.isDead)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        if (playerTransform != null)
        {
            // Oblicz kierunek od wroga do gracza
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            // Oblicz odleg�o�� od wroga do gracza
            float distance = (playerTransform.position - transform.position).magnitude;

            // Je�li gracz jest poza zasi�giem, przeciwnik si� porusza
            if (distance > stopDistance)
            {
                // Aktualizuj pozycj� wroga, poruszaj�c go w kierunku gracza
                transform.position += direction * speed * Time.deltaTime;

                // Zwr�� posta� wroga w kierunku gracza
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, speed * Time.deltaTime);

                // Ustaw animacj� chodzenia
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
            }
            else // Je�li gracz jest w zasi�gu, przeciwnik staje i atakuje
            {
                animator.SetBool("isMoving", false);
                if (!animator.GetBool("isAttacking"))
                {
                    enemyAttack.CheckAttack();
                }
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
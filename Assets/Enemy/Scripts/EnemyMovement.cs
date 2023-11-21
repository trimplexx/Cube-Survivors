using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float stopDistance; // Odleg³oœæ, na której przeciwnik przestaje siê poruszaæ

    private Transform playerTransform;
    private Animator animator;
    private EnemyAttack enemyAttack;
    private EnemyHealth enemyHealth; // Dodajemy referencjê do skryptu EnemyHealth

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
    }

    void Update()
    {
        // Jeœli wróg jest martwy, nie kontroluj jego ruchu
        if (enemyHealth.isDead)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        if (playerTransform != null)
        {
            // Oblicz kierunek od wroga do gracza
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            // Oblicz odleg³oœæ od wroga do gracza
            float distance = (playerTransform.position - transform.position).magnitude;

            // Jeœli gracz jest poza zasiêgiem, przeciwnik siê porusza
            if (distance > stopDistance)
            {
                // Aktualizuj pozycjê wroga, poruszaj¹c go w kierunku gracza
                transform.position += direction * speed * Time.deltaTime;

                // Zwróæ postaæ wroga w kierunku gracza
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, speed * Time.deltaTime);

                // Ustaw animacjê chodzenia
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
            }
            else // Jeœli gracz jest w zasiêgu, przeciwnik staje i atakuje
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
using UnityEngine;
public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown; // Czas, po którym mo¿e nast¹piæ kolejny atak
    [SerializeField]
    private int attackDamage;
    private Animator animator;
    private bool isCoolingDown;
    private float nextAttackAllowed; // Czas, kiedy nastêpny atak jest dozwolony

    void Start()
    {
        // Pobierz komponent Animator z obiektu 
        animator = GetComponent<Animator>();
        isCoolingDown = false;
        nextAttackAllowed = 0f; // Ustaw wartoœæ pocz¹tkow¹ na 0f, aby umo¿liwiæ natychmiastowy atak
    }

    public void CheckAttack()
    {
        if (Time.time > nextAttackAllowed) // Jeœli czas jest wiêkszy ni¿ czas nastêpnego ataku
        {
            if (!isCoolingDown) // Jeœli nie jest w trakcie odnowienia
            {
                animator.Play("Attack"); 
                isCoolingDown = true; // Ustaw flagê odnowienia
                Invoke("ResetAttack", attackCooldown); // Wywo³aj metodê ResetAttack po okreœlonym czasie

                // Zadaj obra¿enia graczowi
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(attackDamage);
                    }
                }
            }
        }
    }

    void ResetAttack()
    {
        animator.SetBool("isAttacking", false); // Zresetuj animacjê ataku
        isCoolingDown = false; // Zresetuj flagê odnowienia
        nextAttackAllowed = Time.time + attackCooldown; // Ustaw czas nastêpnego ataku na bie¿¹cy czas plus czas odnowienia
    }
}
using UnityEngine;

public class GeneratorCapsule : MonoBehaviour
{
    public GameObject capsulePrefab;
    public float predkoscRuchu = 100f;
    public float spawnDistance = 1.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCapsule();
        }
    }


    void GenerateCapsule()
    {
        // Znajdź obiekt Player_Movement
        Player_Movement playerMovement = FindObjectOfType<Player_Movement>();

        if (playerMovement != null)
        {
            // Twórz kapsułę przed graczem (0.1 jednostki nad pozycją gracza)
            Vector3 capsuleSpawnPosition = playerMovement.transform.position + playerMovement.transform.forward * spawnDistance + new Vector3(0, 1.5f, 0);

            // Tworzenie nowego obiektu "Capsule"
            GameObject capsule = Instantiate(capsulePrefab, capsuleSpawnPosition, playerMovement.transform.rotation);

            // Ustaw prędkość kapsuły na podstawie kierunku, w którym patrzy gracz
            Rigidbody capsuleRigidbody = capsule.GetComponent<Rigidbody>();
            capsuleRigidbody.velocity = playerMovement.transform.forward * predkoscRuchu;

            // Wyłącz grawitację dla kapsuły
            capsuleRigidbody.useGravity = false;
        }
    }
}

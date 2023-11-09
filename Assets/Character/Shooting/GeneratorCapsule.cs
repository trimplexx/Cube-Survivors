using UnityEngine;

public class GeneratorCapsule : MonoBehaviour
{
    public GameObject capsulePrefab;
    public float predkoscRuchu = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateCapsule();
        }
    }

    void GenerateCapsule()
    {
        // Tworzenie nowego obiektu "Capsule"
        GameObject capsule = Instantiate(capsulePrefab, transform.position, transform.rotation);

        // Dodawanie komponentu Rigidbody do obiektu "Capsule"
        Rigidbody capsuleRigidbody = capsule.GetComponent<Rigidbody>();
        if (capsuleRigidbody == null)
        {
            capsuleRigidbody = capsule.AddComponent<Rigidbody>();
        }

        // Pobieranie kierunku z obiektu Player_Movement
        Player_Movement playerMovement = FindObjectOfType<Player_Movement>();
        if (playerMovement != null)
        {
            Vector3 kierunek = playerMovement.GetMoveDirection();

            // Nadawanie kierunku i prędkości obiektowi "Capsule"
            capsuleRigidbody.velocity = kierunek * predkoscRuchu;
        }
        else
        {
            Debug.LogError("Player_Movement script not found in the scene.");
        }
    }
}

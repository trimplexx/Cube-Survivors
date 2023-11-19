using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject SpikeBall;
    public GameObject SpikeBallv2;
    public float shootingSpeed;
    public float spawnDistance;
    public int level;
    public bool isFire;
    public bool isFrost;

    void Start()
    {
        // Uruchom korutynę
        StartCoroutine(GenerateCapsuleWithDelayCoroutine());
    }

    IEnumerator GenerateCapsuleWithDelayCoroutine()
    {
        while (true)
        {
            GenerateCapsuleWithDelay();
            yield return new WaitForSeconds(0.5f); // Opoznienie co pół sekundy
        }
    }

    public void GenerateCapsuleWithDelay()
    {
        // Znajdź obiekt Player_Movement
        Player_Movement playerMovement = FindObjectOfType<Player_Movement>();
        GameObject ball;

        if (playerMovement != null)
        {
            Vector3 capsuleSpawnPosition = playerMovement.transform.position +
                playerMovement.transform.forward * spawnDistance + new Vector3(0, 1.5f, 0);

            if(level <5)
            ball = Instantiate(SpikeBall, capsuleSpawnPosition, playerMovement.transform.rotation);
            else
            ball = Instantiate(SpikeBallv2, capsuleSpawnPosition, playerMovement.transform.rotation);

            ball.SetActive(true);

            if(isFire || isFrost)
            {
                Light lightComponent = ball.GetComponent<Light>();
                if (lightComponent == null)
                {
                    lightComponent = ball.AddComponent<Light>();
                }

                lightComponent.type = LightType.Point;
                lightComponent.intensity = 16f;

                if (isFire)
                    lightComponent.color = Color.red;
                if(isFrost)
                    lightComponent.color = Color.blue;
            }

            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.velocity = playerMovement.transform.forward * shootingSpeed;
            ballRigidbody.useGravity = false;
        }
    }
}

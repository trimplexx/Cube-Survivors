using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Tooltip("Prędkość poruszania się postaci")] public float speed;
    Animator animator;
    public GameObject dashEffectPrefab;
    [Tooltip("Czas trwania dashowania")] public float dashDuration;
    [Tooltip("Odległość pokonywana podczas dashowania")] public float dashDistance;
    [Tooltip("Czas odnowienia dasha po użyciu")] public float dashCooldown;
    private bool isDashing = false; //Flaga określająca czy obecnie trwa dash'owanie
    private float lastDashTime; //Czas ostatniego użycia dash'a

    public Vector3 currentPosition { get; set; }
    public Vector3 currentDirection { get; set; }

    void Start()
    {
        animator = GetComponent<Animator>();
        lastDashTime = -dashCooldown; //Możliwość natychmiastowego użycia dash'a po rozpoczęciu gry
    }

    void Update()
    {
        Vector3 moveDirection = GetMoveDirection(); 
        RotateTowardsMouseCursor();
        //Change animator state
        animator.SetBool("IsRunning", moveDirection != Vector3.zero); 

        if (Input.GetKeyDown(KeyCode.LeftShift)) //Wywołanie dash'a po kliknięciu lewego shifta
        {
            Dash();
        }

        //Space.World is used so the character moves in the world, and not based on it's rotation
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World); 
        currentPosition = transform.position;
        currentDirection = transform.forward;
    }

    void RotateTowardsMouseCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Keep the same y-position to avoid tilting

            transform.LookAt(targetPosition);
        }
    }

    /*Funkcja sprawdzająca czy można dashować*/
    void Dash()
    {
        if (!isDashing && Time.time - lastDashTime >= dashCooldown)
        {
            StartCoroutine(PerformDash());
        }
    }

    /*Funkcja wykonująca dashowanie (wykonuje się asynchronicznie, 
      co pozwala reszcie gry na wykonywanie swojego działania)*/
    IEnumerator PerformDash()
    {
        isDashing = true;
        float elapsedTime = 0f;
        Vector3 dashDirection = currentDirection; //Kierunek dash'owania

        while (elapsedTime < dashDuration) //Pętla wykonująca dash'a
        {
            transform.Translate(dashDirection * dashDistance, Space.World);
            currentPosition = transform.position;
            /*Tworzenie efektu wizualnego dash'a (prefab postaci)*/
            GameObject dashEffect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
            Destroy(dashEffect, 0.1f); //Usuwanie obiektów

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Zniszczenie efektu po krótkim czasie (np. 0.1s)

        isDashing = false;
        lastDashTime = Time.time; //Zapisz czas ostatniego użycia dash'a
    }

    public Vector3 GetMoveDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        return moveDirection;
    }

    public (Vector3 position, Vector3 direction) GetPlayerPositionAndDirection()
    {
        return (transform.position, transform.forward);
    }
}

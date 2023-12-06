using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Second_Movement : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private GameObject character;

    private Transform Target;
    [Tooltip("Prêdkoœæ poruszania siê postaci")] public float speed;
    [Tooltip("Prêdkoœæ obrotu postaci")] public float rotationSpeed;

    public AudioSource dashSound;
    public GameObject dashEffectPrefab;
    [Tooltip("Czas trwania dashowania")] public float dashDuration;
    [Tooltip("Odleg³oœæ pokonywana podczas dashowania")] public float dashDistance;
    [Tooltip("Czas odnowienia dasha po u¿yciu")] public float dashCooldown;
    private bool isDashing = false; //Flaga okreœlaj¹ca czy obecnie trwa dash'owanie
    private float lastDashTime; //Czas ostatniego u¿ycia dash'a
    public bool CanDash { get; private set; } = false;

    float radius_limit = 0.0f;
    public Vector3 currentPosition { get; set; }
    public Vector3 currentDirection { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        radius_limit = Camera_Follow_Player.Instance.cameraHeight * 0.8f;
        lastDashTime = -dashCooldown; //Mo¿liwoœæ natychmiastowego u¿ycia dash'a po rozpoczêciu gry
        Target = character.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCharacter();
        Vector3 moveDirection = GetMoveDirection();
        // Change animator state
        animator.SetBool("IsRunning", moveDirection != Vector3.zero);

        if (CanDash && Input.GetKeyDown(KeyCode.KeypadEnter)) //Wywo³anie dash'a po klikniêciu lewego shifta
        {
            Dash();
        }


        // Space.World is used so the character moves in the world, and not based on its rotation
        // Rotate the character around the Y-axis
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // This code keeps the second character within a radius of the main character
        if (Target != null)
        {
            Vector3 offset = Target.position - transform.position;
            float currentDistance = offset.magnitude;

            //If it's beyond the limit, then move inside it
            if (currentDistance > radius_limit)
            {
                Vector3 direction = offset.normalized;
                float moveAmount = currentDistance - radius_limit;
                transform.position += direction * moveAmount;
            }
        }

        currentPosition = transform.position;
        currentDirection = transform.forward;

    }

    void RotateCharacter()
    {
        float rotationInput = 0.0f;

        // Player 2 (Numpad 9 and 8 for rotation)
        rotationInput = Input.GetKey(KeyCode.Keypad9) ? 1.0f : (Input.GetKey(KeyCode.Keypad8) ? -1.0f : 0.0f);
        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;

        Quaternion currentRotation = transform.rotation;
        Quaternion desiredRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y + rotationAmount, 0);

        transform.rotation = desiredRotation;
    }


    public Vector3 GetMoveDirection()
    {
        float horizontalInput = Input.GetKey(KeyCode.RightArrow) ? 1.0f : (Input.GetKey(KeyCode.LeftArrow) ? -1.0f : 0.0f);
        float verticalInput = Input.GetKey(KeyCode.UpArrow) ? 1.0f : (Input.GetKey(KeyCode.DownArrow) ? -1.0f : 0.0f);

        // Adjust movement based on arrow keys
        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        return moveDirection;
    }

    /*Funkcja sprawdzaj¹ca czy mo¿na dashowaæ*/
    void Dash()
    {
        if (!isDashing && Time.time - lastDashTime >= dashCooldown)
        {
            dashSound.Play();
            StartCoroutine(PerformDash());
        }
    }

    /*Funkcja wykonuj¹ca dashowanie (wykonuje siê asynchronicznie, 
      co pozwala reszcie gry na wykonywanie swojego dzia³ania)*/
    IEnumerator PerformDash()
    {
        isDashing = true;
        float elapsedTime = 0f;
        Vector3 dashDirection = GetMoveDirection(); //Kierunek dash'owania

        while (elapsedTime < dashDuration) //Pêtla wykonuj¹ca dash'a
        {
            transform.Translate(dashDirection * dashDistance * Time.deltaTime*10, Space.World);
            currentPosition = transform.position;
            /*Tworzenie efektu wizualnego dash'a (prefab postaci)*/
            GameObject dashEffect = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
            Destroy(dashEffect, 0.1f); //Usuwanie obiektów

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Zniszczenie efektu po krótkim czasie (np. 0.1s)

        isDashing = false;
        lastDashTime = Time.time; //Zapisz czas ostatniego u¿ycia dash'a
    }

    public void SetDashAbility(bool enabled)
    {
        CanDash = enabled;
    }

    public (Vector3 position, Vector3 direction) GetPlayerPositionAndDirection()
    {
        return (transform.position, transform.forward);
    }

}

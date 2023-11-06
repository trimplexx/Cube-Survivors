using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float horizontalinput;
    public float verticalinput;
    public float speed;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        speed = 3.0f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read values from keyboard
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Create a direction vector based on the input
        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        // Rotate towards the mouse cursor
        RotateTowardsMouseCursor();

        // Set the "IsWalking" parameter in the Animator
        animator.SetBool("IsRunning", moveDirection != Vector3.zero);

        // Move the object in the direction of the input
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World); //Space.World is used so the character moves in the world, and not based on it's rotation
    }

    void RotateTowardsMouseCursor()
    {
        // Create a ray from the camera to the mouse cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Perform a raycast to find the point on the ground
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Keep the same y-position to avoid tilting

            // Rotate the character to look at the target position
            transform.LookAt(targetPosition);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed;
    Animator animator;

    void Start()
    {
        speed = 3.0f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 moveDirection = GetMoveDirection(); 
        RotateTowardsMouseCursor();
        animator.SetBool("IsRunning", moveDirection != Vector3.zero); //Change animator state
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World); //Space.World is used so the character moves in the world, and not based on it's rotation
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

    public Vector3 GetMoveDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        return moveDirection;
    }
}

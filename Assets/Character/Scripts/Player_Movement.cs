using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed;
    Animator animator;
    public Vector3 currentPosition { get; set; }
    public Vector3 currentDirection { get; set; }

    void Start()
    {
        speed = 15.0f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 moveDirection = GetMoveDirection(); 
        RotateTowardsMouseCursor();
        //Change animator state
        animator.SetBool("IsRunning", moveDirection != Vector3.zero); 

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

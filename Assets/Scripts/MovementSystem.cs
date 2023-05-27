using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementSystem : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        RotateCharacter(horizontalInput);
        MoveCharacter(verticalInput);

        ApplyGravity();
    }

    private void RotateCharacter(float horizontalInput)
    {
        if (Mathf.Approximately(horizontalInput, 0f))
            return;

        float rotationAmount = horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * rotationAmount);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void MoveCharacter(float verticalInput)
    {
        if (Mathf.Approximately(verticalInput, 0f))
            return;

        Vector3 movement = transform.forward * verticalInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void ApplyGravity()
    {
        isGrounded = CheckGrounded();

        if (!isGrounded)
        {
            Vector3 gravity = Physics.gravity;
            rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    private bool CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            return true;
        }

        return false;
    }


}

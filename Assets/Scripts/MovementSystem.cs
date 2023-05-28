using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementSystem : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;

    private float currentSpeed = 5f;
    private float regularSpeed = 4f;
    private float sprintSpeed = 10f;
    private float rotationSpeed = 300f;
    private float groundCheckDistance = 0.6f;


    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private bool isAnimatorOutside;

    private Rigidbody rb;

    private bool isGrounded;

    Animator animator;
    float idleTimer, idleTimerMax = 12f;
    float attackTimer;

    bool stopInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        if (isAnimatorOutside)
        {
            animator = GetComponentInChildren<Animator>();
        }
        else
        {
            animator = GetComponent<Animator>();
        }
        
    }

    private void Update() //the code i wrote is absolute dogshit, i was watching gnome videos instead of working
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        animator.SetFloat("MoveInput", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); //execute anim if player moves (WASD)

        if (Mathf.Abs(horizontalInput)+Mathf.Abs(verticalInput)<0.1f) //idle logic
        {
            idleTimer += Time.deltaTime;
            if (idleTimer>idleTimerMax)
            {
                idleTimer = 0f;
            }
            animator.SetFloat("IdleTime", idleTimer);

        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            print("Sprinting");
            currentSpeed = sprintSpeed;
            //sprinting
            animator.SetBool("IsRunning", true);

        }
        else
        {
            currentSpeed = regularSpeed;
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Jumping");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //jumping
            animator.SetTrigger("IsJumping");
        }
        else
        {
            
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            print("attacking");
            attackTimer += Time.deltaTime;
            animator.SetFloat("IsAttacking", attackTimer);
            stopInput = true;
            //attacking
        }
        else attackTimer = 0f; animator.SetFloat("IsAttacking", attackTimer); stopInput = false;



        if (Input.GetKeyDown(KeyCode.X))
        {
            print("died");
            animator.SetBool("IsDead", true);
            print("KILLING MOVEMENT SYSTEM AS PLAYER HAS DIED!");
            Destroy(this);

            //attacking
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetTrigger("IsGetHit");
            stopInput = true;
            //gethit
        }
        else stopInput = false;

    }

    private void FixedUpdate()
    {
        if (!stopInput)
        {
            RotateCharacter(horizontalInput);
            MoveCharacter(verticalInput);
        }
        
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

        Vector3 movement = transform.forward * verticalInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

}

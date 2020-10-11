using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10;
    CharacterController playerController;
    Animator animator;
    public Transform cam;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    // keep track of values passed into the player animator blend tree
    float getXInput, getYInput;

    void Awake()
    {
        playerController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    float startTime;
    [SerializeField] float totalDuration = 1.5f;
    void Start()
    {
        startTime = Time.time;
    }

    void PlayStep()
    {
        // print("moving");
    }

    // debugging the ground check sphere
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }

    bool isJumping = false;

    void Update()
    {
        float fractionTime = (Time.time - startTime) / totalDuration;

        // jump
        // only pays attention to the groundMask layer
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            animator.SetTrigger("Jump");
            animator.SetBool("IsJumping", isJumping);
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

            animator.SetFloat("VelX", Mathf.SmoothStep(getXInput, 0f, fractionTime));
            animator.SetFloat("VelZ", Mathf.SmoothStep(getYInput, 0f, fractionTime));
        }
        else if (isGrounded)
        {
            isJumping = false;
            animator.SetBool("IsJumping", isJumping);
            // animator.ResetTrigger("Jump");
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        // walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {
            // this check avoids running while in the air
            if (!isJumping)
            {
                // play the right animations if the player is moving
                animator.SetBool("IsRunning", true);

                getXInput = horizontal;
                getYInput = vertical;

                animator.SetFloat("VelX", getXInput);
                animator.SetFloat("VelZ", getYInput);
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsRunning", false);




            // gradually set back to 0f
            animator.SetFloat("VelX", Mathf.SmoothStep(getXInput, 0f, fractionTime));
            animator.SetFloat("VelZ", Mathf.SmoothStep(getYInput, 0f, fractionTime));
            // if (getXInput - 0f <= Mathf.Epsilon)
            //     animator.SetFloat("VelX", 0f);
            // else
            //     getXInput -= Time.deltaTime;
            // if (getYInput - 0f <= Mathf.Epsilon)
            //     animator.SetFloat("VelZ", 0f);
            // else
            //     getYInput -= Time.deltaTime;

        }
    }
}

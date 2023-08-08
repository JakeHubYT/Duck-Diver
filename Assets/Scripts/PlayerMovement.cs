using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isDashing = false;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    [SerializeField] private float flapForce = 10f;
    public float glideForwardSpeed = 10f;
    public float glideFallMultiplier = 0.5f;


    [Header("Ground Check Settings")]
    public float groundCheckRadius = 0.2f;
    public Vector3 groundCheckOffset = new Vector3(0f, -0.5f, 0f);

    [Header("Jump Settings")]
    [SerializeField] private float maxJumpDuration = 1f;
    private bool isJumping = false;
    private float jumpStartTime;
    [SerializeField] private float maxYVelocity = 30f; // Add this variable for y-velocity clamp

    public bool isGrounded = true;
    public bool isSwimming = false;
    public bool isGliding = false;
    public bool isFacingRight = true;
    public Animator anim;

    private Rigidbody rb;
    private float originalDrag;
    private Vector3 originalVelocity;

    private float targetRotation = 0f;
    private float rotationSpeed = 10f;
    public float waterHeightY = 1;

    public bool isMoving = false;

    [Header("Attack Settings")]
    public List<Attack> attacks = new List<Attack>();
    private bool isAttacking = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Attack();

        if (isGliding)
        {
            GlideMove();
        }
        else
        {
            Move(horizontalInput);
        }

        // Perform ground check using Physics.CheckSphere
        isGrounded = Physics.CheckSphere(transform.position + groundCheckOffset, groundCheckRadius, LayerMask.GetMask("Ground"));

        if (transform.position.y <= waterHeightY) { isSwimming = true; }
        else if (transform.position.y > waterHeightY) { isSwimming = false; }

        //&& isGrounded
        // Check if the player presses the spacebar and is currently grounded to perform a jump.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpStartTime = Time.time;
            Jump();
        }

        // Allow gliding only when not grounded and the player is holding down the Shift key.
        if (Input.GetKey(KeyCode.LeftShift) && !isGrounded)
        {
            Glide();
        }
        else
        {
            // Reset drag and gliding state when Shift key is released or when grounded.
            if (isGliding)
            {
                ResetGlide();
            }
        }

    
    

        if (horizontalInput != 0)
        {
            isMoving = true;
            anim.SetFloat("WalkAmount", 0);

        }
        else if (horizontalInput == 0)
        {
            isMoving = false;
            anim.SetFloat("WalkAmount", 1);
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isGliding", isGliding);
        anim.SetBool("isSwimming", isSwimming);
        anim.SetBool("isMoving", isMoving);

        if (isSwimming)
        {
            transform.position = new Vector3(transform.position.x, waterHeightY, transform.position.z);
        }
    }


    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isDashing)
        {
            foreach (Attack attack in attacks)
            {
                if ((attack.isGrounded && !isGrounded) || (attack.isMoving && !isMoving))
                {
                    continue;
                }

                // If the attack is not grounded or not moving, execute it.
                StartCoroutine(AttackCoroutine(attack));
                attack.attackCollider.SetActive(true);
         
                break; // Break the loop after finding the first valid attack.
            }
        }
    }

 

    void FixedUpdate()
    {
        if (isJumping)
        {
            float jumpDuration = Time.time - jumpStartTime;

            // Adjust the flapForce based on jumpDuration to control the jump height.
            float currentFlapForce = flapForce * (1f - jumpDuration * 2f);

            // Clamp the flapForce to ensure the player doesn't jump too high or too low.
            currentFlapForce = Mathf.Clamp(currentFlapForce, 0f, flapForce);

            // Apply the jump force to the Rigidbody in the upward direction.
            rb.AddForce(Vector3.up * currentFlapForce, ForceMode.Force);

            if (!Input.GetKey(KeyCode.Space) || jumpDuration >= 0.5f)
            {
                isJumping = false;
            }
        }

        // Cap the y-velocity at 30.
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxYVelocity, maxYVelocity), rb.velocity.z);
    }


    IEnumerator Dash(float dashDistance, float dashDuration)
    {
        // Mark the player as dashing and disable user input during the dash.
        isDashing = true;

        // Store the player's current position.
        Vector3 startPos = transform.position;

        // Calculate the dash direction based on the facing direction.
        Vector3 dashDirection = isFacingRight ? Vector3.right : Vector3.left;

        // Calculate the dash end position.
        Vector3 endPos = startPos + dashDirection * dashDistance;

        // Reset the rigidbody velocity to zero before dashing.
        rb.velocity = Vector3.zero;

        // Calculate the force required to reach the dash end position in the specified duration.
        float dashForce = (dashDistance / dashDuration) * rb.mass;

        // Apply the dash force in the dash direction.
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

        // Wait for the specified dash duration.
        yield return new WaitForSeconds(dashDuration);

        // Mark the dash as finished and enable user input again.
        isDashing = false;
    }

    void Move(float horizontalInput)
    {
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);

        if (!isDashing)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        
     
          

        if (moveDirection != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float step = rotationSpeed * Time.deltaTime;
            float newRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, step);
            transform.eulerAngles = new Vector3(0f, newRotation, 0f);

            // Update the facing direction based on the horizontal input.
            isFacingRight = horizontalInput >= 0;
        }
      
    }

    void GlideMove()
    {
        // Move forward while gliding based on the facing direction.
        Vector3 glideMoveDirection = isFacingRight ? Vector3.right : Vector3.left;
        transform.position += glideMoveDirection * glideForwardSpeed * Time.deltaTime;

        // Reduce the vertical velocity while gliding to make the player fall slower.
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (glideFallMultiplier - 1) * Time.deltaTime;
        }

        // Snap the player's rotation to the glide direction.
        float glideAngle = isFacingRight ? 90f : -90f;
        transform.rotation = Quaternion.Euler(0f, glideAngle, 0f);
    }

    void Jump()
    {
        jumpStartTime = Time.time;
        isJumping = true;
        anim.SetTrigger("Flap");
    }

    void Glide()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (glideFallMultiplier - 1) * Time.deltaTime;
        }

        rb.AddForce(transform.forward * glideForwardSpeed * Time.deltaTime, ForceMode.Force);

        isGliding = true;
        rb.drag = glideForwardSpeed;
    }

    void ResetGlide()
    {
        rb.drag = originalDrag;
        isGliding = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.drag = originalDrag;
            isGliding = false;
        }
    }

       void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    IEnumerator AttackCoroutine(Attack attack)
    {
        // Mark the player as attacking and disable user input during the attack.
        isAttacking = true;

        // Trigger the attack animation based on the animation trigger from the Attack object.
        anim.SetTrigger(attack.animationTrigger);

        // Perform the dash associated with the attack using the dashDistance and dashDuration values.
        StartCoroutine(Dash(attack.dashDistance, attack.dashDuration));

        // Wait until the attack animation finishes before re-enabling user input.
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Mark the attack as finished and enable user input again.
        isAttacking = false;

        // Deactivate the attack collider after the attack animation finishes.
        attack.attackCollider.SetActive(false);

        // Countdown the attack duration and then set the attack collider back to false.
        yield return new WaitForSeconds(attack.attackDuration);
        attack.attackCollider.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the ground check sphere with Gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }

}


[System.Serializable]
public class Attack
{
    public string animationTrigger;
    public float dashDistance;
    public float dashDuration;
    public float attackDuration = .5f;
    public bool isGrounded;
    public bool isMoving;
    public GameObject attackCollider;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewControl : MonoBehaviour
{
   
    private AudioSource SoundEffect;
    public AudioClip RunSound;
    public AudioClip WalkSound;
    public AudioClip WhistleSound;
    public AudioClip JumpSound;
    public float moveSpeed = 5f; // Character movement speed
   private float turnSpeed = 15f; // Character rotation speed
    public float jumpForce = 5f; // Jump force applied to the character
    public float sprintMultiplier = 2f; // Speed multiplier when sprinting
    private Animator anim;
    private CharacterController controller;
    private Camera mainCamera;
    private Vector3 velocity; // Character's current velocity
    private bool isGrounded=true;
    private bool isJumping = false;// Flag to check if the character is grounded


    private void Start()
    {
        SoundEffect = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the character is grounded
        isGrounded = controller.isGrounded;

        // Get input for movement and rotation
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Mouse X");

        // Calculate movement direction relative to the camera's rotation
        Vector3 movement = moveVertical * mainCamera.transform.forward + moveHorizontal * mainCamera.transform.right;
        movement.y = 0f;

        // Normalize the movement vector and multiply it by the move speed
        movement = movement.normalized * moveSpeed;

        // Apply sprinting multiplier if Left Shift is pressed
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning)
        {
            movement *= sprintMultiplier;
        }

        // Apply movement to the CharacterController
        controller.SimpleMove(movement);

        // Rotate the character based on mouse input
        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);

        // Check for jumping input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump");
            velocity.y += Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            StopOtherSoundWhenJumping();
            // Stop other sound effects before playing the JumpSound
            SoundEffect.PlayOneShot(JumpSound);
        }

        // Apply gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;

        // Apply vertical velocity to the CharacterController
        controller.Move(velocity * Time.deltaTime);
        

        // Update animator parameters
        UpdateAnimatorParameters(moveHorizontal, moveVertical, isRunning);
    }


private void UpdateAnimatorParameters(float moveHorizontal, float moveVertical, bool isRunning)
    {
        bool isMoving = moveHorizontal != 0f || moveVertical != 0f;

        if (isMoving)
        {
            if (isRunning)
            {
                anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
                if (!SoundEffect.isPlaying)
                {
                    SoundEffect.clip = RunSound;
                    SoundEffect.PlayOneShot(RunSound);
                }
            }
            else
            {
                anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
                if (!SoundEffect.isPlaying)
                {
                    SoundEffect.clip = WalkSound;
                    SoundEffect.PlayOneShot(WalkSound);
                }
            }
        }
        else
        {
            anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
            if (!SoundEffect.isPlaying)
            {
                SoundEffect.clip = WhistleSound;
                SoundEffect.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0f), turnSpeed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the character is grounded
        if (controller.isGrounded)
        {
            isGrounded = true;
          velocity.y = 0f;
        }
    }


    private void StopOtherSoundWhenJumping()
    {
        // Stop any other sound effects when jumping
        if (isJumping)
        {
            if (SoundEffect.isPlaying)
            {
                SoundEffect.Stop();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public bool sprinting = false;
    public bool crouching = false;
    public bool lerpCrouch = false;
    public float crouchTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
     


    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;// the isGrounded variable is updated every frame to reflect whether the player is currently on the ground
        // controller.isGrounded is a property of the CharacterController component that checks whether the player is touching the ground.
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if(p> 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0) { playerVelocity.y = -2f;} // when the player is on the ground, the vertical velocity is reset to a small negative value -2f
        controller.Move(playerVelocity * Time.deltaTime); // after applying gravity, this line moves the player vertically based on the value of playerVelocity.y 
    }
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;

    }

    public void Sprint() // toggle between sprinting speed and walking speed
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8;
        }
        else
        {
            speed = 5;
        }
    }

    public void Jump()
    {
        if (isGrounded) // can only jump when the player is on the ground
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}

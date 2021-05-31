using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public Transform wallrunCheckLeft;
    public Transform wallrunCheckRight;
    public Transform wallrunCheckFront;
    public Transform wallrunCheckBack;
    public float groundDistance = 0.2f;
    public float wallrunDistance = 0.2f;
    public LayerMask groundMask;
    public LayerMask wallrunMask;

    Vector3 velocity;
    bool isGrounded;
    bool isWallrunableLeft;
    bool isWallrunableRight;
    bool isWallrunableFront;
    bool isWallrunableBack;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isWallrunableLeft = Physics.CheckSphere(wallrunCheckLeft.position, wallrunDistance, wallrunMask);
        isWallrunableRight = Physics.CheckSphere(wallrunCheckRight.position, wallrunDistance, wallrunMask);
        isWallrunableFront = Physics.CheckSphere(wallrunCheckFront.position, wallrunDistance, wallrunMask);
        isWallrunableBack = Physics.CheckSphere(wallrunCheckBack.position, wallrunDistance, wallrunMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack)
        {
            if (velocity.y >= -0.5f)
            {
                velocity.y -= 0.1f;
            }
            else if (velocity.y <= -0.5f)
            {
                velocity.y += 0.1f;
            }
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButton("Jump") && (isGrounded || (isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack)))
        {
            if (isGrounded)
            {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else if (isWallrunableLeft)
            {
                Debug.Log("Jumping off Wallrun LEFT");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                velocity += transform.forward * speed;
            }
            else if (isWallrunableRight)
            {
                Debug.Log("Jumping off Wallrun RIGHT");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                velocity += transform.forward * speed;
            }
            else if (isWallrunableFront)
            {
                Debug.Log("Jumping off Wallrun FRONT");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                velocity += transform.forward * speed;
            }
            else if (isWallrunableBack)
            {
                Debug.Log("Jumping off Wallrun BACK");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                velocity += transform.forward * speed;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        if (velocity.x >= 0)
        {
            velocity.x -= 0.1f;
        }
        if (velocity.x <= 0)
        {
            velocity.x += 0.1f;
        }
        if (velocity.z >= 0)
        {
            velocity.z -= 0.1f;
        }
        if (velocity.z <= 0)
        {
            velocity.z += 0.1f;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}

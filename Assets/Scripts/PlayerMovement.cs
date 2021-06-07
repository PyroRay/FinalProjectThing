using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject player;

    public float speed = 12f;
    public float sprintSpeed = 16f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float wallJumpHeight = 1.5f;
    public float doubleJumpHeight = 3f;
    public float wallJumpSpeed = 1f;
    public float wallJumpOffSpeed = 1f;
    public float wallStickSpeed = 12f;
    public float wallUpDownSpeed = 0.1f;
    public float airControlMultiplier = 0.5f;

    public float wallOffTime = 0f;
    public float wallOffOffset = 11f;
    public float wallCameraTilt = 30f;

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
    bool hasDoubleJump;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Time.time >= wallOffTime)
        {
            isWallrunableLeft = Physics.CheckSphere(wallrunCheckLeft.position, wallrunDistance, wallrunMask);
            isWallrunableRight = Physics.CheckSphere(wallrunCheckRight.position, wallrunDistance, wallrunMask);
            isWallrunableFront = Physics.CheckSphere(wallrunCheckFront.position, wallrunDistance, wallrunMask);
            isWallrunableBack = Physics.CheckSphere(wallrunCheckBack.position, wallrunDistance, wallrunMask);
        }


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            hasDoubleJump = true;
        }

        if (isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack)
        {
            if (velocity.y >= -0.5f)
            {
                velocity.y -= wallUpDownSpeed;
            }
            else if (velocity.y <= -0.5f)
            {
                velocity.y += wallUpDownSpeed;
            }

            hasDoubleJump = true;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isGrounded || isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack)
        {
            //controller.Move(move * speed * Time.deltaTime);

            if (Input.GetKey("left shift"))
            {
                velocity.x = move.x * sprintSpeed;
                //velocity.y = move.y * speed;
                velocity.z = move.z * sprintSpeed;
                Debug.Log("Sprint Pressed");
            }
            else
            {
                velocity.x = move.x * speed;
                //velocity.y = move.y * speed;
                velocity.z = move.z * speed;
            }

        }
        else
        {
            velocity.x += move.x * speed * airControlMultiplier;
            //velocity.y += move.y * speed * airControlMultiplier;
            velocity.z += move.z * speed * airControlMultiplier;
            //Debug.Log("NotOnGround");
        }

        if(Input.GetButtonDown("Jump") && (isGrounded || (isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack) || hasDoubleJump))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                //Debug.Log(velocity.y);
            }
            else if (isWallrunableLeft)
            {
                Debug.Log("Jumping off Wallrun LEFT");
                velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);
                velocity += transform.forward * wallJumpSpeed;
                velocity += transform.right * wallJumpOffSpeed;
                wallOffTime = Time.time + wallOffOffset;
                isWallrunableLeft = false;
            }
            else if (isWallrunableRight)
            {
                Debug.Log("Jumping off Wallrun RIGHT");
                velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);
                velocity += transform.forward * wallJumpSpeed;
                velocity += transform.right * -wallJumpOffSpeed;
                wallOffTime = Time.time + wallOffOffset;
                isWallrunableRight = false;
            }
            else if (isWallrunableFront)
            {
                Debug.Log("Jumping off Wallrun FRONT");
                velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);
                velocity += transform.forward * -wallJumpOffSpeed;
                wallOffTime = Time.time + wallOffOffset;
                isWallrunableFront = false;
            }
            else if (isWallrunableBack)
            {
                Debug.Log("Jumping off Wallrun BACK");
                velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);
                velocity += transform.forward * wallJumpOffSpeed;
                wallOffTime = Time.time + wallOffOffset;
                isWallrunableBack = false;
            }
            else if (hasDoubleJump && !(isGrounded || isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack))
            {
                velocity.y = Mathf.Sqrt(doubleJumpHeight * -2f * gravity);
                hasDoubleJump = false;
                Debug.Log("Double Jumped");
            }
        }

        if (isWallrunableLeft && !isGrounded)
        {
            player.transform.localRotation = Quaternion.Euler(player.transform.localRotation.x, player.transform.localRotation.y, -wallCameraTilt);
            velocity += transform.right * -wallStickSpeed;

        }
        else if (isWallrunableRight && !isGrounded)
        {
            player.transform.localRotation = Quaternion.Euler(player.transform.localRotation.x, player.transform.localRotation.y, wallCameraTilt);
            velocity += transform.right * wallStickSpeed;
        }
        else if ((!isWallrunableLeft && !isWallrunableRight) || isGrounded)
        { 
            player.transform.localRotation = Quaternion.Euler(player.transform.localRotation.x, player.transform.localRotation.y, 0);
        }


        velocity.y += gravity * Time.deltaTime;
        //if (velocity.x >= 0)
        //{
        //    velocity.x -= 0.1f;
        //}
        //if (velocity.x <= 0)
        //{
        //    velocity.x += 0.1f;
        //}
        //if (velocity.z >= 0)
        //{
        //    velocity.z -= 0.1f;
        //}
        //if (velocity.z <= 0)
        //{
        //    velocity.z += 0.1f;
        //}

        //Debug.Log(velocity);
        controller.Move(velocity * Time.deltaTime);
    }
}

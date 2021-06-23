using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject player;

    public float speedChangeable = 12f;
    public float speed = 12f;
    public float sprintSpeedMultiplier = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float wallJumpHeight = 1.5f;
    public float doubleJumpHeight = 3f;
    public float wallJumpSpeed = 1f;
    public float wallJumpOffSpeed = 1f;
    public float wallStickSpeed = 12f;
    public float wallRunSpeedMult = 3f;
    public float wallUpDownSpeed = 0.1f;
    public float airControlMultiplier = 0.5f;

    public float wallOffTime = 0f;
    public float wallOffOffset = 11f;
    public float wallCameraTilt = 30f;

    public float wallOffWASDTime = 0f;
    public float wallOffWASDOffset = 0.5f;

    public float wallStickTime = 0f;
    public float wallStickOffset = 2f;

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
    bool isWallRunning;

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



        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;


        if (isGrounded || isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack)
        {
            //controller.Move(move * speed * Time.deltaTime);

            if (Input.GetKey("left shift") && Input.GetKey("w") && !isWallRunning)
            {
                velocity.x = move.x * speed * sprintSpeedMultiplier;
                //velocity.y = move.y * speed;
                velocity.z = move.z * speed * sprintSpeedMultiplier;
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
                wallRun("left");
            }
            else if (isWallrunableRight)
            {
                Debug.Log("Jumping off Wallrun RIGHT");
                wallRun("right");
            }
            else if (isWallrunableFront)
            {
                Debug.Log("Jumping off Wallrun FRONT");
                wallRun("forward");
            }
            else if (isWallrunableBack)
            {
                Debug.Log("Jumping off Wallrun BACK");
                wallRun("back");
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
            player.transform.localRotation = Quaternion.Lerp(player.transform.localRotation, Quaternion.Euler(player.transform.localRotation.x, player.transform.localRotation.y, -wallCameraTilt), 10f * Time.deltaTime) ;
            velocity += transform.right * -wallStickSpeed;
            speed = speedChangeable * wallRunSpeedMult;
            isWallRunning = true;
        }
        else if (isWallrunableRight && !isGrounded)
        {
            player.transform.localRotation = Quaternion.Lerp(player.transform.localRotation, Quaternion.Euler(player.transform.localRotation.x, player.transform.localRotation.y, wallCameraTilt), 10f * Time.deltaTime);
            velocity += transform.right * wallStickSpeed;
            speed = speedChangeable * wallRunSpeedMult;
            isWallRunning = true;
        }
        else if ((!isWallrunableLeft && !isWallrunableRight) || isGrounded)
        { 
            player.transform.localRotation = Quaternion.Lerp(player.transform.localRotation, Quaternion.Euler(player.transform.localRotation.x, player.transform.localRotation.y, 0), 10f * Time.deltaTime);
            speed = speedChangeable;
            isWallRunning = false;
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void wallRun(string direction)
    {
        Debug.Log("Jumping off");
        velocity = new Vector3(0, 0, 0) + transform.forward * speedChangeable * wallRunSpeedMult;
        velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);

        if (direction == "forward")
        {
            velocity = transform.forward * -wallJumpOffSpeed * 2;
            velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);
            wallOffTime = Time.time + wallOffOffset;
            isWallrunableFront = false;
        }
        else if (direction == "back")
        {
            velocity = transform.forward * wallJumpOffSpeed * 2;
            velocity.y = Mathf.Sqrt(wallJumpHeight * -2f * gravity);
            wallOffTime = Time.time + wallOffOffset;
            isWallrunableBack = false;
        }
        else if (direction == "left")
        {
            velocity += transform.forward * wallJumpSpeed;
            velocity += transform.right * wallJumpOffSpeed;
            wallOffTime = Time.time + wallOffOffset;
            isWallrunableLeft = false;
        }
        else if (direction == "right")
        {
            velocity += transform.forward * wallJumpSpeed;
            velocity += transform.right * -wallJumpOffSpeed;
            wallOffTime = Time.time + wallOffOffset;
            isWallrunableRight = false;
        }

        wallOffTime = Time.time + wallOffOffset;
        isWallrunableLeft = false;
    }

    private void FixedUpdate()
    {
        if (isWallrunableLeft || isWallrunableRight || isWallrunableFront || isWallrunableBack)
        {

            if (wallStickTime >= Time.time)
            {
                if (velocity.y >= -0.5f)
                {
                    velocity.y -= wallUpDownSpeed;
                }
                else if (velocity.y <= -0.5f)
                {
                    velocity.y += wallUpDownSpeed;
                }
            }
            else if (wallStickTime <= Time.time - 2f)
            {
                wallStickTime = Time.time + wallStickOffset;
            }

            hasDoubleJump = true;
        }
        else
        {
            wallStickTime = 0;
        }
    }

}

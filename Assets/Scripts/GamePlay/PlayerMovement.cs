using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    // Start is called before the first frame update
    public float verticalInput;
    public float movementSpeed;
    private float speed = 0.0f;
    public float jumpSpeed = 0.0f;
    public float maxSpeed = 1.0f;
    public float maxJumpTime = 3.0f;
    private bool isGrounded = false;
    public float jumpForce = 10.0f;
    public Quaternion q;
    public float jumpTimeCounter = 0.0f;
    public float minJumpForce;
    public float jumpTime;
    public float maxJumpForce;
    public bool anyMovement = false;

    enum TYPES
    {
        NONE,
        LEFT,
        RIGHT,
        JUMP
    }

    void Start()
    {
       
        movementSpeed = movementSpeed / 30;
        //q = new Quaternion((float)-0.5, (float)-0.5,(float)0.5, (float)0.5);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            Movment(TYPES.LEFT);
        }
        else if (speed > 0)
        {
            anyMovement = true;

            speed += -movementSpeed * Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
           Movment(TYPES.RIGHT);
        }
        else if (speed < 0)
        {
            anyMovement = true;

            speed += movementSpeed * Time.deltaTime;
            if (speed > 0)
            {
                speed = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Movment(TYPES.JUMP);
            Debug.Log("Jump");
        }

        if (Input.GetButton("Jump") && jumpTimeCounter > 0 && isGrounded)
        {
            ContinueJump();
            Debug.Log("Jumpp");
        }

        if (Input.GetButtonUp("Jump") && isGrounded)
        {
            EndJump();
        }

        if (jumpTimeCounter < 0 && isGrounded)
        {
            EndJump();
        }

        transform.position += Vector3.left * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //transform.rotation = q;


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpTimeCounter = jumpTime;
            Debug.Log("DAVID TONTO");

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player is not on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void ContinueJump()
    {
        if (jumpTimeCounter > 0)
        {
            jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, 1 - (jumpTimeCounter / jumpTime));
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            EndJump();
        }
    }

    void EndJump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        jumpTimeCounter = 0;
    }

    void Movment(TYPES type)
    {

        int i = (int)type;

        switch (i)
        {
            case 1:
                anyMovement = true;
                speed += movementSpeed * Time.deltaTime;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
                break;
            case 2:
                anyMovement = true;
                speed -= movementSpeed * Time.deltaTime;
                if (speed < -maxSpeed)
                {
                    speed = -maxSpeed;
                }
                break;
            case 3:
                jumpForce = minJumpForce;
                jumpTimeCounter = jumpTime;
                Debug.Log("Jumppp");
                break;
        }
    }
}   



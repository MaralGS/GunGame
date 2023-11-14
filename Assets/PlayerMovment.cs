using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    // Start is called before the first frame update
    public float verticalInput;
    public float movementSpeed;
    private float speed = 0.0f;
    private float verticalspeed = 0.0f;
    public float maxSpeed = 5.0f;
    void Start()
    {
        movementSpeed = movementSpeed / 30;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetKey(KeyCode.A))
        {
           speed += movementSpeed * Time.deltaTime;
            if(speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }
        else if(speed > 0)
        {
            speed += -movementSpeed * Time.deltaTime;
            if(speed < 0)
            {
                speed = 0;
            }
        }
        
        if (Input.GetKey(KeyCode.D))
        {
           speed -= movementSpeed * Time.deltaTime;
            if(speed < -maxSpeed)
            {
                speed = -maxSpeed;
            }
        }
        else if(speed < 0)
        {
            speed += movementSpeed * Time.deltaTime;
            if(speed > 0)
            {
                speed = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) )
        {
            JumpAction();
        }

        transform.position += Vector3.back * speed;
        transform.position += Vector3.up * verticalspeed;
        verticalspeed = 0;
    }

    void JumpAction()
    {
        verticalspeed = 0.75f;
    }

}



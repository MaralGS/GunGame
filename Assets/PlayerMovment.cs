using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    // Start is called before the first frame update
    public float verticalInput;
    public float movementSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + transform.right * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + (-transform.right) * movementSpeed * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            JumpAction();
        }

    }

    void JumpAction()
    {
        for (int i = 0; i < 10; i++)
        {
            transform.position = transform.position + (transform.up) * verticalInput * Time.deltaTime;
        }

    }
}



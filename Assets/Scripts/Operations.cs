using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Operations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Normal Operation start");
            for (int i = 0; i < 15000000; i++)
            {
                float a = Mathf.Sqrt(Mathf.Cos(i));
            }

            Debug.Log("Normal Operation finished");
        }
        //thread
        if (Input.GetKeyDown(KeyCode.T))
        {
            Thread myThread = new Thread(ThreadInfiniteSecondPrinter);
            myThread.Start();
        }*/
    }

    void ThreadInfiniteSecondPrinter()
    {
        Debug.Log("Starting Thread!");

        for (int i = 0; i < 15000000; i++)
        {
            float a = Mathf.Sqrt(Mathf.Cos(i));
        }
        Debug.Log("Ended Thread!");
    }
}

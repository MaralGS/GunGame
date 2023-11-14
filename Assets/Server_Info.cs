using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server_Info : MonoBehaviour
{

    [HideInInspector] public Socket Server_Sock;
    [HideInInspector] public EndPoint Remote_EP;
    Server SUDP;
    // Start is called before the first frame update
    void Start()
    {
        SUDP = GameObject.Find("Server_UDP").GetComponent<Server>();
 
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(this);
        if (Input.GetKeyDown(KeyCode.X))
        {
            IsServer();
        }
    }
    public void IsServer()
    {
        Server_Sock = SUDP.newsock;
        Remote_EP = SUDP.Remote;
    }
}

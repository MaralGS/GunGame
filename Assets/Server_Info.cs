using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server_Info : MonoBehaviour
{
    [HideInInspector] public Socket Server;
    [HideInInspector] public EndPoint Remote;
    Server UDP;
    // Start is called before the first frame update
    void Start()
    {
        Server = UDP.newsock;
        Remote = UDP.Remote;
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(this);
    }
}

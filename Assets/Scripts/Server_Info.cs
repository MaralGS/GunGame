using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server_Info : MonoBehaviour
{

    [HideInInspector] public Socket sock;
    [HideInInspector] public EndPoint ep;
    [HideInInspector] public int type; 
    void Start()
    {
        DontDestroyOnLoad(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server_Info : MonoBehaviour
{

    [HideInInspector] public Socket sock;
    [HideInInspector] public EndPoint[] ep;
    [HideInInspector] public EndPoint serverEp;
    [HideInInspector] public int type; 
    [HideInInspector] public string name;
    [HideInInspector] public int numberOfPlayers;
    [HideInInspector] public bool startServer;
    [HideInInspector] public bool im_Client;
    void Start()
    {
        im_Client = false;
        ep = new EndPoint[3];
        DontDestroyOnLoad(this);
    }
}

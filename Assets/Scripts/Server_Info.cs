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
    [HideInInspector] public EndPoint clientEp;
    [HideInInspector] public int clientID = 0; 
    [HideInInspector] public string name;
    [HideInInspector] public string winner;
    [HideInInspector] public string loser;
    [HideInInspector] public int numberOfPlayers;
    [HideInInspector] public bool startServer;
    [HideInInspector] public bool im_Client;

    void Start()
    {
        im_Client= false;
        ep = new EndPoint[3];
        DontDestroyOnLoad(this);
    }
}

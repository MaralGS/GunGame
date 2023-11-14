using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server_Info : MonoBehaviour
{

    [HideInInspector] public Socket sock;
    [HideInInspector] public EndPoint ep;
    bool client;
    //Server SUDP;
    //// Start is called before the first frame update
   void Start()
   {

   }
   
    //// Update is called once per frame
    //void Update()
    //{
    //    DontDestroyOnLoad(this);
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        IsServer();
    //    }
    //}
    public void SaveInfo(Socket rsock, EndPoint r_ep, bool risClient)
    {
        sock = rsock;
        ep = r_ep;
        client = risClient;
    }
}

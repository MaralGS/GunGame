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
    //Server SUDP;
    //// Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }
  
    public void SaveInfo(int type)
    {
        this.type = type;
    }
}

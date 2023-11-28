using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.WebSockets;
//using UnityEditor.PackageManager;
using System.Threading;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{

    public static Server _instance;

    public static Server Instanace => _instance;

    Thread serverThread;
    int recv;
    int numberPlayers;
    byte[] data = new byte[1024];
    [HideInInspector] public Socket[] newsock;
    [HideInInspector] public EndPoint Remote;
    IPEndPoint[] ipep;
    public GameObject TextName;
    string UserName;
    string ClientM;
    bool imWaiting = false;
    bool newConection = false;
    [HideInInspector] public string type = "Server";

    private void Awake()
    {
        newsock = new Socket[4];
        ipep = new IPEndPoint[4];


    }

    public void StartServer()
    {
        if (!imWaiting)
        {
            newsock[1] = new Socket(AddressFamily.InterNetwork,
            SocketType.Dgram, ProtocolType.Udp);

           // serverThread = new Thread(StartThread);
            serverThread.Start();
        }
        else
        {
            imWaiting = false;
            Debug.Log("server started");
        }

    }
    public void ChangeName()
    {
        if (UserName != "")
        {
            UserName = TextName.GetComponent<TMP_InputField>().text;
        }
        else
        {
            UserName = "Hola";
        }
      
    }

    private void Update()
    {

        if (newConection == true) { 
        }

        if (ClientM == "Connected")
        {
            SaveServer();
            ClientM = "Disconnected";
        }
    }
   
    //void StartThread()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < ipep.Length; i++)
    //        {
    //            ipep[i] = new IPEndPoint(IPAddress.Any, 9050+i);
    //            newsock[i++].Bind(ipep[i]);
    //
    //            Debug.Log("Waiting for a clients...");
    //
    //            imWaiting = true;
    //        }
    //    }
    //    IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
    //    Remote = (EndPoint)(sender);
    //    try
    //    {
    //        recv = newsock.ReceiveFrom(data, ref Remote);
    //        Debug.Log("Message received from:" + Remote.ToString());
    //        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
    //        ClientM = Encoding.ASCII.GetString(data, 0, recv);
    //        string welcome = "StartServer";
    //        data = Encoding.ASCII.GetBytes(welcome);
    //        newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
    //    }
    //    catch (Exception)
    //    {
    //        Debug.Log("Connected failed... try again...");
    //        throw;
    //    }
    //}

    void SaveServer()
    {
        Server_Info S_info = FindAnyObjectByType<Server_Info>();
    //    S_info.sock = newsock;
        S_info.ep = Remote;
        SceneManager.LoadScene(1);
        S_info.name = UserName;
    }
}
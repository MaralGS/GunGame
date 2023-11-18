using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
C# Network Programming 
by Richard Blum

Publisher: Sybex 
ISBN: 0782141765
*/
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

    Thread serverThread;
    int recv;
    byte[] data = new byte[1024];
    [HideInInspector] public Socket newsock;
    [HideInInspector] public EndPoint Remote;
    IPEndPoint ipep;
    public GameObject TextName;
    string UserName;
    string ClientM;
    Server_Info info;
    void Start()
    {
        //StartServer();
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
        if (ClientM == "Connected")
        {
            SaveServer();
        }
    }
    public void StartServer()
    {
        ipep = new IPEndPoint(IPAddress.Any, 9050);

        newsock = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

        newsock.Bind(ipep);

        info = GameObject.Find("Perma_server").GetComponent<Server_Info>();

        Debug.Log("Waiting for a client...");
        serverThread = new Thread(StartThread);
        serverThread.Start();
     
        
    }
    void StartThread()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = (EndPoint)(sender);
        try
        {
            recv = newsock.ReceiveFrom(data, ref Remote);
            Debug.Log("Message received from:" + Remote.ToString());
            Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
            ClientM = Encoding.ASCII.GetString(data, 0, recv);
            string welcome = "StartServer";
            data = Encoding.ASCII.GetBytes(welcome);
            newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
        }
        catch (Exception)
        {
            Debug.Log("Connected failed... try again...");
            throw;
        }
    }

    void SaveServer()
    {
        info.SaveInfo(newsock, ipep, 0);
        SceneManager.LoadScene(1);
    }
}
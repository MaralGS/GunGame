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
using UnityEditor.PackageManager;
using System.Threading;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{

    Thread serverThread;
    int recv;
    byte[] data = new byte[1024];
    Socket newsock;
    IPEndPoint ipep;
    public GameObject TextName;
    string UserName;
    void Start()
    {
        
    }

    public void ChangeName()
    {
        UserName = TextName.GetComponent<TMP_InputField>().text;
    }

    public void StartServer()
    {
        ipep = new IPEndPoint(IPAddress.Any, 9050);

        newsock = new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);

        newsock.Bind(ipep);

        Debug.Log("Waiting for a client...");
        serverThread = new Thread(StartThread);
        serverThread.Start();
        SceneManager.LoadScene(2);
    }
    void StartThread()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        recv = newsock.ReceiveFrom(data, ref Remote);

        Debug.Log("Message received from:" + Remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

        string welcome = "Welcome to "  + UserName + " server";
        data = Encoding.ASCII.GetBytes(welcome);
        newsock.SendTo(data, data.Length, SocketFlags.None, Remote);

        if (newsock.Connected) {newsock.Shutdown(SocketShutdown.Both);}
        newsock.Close();

    }
}
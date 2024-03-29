using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;
using UnityEditor;
//using UnityEditor.PackageManager;
//using UnityEngine.tvOS;

public class Client : MonoBehaviour
{

    public static Client _instance;

    public static Client Instanace => _instance;

    byte[] data = new byte[1024];
    public GameObject ip;
    public GameObject textName;
    string ServerM;
    string usingIP;
    string userName;
    Thread clientThread;
    [HideInInspector] public Socket client;
    IPEndPoint ipep;
    public EndPoint remote;
    [HideInInspector] public string type = "Client";
    [HideInInspector] public bool gameStarted = false;
    [HideInInspector] public MenuConections mConections;
    [HideInInspector] public int nplayers = 0;

    Server_Info S_info;

    private void Start()
    {
        S_info = FindAnyObjectByType<Server_Info>();
        mConections = GameObject.Find("MenuConections").GetComponent<MenuConections>();
    }

    private void Update()
    {
        for (int i = 1; i <= 2; i++)
        {
            if (ServerM == "StartServer"+i)
            {

                S_info.sock = client;
                S_info.serverEp = remote;
                S_info.clientEp = ipep;
                S_info.name = userName;
                mConections.start = true;
                mConections.imClient = true;
                ServerM = "StopServer";
                S_info.im_Client = true;
                S_info.clientID = i;
            }
        }

        if (gameStarted == true)
        {
            S_info.numberOfPlayers = nplayers;
            SceneManager.LoadScene(1);
        }

    }
    public void ConnectPlayer()
    {

        usingIP = ip.GetComponent<TMP_InputField>().text;

        ipep = new IPEndPoint(
                       IPAddress.Parse(usingIP), 9050);

        client = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);

        clientThread = new Thread(StartThread);
        clientThread.Start();
        
    }

    void StartThread()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)sender;

        try
        {
            string welcome = "Connected";
            data = Encoding.ASCII.GetBytes(welcome);
            client.SendTo(data, data.Length, SocketFlags.None, ipep);
            data = new byte[1024];
            int recv = client.ReceiveFrom(data, ref remote);
            Debug.Log("Message received from:" + remote.ToString());
            ServerM = Encoding.ASCII.GetString(data, 0, recv);
        }
        catch (Exception e)
        {
            Debug.Log("Unable to connect to server.");
            Debug.Log(e.ToString());

        }
    }

    public void ChangeName()
    {
        if (userName != "")
        {
            userName = textName.GetComponent<TMP_InputField>().text;
        }
        else
        {
            userName = "Hola";
        }

    }

}

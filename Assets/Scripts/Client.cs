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

    //Green/Grey Button for connection of players

    private void Start()
    {
        mConections = GameObject.Find("MenuConections").GetComponent<MenuConections>();
    }

    private void Update()
    {
        if (ServerM == "StartServer")
        {
            Server_Info S_info = FindAnyObjectByType<Server_Info>();
            S_info.sock = client;
            S_info.serverEp = remote;
            S_info.name = userName;
            mConections.start = true; 
            mConections.imClient = true;
            ServerM = "StopServer";
        }
        if (gameStarted == true)
        {
            SceneManager.LoadScene(1);
        }

    }
    public void ConnectPlayer()
    {

        //userName = TextName.GetComponent<TMP_InputField>().text;
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
 
            //Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
            ServerM = Encoding.ASCII.GetString(data, 0, recv);
        }
        catch (Exception e)
        {
            Debug.Log("Unable to connect to server.");
            Debug.Log(e.ToString());

        }
        //Debug.Log("Stopping client");
        //server.Close();
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

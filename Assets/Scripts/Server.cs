
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


public class Server : MonoBehaviour
{

    public static Server _instance;

    public static Server Instanace => _instance;

    Thread serverThread;
    byte[] data = new byte[1024];
    [HideInInspector] public int numberPlayers = 0;
    [HideInInspector] public Socket newsock;
    [HideInInspector] public EndPoint[] Remote;
    [HideInInspector] public bool gameStarted;
    [HideInInspector] public bool menuReciveActive;
    [HideInInspector] public string type = "Server";
    [HideInInspector] public MenuConections mConections;
    IPEndPoint[] ipep;
    public GameObject TextName;
    string UserName;
    string ClientM;
    bool imWaiting = false;
    public int Connection;


    private void Awake()
    {
        gameStarted = false; 
        ipep = new IPEndPoint[4];
        Remote = new EndPoint[3];

    }

    private void Start()
    {
        mConections = GameObject.Find("MenuConections").GetComponent<MenuConections>();
   
    }

    public void StartServer()
    {
        int port = 9050;
        bool ConnectPort = false;

        if (!imWaiting)
        {
            newsock = new Socket(AddressFamily.InterNetwork,
            SocketType.Dgram, ProtocolType.Udp);

            while(!ConnectPort)
            {
                try
                {
                    ipep[0] = new IPEndPoint(IPAddress.Any, port);
                    newsock.Bind(ipep[0]);

                    ConnectPort = true;
                   
                }
                catch
                {
                    port++;
                }
            }

            serverThread = new Thread(StartThread);
            serverThread.Start();
            imWaiting = true;
            Connection = 1;
        }
        else
        {
            imWaiting = false;
            Debug.Log("server started");  
        }

    }


    private void Update()
    {

        if (ClientM == "Connected")
        {
            numberPlayers++;
            SaveServer();
            Connection = numberPlayers;
            ClientM = "Disconnected";
        }

        if (gameStarted == true)
        {
            gameStarted = true;
            serverThread.Abort();
        }
        Debug.Log("Numero de Players: "+numberPlayers);
    }
   
   public void StartThread()
   {

       while (gameStarted == false) { 
           for (int i = 1; i < ipep.Length; i++)
           {
                //REVISAR
                if (ipep[i] == null && menuReciveActive == false)
                {
                    ipep[i] = new IPEndPoint(IPAddress.Any, 9050 + i);
                    Remote[i - 1] = (EndPoint)(ipep[i]);

                    //HASTA AQUI

                    try
                    {

                        int recv = newsock.ReceiveFrom(data, ref Remote[i - 1]); //recv????
                        Debug.Log("Message received from:" + Remote.ToString());
                        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
                        ClientM = Encoding.ASCII.GetString(data, 0, recv);
                        string welcome = "StartServer" + i;
                        data = Encoding.ASCII.GetBytes(welcome);
                        newsock.SendTo(data, data.Length, SocketFlags.None, Remote[i - 1]);



                    }
                    catch (Exception)
                    {
                        Debug.Log("Connected failed... try again...");
                        // throw;
                    }
                }
            }
       }
   }

    void SaveServer()
    {

        Server_Info S_info = FindAnyObjectByType<Server_Info>();
        S_info.sock = newsock;
        for (int i = 0; i < numberPlayers; i++)
        {
            S_info.ep[i] = Remote[i];
        }


        S_info.name = UserName;
        S_info.numberOfPlayers = numberPlayers;
        S_info.startServer = gameStarted;
        mConections.start = true;
        mConections.imServer = true;

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
    public void GameStart()
    {
        gameStarted = true;

    }

}
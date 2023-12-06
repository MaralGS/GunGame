//https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.select?view=net-5.0
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
    int numberPlayers = 0;
    byte[] data = new byte[1024];
    [HideInInspector] public Socket[] clientSock;
    [HideInInspector] public Socket newsock;
    [HideInInspector] public EndPoint Remote;
    IPEndPoint[] ipep;
    public GameObject TextName;
    string UserName;
    string ClientM;
    bool imWaiting = false;
    bool serverStarted = true;

    [HideInInspector] public string type = "Server";

    private void Awake()
    {
        clientSock = new Socket[4];
        ipep = new IPEndPoint[4];


    }

    public void StartServer()
    {
        if (!imWaiting)
        {

            newsock = new Socket(AddressFamily.InterNetwork,
            SocketType.Dgram, ProtocolType.Udp);
            
            ipep[0] = new IPEndPoint(IPAddress.Any, 9050);
            newsock.Bind(ipep[0]);
           
           
            serverThread = new Thread(StartThread);
            serverThread.Start();
            imWaiting = true;
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
            SaveServer();
            ClientM = "Disconnected";
        }
    }
   
   void StartThread()
   {
       while (serverStarted == true) { 
           for (int i = 1; i < ipep.Length; i++)
           {
                if (ipep[i] == null)
                {
                    ipep[i] = new IPEndPoint(IPAddress.Any, 9050 + i);                   
                    Remote = (EndPoint)(ipep[i]);
                    try
                    {
                        int recv = newsock.ReceiveFrom(data, ref Remote); //recv????
                        Debug.Log("Message received from:" + Remote.ToString());
                        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
                        ClientM = Encoding.ASCII.GetString(data, 0, recv);
                        string welcome = "StartServer";
                        numberPlayers++;
                        data = Encoding.ASCII.GetBytes(welcome);
                        newsock.SendTo(data, data.Length, SocketFlags.None, Remote);

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
        S_info.ep = Remote;
       

        //SceneManager.LoadScene(1);
        S_info.name = UserName;
        S_info.numberOfPlayers = numberPlayers;
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

}
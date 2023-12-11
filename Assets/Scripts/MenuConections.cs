//using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
//using UnityEditor.PackageManager;
//using UnityEngine.tvOS;

public class MenuConections : MonoBehaviour
{

    public struct ConectionsInfo
    {
        public bool Start;
        public int ConnectedPlayer;
    }

    public ConectionsInfo _serverStruct;
    public ConectionsInfo _clientStruct;


    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    public Server_Info _info;
    public GameObject client;
    public GameObject server;
    [HideInInspector] public bool start;
    private bool going;
    [HideInInspector] public bool imServer;
    [HideInInspector] public bool imClient;
    Server _server;
    bool gameStarted;
    // Start is called before the first frame update
    void Start() {

        _serverStruct = new ConectionsInfo();
        _clientStruct = new ConectionsInfo();

        _server = FindAnyObjectByType<Server>();

        imServer = false;
        imClient = false;

        going = true;
        start = false;
        gameStarted = false;
        StartThread();
    }


    // Update is called once per frame
    void Update()
    {

        if (imServer == true)
        {
           _serverStruct.Start = server.GetComponent<Server>().gameStarted;
          // _serverStruct.ConnectedPlayer = server.GetComponent<Server>().Connection;
           //imServer = false;
        }
        else if (imClient == true)
        {
            client.GetComponent<Client>().gameStarted = _clientStruct.Start;
            client.GetComponent<Client>().players = _clientStruct.ConnectedPlayer;
            client.GetComponent<Client>().OnConnectToServer();
            //imClient = false;
        }

        if (gameStarted == true)
        {

        }
    }

    public void StartGame()
    {
        _info = FindAnyObjectByType<Server_Info>();



    }

    void StartThread()
    {
        ThreadRecieveInfo = new Thread(ReciveInfo);
        ThreadRecieveInfo.Start();
        ThreadSendInfo = new Thread(SendInfo);
        ThreadSendInfo.Start();
 


    }

    void SendInfo()
    {
        while (going == true)
        {
           
            if (start == true && imClient == false)
            {
                string P_Info = JsonUtility.ToJson(_serverStruct);
                byte[] data = Encoding.ASCII.GetBytes(P_Info);
                //Si es fa pause Funciona, Sino peta ns PK
                for (int i = 0; i < _server.numberPlayers; i++) {
                    _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep[i]);
                }
            }
            //else if (imClient == true)
            //{
            //    string P_Info = JsonUtility.ToJson(_serverStruct);
            //    byte[] data = Encoding.ASCII.GetBytes(P_Info);
            //    _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.serverEp);
            //    
            //}

        }

    }

    void ReciveInfo()
    {
        int[] recv = new int[_server.numberPlayers];
        string[] p_info = new string[_server.numberPlayers];

        byte[] data = new byte[1024];

        while (going == true)
        {
           if (start == true && _server.numberPlayers > 0 )
           {
              // for (int i = 0; i < _server.numberPlayers; i++)
              // {
              //     recv[i] = _info.sock.ReceiveFrom(data, ref _info.ep[i]);
              //     p_info[i] = Encoding.ASCII.GetString(data, 0, recv[i]);
              //     _clientStruct = JsonUtility.FromJson<ConectionsInfo>(p_info[i]);
              // }
           
           }
           else if (imClient == true)
           {
         
                int recvC = _info.sock.ReceiveFrom(data, ref _info.serverEp);
                string p_infoC = Encoding.ASCII.GetString(data, 0, recvC);
                _clientStruct = JsonUtility.FromJson<ConectionsInfo>(p_infoC);
             

           }
        }
    }
}

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
using UnityEngine.SceneManagement;
//using UnityEditor.PackageManager;
//using UnityEngine.tvOS;

public class MenuConections : MonoBehaviour
{

    public struct ConectionsInfo
    {
        public bool Start;
        public int ConnectedPlayer;
        public int NumberOfPlayers;
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
    public GameObject[] Buttons;
    public GameObject[] penguins_players;

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

    void Update()
    {

        if (imServer == true)
        {
           _serverStruct.Start = server.GetComponent<Server>().gameStarted;
           _serverStruct.ConnectedPlayer = server.GetComponent<Server>().Connection;
           _serverStruct.NumberOfPlayers = _info.numberOfPlayers;
        }
        else if (imClient == true)
        {
            client.GetComponent<Client>().gameStarted = _clientStruct.Start;
            client.GetComponent<Client>().nplayers = _clientStruct.NumberOfPlayers;
        }

        if (gameStarted == true || client.GetComponent<Client>().gameStarted == true)
        {
            ThreadRecieveInfo.Abort();
            ThreadSendInfo.Abort();
            if (imServer == true)
            {
                SceneManager.LoadScene(1);
            }

        }
        OnConnectToServer();
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

                for (int i = 0; i < _info.numberOfPlayers; i++)
                {
                    if (_info.ep[i] != null)
                    {
                        _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep[i]);
                    }

                }
                if (_server.gameStarted == true)
                {
                    gameStarted = true;
                }
            }

        }

    }

    void ReciveInfo()
    {

        while (going == true)
        {
            if (imClient == true)
           {
                byte[] data = new byte[1024];
                int recvC = _info.sock.ReceiveFrom(data, ref _info.serverEp);
                string p_infoC = Encoding.ASCII.GetString(data, 0, recvC);
                _clientStruct = JsonUtility.FromJson<ConectionsInfo>(p_infoC);
            }
        }
    }

    public void OnConnectToServer()
    {
        if (imServer == true)
        {
            Buttons[_serverStruct.ConnectedPlayer].GetComponent<Image>().color = new Color(0f, 1f, 0f);
            penguins_players[_serverStruct.ConnectedPlayer].SetActive(true);
        }
        else if(imClient == true) 
        {
            Buttons[_clientStruct.ConnectedPlayer].GetComponent<Image>().color = new Color(0f, 1f, 0f);
            penguins_players[_clientStruct.ConnectedPlayer].SetActive(true);
        }
        
    }
}

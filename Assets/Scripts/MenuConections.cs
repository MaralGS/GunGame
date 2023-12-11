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
    }

    public ConectionsInfo P1_S;
    public ConectionsInfo P2_S;


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

        P1_S = new ConectionsInfo();
        P2_S = new ConectionsInfo();

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
           P1_S.Start = server.GetComponent<Server>().gameStarted;
           //imServer = false;
        }
        else if (imClient == true)
        {
            client.GetComponent<Client>().gameStarted = P2_S.Start;
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
            if (start == true)
            {
                string P_Info = JsonUtility.ToJson(P1_S);
                byte[] data = Encoding.ASCII.GetBytes(P_Info);
                for (int i = 0; i < _server.numberPlayers; i++) {
                    _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep[i]);
                }

          
            }

        }

    }

    void ReciveInfo()
    {
        int[] recv = new int[4];
        string[] p_info = new string[4];

        byte[] data = new byte[1024];

        while (going == true)
        {
            if (start == true && _server.numberPlayers > 1 )
            {
                for (int i = 0; i < _server.numberPlayers; i++)
                {
                    recv[i] = _info.sock.ReceiveFrom(data, ref _info.ep[i]);
                    p_info[i] = Encoding.ASCII.GetString(data, 0, recv[i]);
                    P2_S = JsonUtility.FromJson<ConectionsInfo>(p_info[i]);
                }

            }
            else if (imClient == true)
            {
                byte[] dataC = new byte[1024];
                int recvC = _info.sock.ReceiveFrom(data, ref _info.serverEp);
                string p_infoC = Encoding.ASCII.GetString(data, 0, recvC);
                P2_S = JsonUtility.FromJson<ConectionsInfo>(p_infoC);
            }


        }
    }
}

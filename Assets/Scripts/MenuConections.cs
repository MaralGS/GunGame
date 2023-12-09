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
//using UnityEditor.PackageManager;
//using UnityEngine.tvOS;

public class MenuConections : MonoBehaviour
{
    
    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    public Server_Info _info;
    public GameObject client;
    // Start is called before the first frame update
    void Start() { 

    }


    // Update is called once per frame
    void Update()
    {
        client.GetComponent<Client>().gameStarted = _info.startServer;
    }

    public void StartGame()
    {
        _info = FindAnyObjectByType<Server_Info>();
        client = GameObject.Find("Client");

        StartThread();
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
      string P_Info = JsonUtility.ToJson(_info.startServer);
      byte[] data = Encoding.ASCII.GetBytes(P_Info);
      _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep);

    }

    void ReciveInfo()
    {
        byte[] data = new byte[1024];
        int recv = _info.sock.ReceiveFrom(data, ref _info.ep);
        string P_Info = Encoding.ASCII.GetString(data, 0, recv);
        _info = JsonUtility.FromJson<Server_Info>(P_Info);
    }
}

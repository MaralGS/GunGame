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
//using UnityEngine.tvOS;

public class InGameConnection : MonoBehaviour
{
    struct Player_Info 
    {
        public string name;
        public Vector3 position;
        public int hp;
        public Quaternion rotation;
        public GameObject shot;
        public Vector3 shotPosition;
    }
    Server _server;
    Client _client;
    Player_Info P1;
    Thread _threadSend;
    // Start is called before the first frame update
    void Start()
    {
         _server = Server.Instanace;
         _client = Client.Instanace;


    }
  
    // Update is called once per frame
    void Update()
    {
          if (_server.type == "Server") //0 server 
        {
            //_threadSend = new Thread(SendInfo);
            //_threadSend.Start();
            //Debug.Log(_server.type);
            SendInfo();
        }
        else if (_server.type == "Client") //1 player
        {
            // ReciveInfo();
        }
    }
  
    void SendInfo()
    {
        try
        {
            byte[] data = new byte[1024];
            string P_Info = JsonUtility.ToJson(P1);
            data = Encoding.ASCII.GetBytes(P_Info);
            Debug.Log(data + "holaaaa");
            _server.newsock.SendTo(data, data.Length, SocketFlags.None, _server.Remote);
        }
        catch(Exception)
        {
            Debug.Log("Connected failed... try again...");
            throw;
        }
    }
  
    void ReciveInfo(byte[] data, Socket Server, EndPoint remote)
    {
        int recv = Server.ReceiveFrom(data, ref remote);
        string P_Info = Encoding.ASCII.GetString(data, 0, recv);
        P1 = JsonUtility.FromJson<Player_Info>(P_Info);
    }
    
    public void GetPlayerMovmentInfo(Vector3 pPosition, Quaternion pRotation)
    {
        P1.position = pPosition;
        P1.rotation = pRotation;
    }
    
    public void GetPlayerShotInfo(GameObject pShot, Vector3 pShotPosition)
    {
        P1.shot = pShot;
        P1.shotPosition = pShotPosition;
    }
    public void GetPlayerHPInfo(int pHp)
    {
        P1.hp = pHp;
    }
}

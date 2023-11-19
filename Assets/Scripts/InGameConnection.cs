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
//using UnityEditor.PackageManager;
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
    public Server _server;
    public Client _client;
    Player_Info P1;
    Thread _threadRecieveClient;
    Server_Info _info;
    // Start is called before the first frame update
    void Start()
    {
        _server = Server.Instanace;
        _client = Client.Instanace;
        _info = GameObject.Find("Perma_server").gameObject.GetComponent<Server_Info>();
       //if (_server.type == "Server")
       //{
       //    StartServerThread();
       //}
     
    }

    private void StartClientThread()
    {
        _threadRecieveClient = new Thread(ReciveInfoFS);
        _threadRecieveClient.Start();
    }
    private void StartServerThread()
    {
        //_threadRecieveClient = new Thread(ReciveInfoFC);
        //_threadRecieveClient.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (_info.type == 1)
        {
            StartClientThread();
        }
        if (GameObject.Find("Player1").GetComponent<PlayerMovment>().anyMovement) //0 server 
        {
            SendInfoSTC();
            GameObject.Find("Player1").GetComponent<PlayerMovment>().anyMovement = false;
        }
        /*
        else if (GameObject.Find("Player2").GetComponent<PlayerMovment>().anyMovement) //1 player
        {
            SendInfoCTS();
            GameObject.Find("Player2").GetComponent<PlayerMovment>().anyMovement = false;
        }*/
    }
  
    void SendInfoSTC()
    {
        try
        {
            byte[] data = new byte[1024];
            string P_Info = JsonUtility.ToJson(P1);
            data = Encoding.ASCII.GetBytes(P_Info);
            Debug.Log(data + "holaaaa");
            _server.newsock.SendTo(data, data.Length, SocketFlags.None, _server.Remote); //server to client
        }
        catch(Exception)
        {
            Debug.Log("Connected failed... try again...");
            throw;
        }
    }
    void SendInfoCTS()
    {
        try
        {
            byte[] data = new byte[1024];
            string P_Info = JsonUtility.ToJson(P1);
            data = Encoding.ASCII.GetBytes(P_Info);
            Debug.Log(data + "holaaaa");
            _client.client.SendTo(data, data.Length, SocketFlags.None, _client.remote); //client to server
        }
        catch (Exception)
        {
            Debug.Log("Connected failed... try again...");
            throw;
        }
    }
  
    void ReciveInfoFS()
    {
        try
        {
            byte[] data = new byte[1024];
            int recv = _client.client.ReceiveFrom(data, ref _client.remote);
            string P_Info = Encoding.ASCII.GetString(data, 0, recv);
            P1 = JsonUtility.FromJson<Player_Info>(P_Info);
        }
        catch (Exception)
        {
           //Debug.Log("ERROR");
        }
    }
    void ReciveInfoFC()
    {
        byte[] data = new byte[1024];
        int recv = _server.newsock.ReceiveFrom(data, ref _server.Remote);
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

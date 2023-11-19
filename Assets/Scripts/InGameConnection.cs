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
    public struct Player_Info 
    {
        public Vector3 position;
        public int hp;
        public GameObject shot;
        public Vector3 shotPosition;
        public int gunType;
    }
    Player_Info P1_S;
    Player_Info P2_S;
    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    Server_Info _info;
    PlayerMovment P1;
    GameObject Player1;
    GameObject Player2;
    bool going;
    // Start is called before the first frame update
    void Start()
    {

        P1_S = new Player_Info();
        P2_S = new Player_Info();
        _info = FindAnyObjectByType<Server_Info>();
        P1 = FindAnyObjectByType<PlayerMovment>();
        if (_info.type == 1)
        {
            Player1 = GameObject.Find("Player1").gameObject;
            Player2 = GameObject.Find("Player2").gameObject;
        }
        else if (_info.type == 0)
        {
            Player1 = GameObject.Find("Player2").gameObject;
            Player2 = GameObject.Find("Player1").gameObject;
        }
        going = true;
        StartThread();
    }

 

    // Update is called once per frame
    void Update()
    {
        if(Player1)
        {
            P1_S.position = Player1.transform.position;
        }
        else if (Player2)
        {
      
        }
    }

    private void StartThread()
    {
        ThreadRecieveInfo = new Thread(ReciveInfo);
        ThreadRecieveInfo.Start();
        ThreadSendInfo = new Thread(SendInfo);
        ThreadSendInfo.Start();

    }

    void SendInfo()
    {
        while (going)
        {
            byte[] data = new byte[1024];
            string P_Info = JsonUtility.ToJson(Player1);
            data = Encoding.ASCII.GetBytes(P_Info);
            _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep);
        }     
    }

    void ReciveInfo()
    {
        while (true)
        {
            byte[] data = new byte[1024];
            int recv = _info.sock.ReceiveFrom(data, ref _info.ep);
            string P_Info = Encoding.ASCII.GetString(data, 0, recv);
            Player1 = JsonUtility.FromJson<Player_Info>(P_Info);
        }
            
    }
    
    public void GetPlayerMovmentInfo(Vector3 pPosition)
    {
    }
    
    public void GetPlayerShotInfo(GameObject pShot, Vector3 pShotPosition)
    {
    }
    public void GetPlayerHPInfo(int pHp)
    {
  
    }
}

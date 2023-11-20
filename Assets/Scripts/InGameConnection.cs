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
        public Quaternion rotation;
        public int hp;
        public bool alive;
        public int gunNum;
        //public GameObject shot;
        //public Vector3 shotPosition;
    }
    Player_Info P1_S;
    Player_Info P2_S;
    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    Server_Info _info;
    PlayerMovment P1_movement;
    GameObject Player1;
    GameObject Player2;
    bool imServer = false;
    bool imClient = false;
    bool going;
    // Start is called before the first frame update
    void Start()
    {

        P1_S = new Player_Info();
        P2_S = new Player_Info();
        _info = FindAnyObjectByType<Server_Info>();
        P1_movement = FindAnyObjectByType<PlayerMovment>();
        if (_info.type == 1)
        {
            Player1 = GameObject.FindGameObjectWithTag("Player");
            Player2 = GameObject.FindGameObjectWithTag("Player2");  

        }
        else if (_info.type == 0)
        {
            Player1 = GameObject.FindGameObjectWithTag("Player2");
            Player2 = GameObject.FindGameObjectWithTag("Player");
        }
        Player2.GetComponent<PlayerMovment>().enabled = false;
        Player2.GetComponent<PlayerShoot>().enabled = false;
        //Player2.GetComponent<HpHandler>().enabled = false;
        going = true;
        StartThread();
    }

    // Update is called once per frame
    void Update()
    {
        if(imServer)
        {
            P1_S.position = Player1.transform.position;
            P1_S.rotation = Player1.transform.rotation;
            P1_S.alive = Player1.GetComponent<HpHandler>().alive;
            P1_S.gunNum = Player1.GetComponent<PlayerShoot>().gunType;
            //P1_S.hp = Player1.GetComponent<HpHandler>().hp;
            imServer = false;

        }
        if (imClient)
        {
            Player2.transform.position = P2_S.position;
            Player2.transform.rotation = P2_S.rotation;
            Player2.GetComponent<HpHandler>().alive = P2_S.alive;
            //Player2.GetComponent<HpHandler>().hp = P2_S.hp;
            Player2.GetComponent<PlayerShoot>().gunType = P2_S.gunNum;
            
            imClient = false;
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
            imServer = true;
 
            string P_Info = JsonUtility.ToJson(P1_S);

            byte[] data = Encoding.ASCII.GetBytes(P_Info);
            _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep);
        }     
    }

    void ReciveInfo()
    {
        while (going)
        {
           
            byte[] data = new byte[1024];
            int recv = _info.sock.ReceiveFrom(data, ref _info.ep);
            string P_Info = Encoding.ASCII.GetString(data, 0, recv);
            P2_S = JsonUtility.FromJson<Player_Info>(P_Info);

            imClient = true;

        }
            
    }
}

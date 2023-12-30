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
using static MenuConections;
//using UnityEditor.PackageManager;
//using UnityEngine.tvOS;
public class InGameConnection : MonoBehaviour
{
    public struct Player_Info
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
        public int hp;
        public bool alive;
        public bool shield;
        public int gunNum;
        public bool shot;
        public Vector3 v;
        public int playerNum;
    }
    public Player_Info _thisPlayer;
    public Player_Info _thisEnemy;
    public Player_Info _client1;
    public Player_Info _client2;
    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    [HideInInspector] public Server_Info _info;
    public GameObject players;
    public GameObject serverScreen;
    [HideInInspector] public GameObject[] player;
    public GameObject respawnPosition;

    bool going = true;
    public Vector3 v2;
    EndPoint Remote;

    //update the info you recive
    bool _updatePlayer;
    bool _updateEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //Define the remote
        Remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
        _info = FindAnyObjectByType<Server_Info>();
        player = new GameObject[2];

        //Define the player you will play
        _thisPlayer = new Player_Info(); 

        //Instantiate Players
        //Your player == 1
        player[0] = Instantiate(players);
        player[0].name = "Player1";
        //Enemy player == 2
        player[1] = Instantiate(players);
        player[1].name = "Player2";


        if (_info.clientID == 1)
        {
            _thisPlayer.playerNum = 1;

            player[1].GetComponent<Animator>().enabled = false;
            player[1].GetComponent<PlayerMovment>().enabled = false;
            player[1].GetComponent<PlayerShoot>().enabled = false;
            player[1].GetComponent<Shield>().enabled = false;

        }
        else if (_info.clientID == 2)
        {
            _thisPlayer.playerNum = 2;
            player[0].GetComponent<Animator>().enabled = false;
            player[0].GetComponent<PlayerMovment>().enabled = false;
            player[0].GetComponent<PlayerShoot>().enabled = false;
            player[0].GetComponent<Shield>().enabled = false;
        }



        GameObject respawnPosition1 = Instantiate(respawnPosition);
        respawnPosition1.transform.position = new Vector3(7.11000013f, 1.36000001f, 0);

        GameObject respawnPosition2 = Instantiate(respawnPosition);
        respawnPosition2.transform.position = new Vector3(-6.53999996f, 0.866069973f, 0);

        GameObject respawnPosition3 = Instantiate(respawnPosition);
        respawnPosition3.transform.position = new Vector3(4.30000019f, 6.0f, 0);

        GameObject respawnPosition4 = Instantiate(respawnPosition);
        respawnPosition4.transform.position = new Vector3(-4.8499999f, 4.59000015f, 0);



        if (!_info.im_Client)
        {

            //Server disables the 2 players

                 player[0].GetComponent<Animator>().enabled = false;
                 player[0].GetComponent<PlayerMovment>().enabled = false;
                 player[0].GetComponent<PlayerShoot>().enabled = false;
                 player[0].GetComponent<Shield>().enabled = false;

                 player[1].GetComponent<Animator>().enabled = false;
                 player[1].GetComponent<PlayerMovment>().enabled = false;
                 player[1].GetComponent<PlayerShoot>().enabled = false;
                 player[1].GetComponent<Shield>().enabled = false;

            //Create Black screen for the server
                 Instantiate(serverScreen);
        }

        else
        {
                player[0].transform.position = respawnPosition1.transform.position;
                player[1].transform.position = respawnPosition2.transform.position;
        }       


        going = true;
        StartThread();
    }


    // Update is called once per frame
    void Update()
    {
       if (_updatePlayer == true)//Passar info
       {
            if(_info.clientID == 1)
            {
                _thisPlayer.position = player[0].transform.position;
                _thisPlayer.name = player[0].name;
                _thisPlayer.gunNum = player[0].GetComponent<PlayerShoot>().gunType;
                _thisPlayer.shot = player[0].GetComponent<PlayerShoot>().imShooting;
                _thisPlayer.v = player[0].GetComponent<PlayerShoot>().shootDirection;
                _thisPlayer.shield = player[0].GetComponent<Shield>().shieldActive;

            }
            else if(_info.clientID == 2)
            {
                _thisPlayer.position = player[1].transform.position;
                _thisPlayer.name = player[1].name;
                _thisPlayer.gunNum = player[1].GetComponent<PlayerShoot>().gunType;
                _thisPlayer.shot = player[1].GetComponent<PlayerShoot>().imShooting;
                _thisPlayer.v = player[1].GetComponent<PlayerShoot>().shootDirection;
                _thisPlayer.shield = player[1].GetComponent<Shield>().shieldActive;

            }

            _updatePlayer = false;
       }
       
       if (_updateEnemy == true)//Rebre info
       {
            if (_info.clientID == 1)
            {
                player[1].transform.position = _thisEnemy.position;
                player[1].name = _thisEnemy.name;
                player[1].GetComponent<Shield>().shield.SetActive(_thisEnemy.shield);
                
                if(_thisEnemy.shot == true)
                {
                    player[1].GetComponent<PlayerShoot>().Shoot(_thisEnemy.v, _thisEnemy.position, _thisEnemy.gunNum, 2, true);
                    _thisEnemy.shot = false;
                }



            }
            else if (_info.clientID == 2)
            {
                player[0].transform.position = _thisEnemy.position;
                player[0].name = _thisEnemy.name;
                player[0].GetComponent<Shield>().shield.SetActive(_thisEnemy.shield);
                
                if (_thisEnemy.shot == true)
                {
                    player[0].GetComponent<PlayerShoot>().Shoot(_thisEnemy.v, _thisEnemy.position, _thisEnemy.gunNum, 1, true);
                    _thisEnemy.shot = false;
                }

            }

            _updateEnemy = false;
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
        while (going == true)
        {

            if (_info.im_Client == false)
            {
                 string P1_Info = JsonUtility.ToJson(_client1);
                 byte[] data1 = Encoding.ASCII.GetBytes(P1_Info); 
                 _info.sock.SendTo(data1, data1.Length, SocketFlags.None, _info.ep[1]);

                string P2_Info = JsonUtility.ToJson(_client2);
                byte[] data2 = Encoding.ASCII.GetBytes(P2_Info);
                _info.sock.SendTo(data2, data2.Length, SocketFlags.None, _info.ep[0]);
            }
            else 
            {
                 _updatePlayer = true;
                 string P_Info = JsonUtility.ToJson(_thisPlayer);
                 byte[] data = Encoding.ASCII.GetBytes(P_Info);
                 _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.serverEp);

            }

        }

    }

    void ReciveInfo()
    {
        while (going == true)
        {

            if (_info.im_Client == false)
            {
                byte[] data = new byte[1024];
                int recv = _info.sock.ReceiveFrom(data, ref Remote);
                string json = Encoding.ASCII.GetString(data, 0, recv);

                _thisPlayer = JsonUtility.FromJson<Player_Info>(json);

                if(_thisPlayer.playerNum == 1)
                {
                    _client1 = _thisPlayer;
                }
                else if(_thisPlayer.playerNum == 2)
                {
                    _client2 = _thisPlayer;
                }
            }

            else if (_info.im_Client == true)
            {
                byte[] data = new byte[1024];
                int recvC = _info.sock.ReceiveFrom(data, ref _info.serverEp);
                string p_infoC = Encoding.ASCII.GetString(data, 0, recvC);
            
                _thisEnemy = JsonUtility.FromJson<Player_Info>(p_infoC);

                _updateEnemy = true;
            }
        }
    }
}
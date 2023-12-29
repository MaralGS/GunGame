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
    public GameObject Players;
    GameObject ClientPlayer;
    GameObject ClientEnemy;
    [HideInInspector] public GameObject[] player;
    public GameObject respawnPosition;
    GameObject[] respawnPositions;

    public Camera cam;
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
        player = new GameObject[3];
        //Define the player you will play
        _thisPlayer = new Player_Info(); 

        //Instantiate Players
      
        //Your player == 1
        player[1] = Instantiate(Players);
        //Enemy player == 2
        player[2] = Instantiate(Players);


        if (_info.clientID == 1)
        {
            _thisPlayer.playerNum = 1;
            ClientPlayer = player[1];
            ClientEnemy = player[2];
            ClientPlayer.name = _info.name;

        }
        else if (_info.clientID == 2)
        {
            _thisPlayer.playerNum = 2;
            ClientPlayer = player[2];
            ClientEnemy = player[1];
            ClientPlayer.name = _info.name;
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

                 player[1].GetComponent<Animator>().enabled = false;
                 player[1].GetComponent<PlayerMovment>().enabled = false;
                 player[1].GetComponent<PlayerShoot>().enabled = false;
                 player[1].GetComponent<Shield>().enabled = false;

                 player[2].GetComponent<Animator>().enabled = false;
                 player[2].GetComponent<PlayerMovment>().enabled = false;
                 player[2].GetComponent<PlayerShoot>().enabled = false;
                 player[2].GetComponent<Shield>().enabled = false;  
        }

        else
        {
                ClientEnemy.GetComponent<Animator>().enabled = false;
                ClientEnemy.GetComponent<PlayerMovment>().enabled = false;
                ClientEnemy.GetComponent<PlayerShoot>().enabled = false;
                ClientEnemy.GetComponent<Shield>().enabled = false;

                ClientEnemy.transform.position = respawnPosition1.transform.position;
                ClientPlayer.transform.position = respawnPosition2.transform.position;
        }

        //Destroy(player[1]);
        //Destroy(player[2]);
 
        
        going = true;
        StartThread();
    }


    // Update is called once per frame
    void Update()
    {
       if (_updatePlayer)
       {
           _thisPlayer.position = ClientPlayer.transform.position;
           _thisPlayer.name = ClientPlayer.name;
           _updatePlayer = false;
       }
       else if (_updateEnemy)
       {
           ClientEnemy.transform.position = _thisEnemy.position;
           ClientEnemy.name = _thisEnemy.name;
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
                 _info.sock.SendTo(data1, data1.Length, SocketFlags.None, _info.ep[2]);

                string P2_Info = JsonUtility.ToJson(_client2);
                byte[] data2 = Encoding.ASCII.GetBytes(P2_Info);
                _info.sock.SendTo(data1, data1.Length, SocketFlags.None, _info.ep[1]);
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

            if (_info.im_Client == false && _info.numberOfPlayers > 0)
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

    void Setplayers(int numberPlayer)
    {
        for (int i = 0; i <= _info.numberOfPlayers; i++)
        {
            player[i] = GameObject.Find("Player" + numberPlayer);
            numberPlayer++;
            
            if (i != 0)
            {
                player[i].GetComponent<Animator>().enabled = false;
                player[i].GetComponent<PlayerMovment>().enabled = false;
                player[i].GetComponent<PlayerShoot>().enabled = false;
                player[i].GetComponent<Shield>().enabled = false;
            }
            if (numberPlayer > _info.numberOfPlayers)
            {
                numberPlayer = 0;
            }

        }
    }
}
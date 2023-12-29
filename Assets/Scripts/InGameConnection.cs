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
        public int id;
        public bool shot;
        public Vector3 v;
        //public GameObject shot;
        //public Vector3 shotPosition;
    }

    public Player_Info[] Ps;
    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    [HideInInspector] public Server_Info _info;
    public GameObject Players;
    public GameObject[] player;
    public GameObject respawnPosition;
    GameObject[] respawnPositions;
    public int[] localplayersId;

    public Camera cam;
    bool going = true;
    public Vector3 v2;
    // Start is called before the first frame update
    void Start()
    {
        
        _info = FindAnyObjectByType<Server_Info>();
        Ps = new Player_Info[_info.numberOfPlayers + 1];
        player = new GameObject[_info.numberOfPlayers + 1];
        localplayersId = new int[_info.numberOfPlayers + 1];


        GameObject respawnPosition1 = Instantiate(respawnPosition);
        respawnPosition1.transform.position = new Vector3(7.11000013f, 1.36000001f,0);
       // respawnPositions[0] = respawnPosition1;
        GameObject respawnPosition2 = Instantiate(respawnPosition);
        respawnPosition2.transform.position = new Vector3(-6.53999996f, 0.866069973f, 0);
        //respawnPositions[1] = respawnPosition2;

        GameObject respawnPosition3 = Instantiate(respawnPosition);
        respawnPosition3.transform.position = new Vector3(4.30000019f, 6.0f, 0);
        //respawnPositions[2] = respawnPosition3;

        GameObject respawnPosition4 = Instantiate(respawnPosition);
        respawnPosition4.transform.position = new Vector3(-4.8499999f, 4.59000015f, 0);
        //respawnPositions[3] = respawnPosition4;

        Camera gameCam = Instantiate(cam);


        for (int i = 0; i <= _info.numberOfPlayers; i++)
        {

            Ps[i] = new Player_Info();
            //Instantiate Players
            GameObject Gplayers = Instantiate(Players);
            Gplayers.GetComponent<HpHandler>().respawnPosition1 = respawnPosition1;
            Gplayers.GetComponent<HpHandler>().respawnPosition2 = respawnPosition2;
            Gplayers.GetComponent<HpHandler>().respawnPosition3 = respawnPosition3;
            Gplayers.GetComponent<HpHandler>().respawnPosition4 = respawnPosition4;
            Gplayers.GetComponent<PlayerShoot>().cam = cam;
            player[i] = Gplayers;
     
            player[i].gameObject.name = "Player" + i;
            if (_info.name != null)
            {
                player[i].GetComponentInChildren<TextMeshPro>().text = _info.name;
                player[i].gameObject.transform.position = new Vector3(7.8f + i, 0.86607f, 0);
                //player[i].gameObject.transform.position = respawnPositions[i].transform.position;
            }

        }

        Setplayers(_info.clientID);
        
        going = true;
        StartThread();
    }


    // Update is called once per frame
    void Update()
    {
        Ps[0].name = player[0].GetComponentInChildren<TextMeshPro>().text;
        Ps[0].position = player[0].transform.position;
        Ps[0].alive = player[0].GetComponent<HpHandler>().alive;
        Ps[0].gunNum = player[0].GetComponent<PlayerShoot>().gunType;
        Ps[0].shot = player[0].GetComponent<PlayerShoot>().imShooting;
        Ps[0].v = player[0].GetComponent<PlayerShoot>().shootDirection;
        Ps[0].id = _info.clientID;
        //Ps[0].shield = player[0].GetComponent<Shield>().shieldActive;
        
        for (int i = 1; i <= _info.numberOfPlayers; i++)
        {
            int selectedPlayer = UpdatePlayers(Ps[i].id, localplayersId);
            
            player[selectedPlayer].GetComponentInChildren<TextMeshPro>().text = Ps[i].name;
            player[selectedPlayer].transform.position = Ps[i].position;
            player[selectedPlayer].GetComponent<Collider2D>().enabled = Ps[i].alive;
            player[selectedPlayer].GetComponent<PlayerMovment>().enabled = Ps[i].alive;
            //player[selectedPlayer].GetComponent<Shield>().shield.SetActive(Ps[i].shield);
            player[selectedPlayer].GetComponent<PlayerShoot>().enabled = false;
            player[selectedPlayer].GetComponent<PlayerShoot>().gunType = Ps[i].gunNum;
            player[selectedPlayer].GetComponent<PlayerShoot>().imShooting = Ps[i].shot;
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
                for (int i = 0; i < _info.numberOfPlayers; i++)
                {
                    // Ps[0].id = _info.clientID;
                    string P_Info = JsonUtility.ToJson(Ps[i]);
                    byte[] data = Encoding.ASCII.GetBytes(P_Info);

                    for (int j = 0; j < _info.numberOfPlayers; j++)
                    {
                        if (_info.ep[j] != null)
                        {
                            _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep[j]);
                        }
                    }
                }
            }
            else if (_info.im_Client == true)
            {
                //Aixo tambe esta Xungo
                //Ps[0].id = _info.clientID;
                string P_Info = JsonUtility.ToJson(Ps[0]);
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
                int[] recv = new int[_info.numberOfPlayers];
                string[] p_info = new string[_info.numberOfPlayers];
                byte[] data = new byte[1024];
                for (int i = 0; i < _info.numberOfPlayers; i++)
                {

                    recv[i] = _info.sock.ReceiveFrom(data, ref _info.ep[i]);
                    p_info[i] = Encoding.ASCII.GetString(data, 0, recv[i]);
                    Ps[i + 1] = JsonUtility.FromJson<Player_Info>(p_info[i]);
                }

            }
            else if (_info.im_Client == true)
            {
                //mirar
                for (int i = 0; i < _info.numberOfPlayers; i++)
                {
                    byte[] data = new byte[1024];
                    int recvC = _info.sock.ReceiveFrom(data, ref _info.serverEp);
                    string p_infoC = Encoding.ASCII.GetString(data, 0, recvC);
                    Ps[i + 1] = JsonUtility.FromJson<Player_Info>(p_infoC);
                }

            }
        }
    }


    void Setplayers(int numberPlayer)
    {

        for (int i = 0; i <= _info.numberOfPlayers; i++)
        {
            localplayersId[i] = numberPlayer;
            player[i] = GameObject.Find("Player" + numberPlayer);
            numberPlayer++;
            
            if (i != 0)
            {
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

    int UpdatePlayers(int id, int[] local)
    {

         for (int i = 0; i < _info.numberOfPlayers; i++)
         {
            if (id == local[i])
            {
                return local[i];
            }
         }
        return 0; 
    }
}
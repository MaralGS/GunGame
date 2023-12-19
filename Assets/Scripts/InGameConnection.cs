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
        //public GameObject shot;
        //public Vector3 shotPosition;
    }
    public Player_Info[] Ps;
    Thread ThreadRecieveInfo;
    Thread ThreadSendInfo;
    [HideInInspector] public Server_Info _info;
    public GameObject Players;
    GameObject[] player;
    bool going;
    public Vector3 v2;
    // Start is called before the first frame update
    void Start()
    {

        _info = FindAnyObjectByType<Server_Info>();
        Ps = new Player_Info[_info.numberOfPlayers + 1];
        player = new GameObject[_info.numberOfPlayers + 1];
        


        for (int i = 0; i <= _info.numberOfPlayers; i++)
        {

            Ps[i] = new Player_Info();
            //Instantiate Players
            GameObject Gplayers = Instantiate(Players);
            player[i] = Gplayers;

            player[i].gameObject.name = "Player" + i;
            if (i != 0)
            {
                player[i].gameObject.transform.position = new Vector3(7.8f, 0.86607f, 0);
            }

        }
        Setplayers(GetPlayerNumber(_info.clientEp));


        going = true;
        StartThread();
    }

    // Update is called once per frame
    void Update()
    {
        Ps[0].name = player[0].GetComponentInChildren<TextMeshPro>().text;
        Ps[0].position = player[0].transform.position;
        Ps[0].rotation = player[0].transform.rotation;
        Ps[0].alive = player[0].GetComponent<HpHandler>().alive;
        Ps[0].gunNum = player[0].GetComponent<PlayerShoot>().gunType;
        Ps[0].shot = player[0].GetComponent<PlayerShoot>().imShooting;
        Ps[0].v = player[0].GetComponent<PlayerShoot>().shootDirection;
        Ps[0].shield = player[0].GetComponent<Shield>().shieldActive;

        for (int i = 1; i < _info.numberOfPlayers; i++)
        {

        
            player[i].GetComponentInChildren<TextMeshPro>().text = Ps[i].name;
            player[i].transform.position = Ps[i].position;
            player[i].transform.rotation = Ps[i].rotation;
            player[i].GetComponent<Collider>().enabled = Ps[i].alive;
            player[i].GetComponent<MeshRenderer>().enabled = Ps[i].alive;
            player[i].GetComponent<PlayerMovment>().enabled = Ps[i].alive;
            player[i].GetComponent<Shield>().shield.SetActive(Ps[i].shield);
            player[i].GetComponent<PlayerShoot>().enabled = false;
            player[i].GetComponent<PlayerShoot>().gunType = Ps[i].gunNum;
            player[i].GetComponent<PlayerShoot>().imShooting = Ps[i].shot;
            //  v2 = P2_S.v;


        }
        //  P1_S.name = Player1.GetComponentInChildren<TextMeshPro>().text;
        //  P1_S.position = Player1.transform.position;
        //  P1_S.rotation = Player1.transform.rotation;
        //  P1_S.alive = Player1.GetComponent<HpHandler>().alive;
        //  P1_S.gunNum = Player1.GetComponent<PlayerShoot>().gunType;
        //  P1_S.shot = Player1.GetComponent<PlayerShoot>().imShooting;
        //  P1_S.v = Player1.GetComponent<PlayerShoot>().shootDirection;
        //  P1_S.shield = Player1.GetComponent<Shield>().shieldActive;
        //  Player2.GetComponent<Shield>().enabled = false;
        //
        //  Player2.GetComponentInChildren<TextMeshPro>().text = P2_S.name;
        //  Player2.transform.position = P2_S.position;
        //  Player2.transform.rotation = P2_S.rotation;
        //  Player2.GetComponent<Collider>().enabled = P2_S.alive;
        //  Player2.GetComponent<MeshRenderer>().enabled = P2_S.alive;
        //  Player2.GetComponent<PlayerMovment>().enabled = P2_S.alive;
        //  Player2.GetComponent<Shield>().shield.SetActive(P2_S.shield);
        //  Player2.GetComponent<PlayerShoot>().enabled = false;
        //  Player2.GetComponent<PlayerShoot>().gunType = P2_S.gunNum;
        //  Player2.GetComponent<PlayerShoot>().imShooting = P2_S.shot;
        //  v2 = P2_S.v;

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
                 string P_Info = JsonUtility.ToJson(Ps[0]);
                 byte[] data = Encoding.ASCII.GetBytes(P_Info);
                 //Si es fa pause Funciona, Sino peta ns PK
                
                 for (int i = 0; i < _info.numberOfPlayers; i++)
                 {
                     if (_info.ep[i] != null)
                     {
                         _info.sock.SendTo(data, data.Length, SocketFlags.None, _info.ep[i]);
                     }
                
                 }
            }
            else if (_info.im_Client == true)
            {
                //Aixo tambe esta Xungo
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
                for (int i = 1; i < _info.numberOfPlayers; i++)
                {
                    recv[i] = _info.sock.ReceiveFrom(data, ref _info.ep[i]);
                    p_info[i] = Encoding.ASCII.GetString(data, 0, recv[i]);
                    Ps[i] = JsonUtility.FromJson<Player_Info>(p_info[i]);
                }

            }
            else if (_info.im_Client == true)
            {
                byte[] data = new byte[1024];
                int recvC = _info.sock.ReceiveFrom(data, ref _info.serverEp);
                string p_infoC = Encoding.ASCII.GetString(data, 0, recvC);
                for (int i = 1; i < _info.numberOfPlayers; i++)
                {
                    Ps[i] = JsonUtility.FromJson<Player_Info>(p_infoC);
                }
 
            }
        }
    }

    int GetPlayerNumber(EndPoint ep)
    {
        int playerid = 0;
        for (int i = 0; i < _info.numberOfPlayers; i++)
        {
            if (_info.ep[i] == ep)
            {
                i = playerid;
                return i;
            }
        }
        return playerid;
    }

    void Setplayers(int numberPlayer)
    {

        for (int i = 0; i <= _info.numberOfPlayers; i++)
        {

            player[i] = GameObject.Find("Player" + numberPlayer);
            numberPlayer++;
            if (i != 0)
            {
                player[i].GetComponent<PlayerMovment>().enabled = false;
                player[i].GetComponent<PlayerShoot>().enabled = false;
            }
            if (numberPlayer > _info.numberOfPlayers)
            {
               
                numberPlayer = i;
            }

        }

        //Player1.GetComponentInChildren<TextMeshPro>().text = _info.name;



        //if (_info.type == 1)
        // {
        //     Instantiate(Camera, new Vector3(0, 0, -4), Quaternion.identity);
        //     Player1 = GameObject.Find("Player1");
        //     Player2 = GameObject.Find("Player0");
        //     Player2.GetComponentInChildren<TextMeshPro>().text = _info.name;
        // }
    }
}
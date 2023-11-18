//using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
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
    Server_Info S;
    Player_Info P1;
    // Start is called before the first frame update
    void Start()
    {
        S = GameObject.Find("Perma_server").GetComponent<Server_Info>(); 
    }
  
    // Update is called once per frame
    void Update()
    {
        if (S.type == 0) //0 server 
        {
            SendInfo(P1, S.sock, S.ep);
            Debug.Log(S.type);
        }
        else if (S.type == 1) //1 player
        {
          // ReciveInfo();
        }
    }
  
    void SendInfo(Player_Info Player, Socket sock, EndPoint remote)
    {
        byte[] data = new byte[1024];
        string P_Info = JsonUtility.ToJson(Player);
        data = Encoding.ASCII.GetBytes(P_Info);
        Debug.Log(data + "holaaaa");
        sock.SendTo(data, data.Length, SocketFlags.None,remote);
       
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

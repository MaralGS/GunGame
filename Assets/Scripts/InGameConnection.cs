using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.tvOS;

public class InGameConnection : MonoBehaviour
{
    struct Player_Info 
    {
        public string Name;
        public Vector2 Position;
        public bool shoot;
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
        sock.SendTo(data, data.Length, SocketFlags.None,remote);
       // Debug.Log(data);
    }
  
    void ReciveInfo(byte[] data, Socket Server, EndPoint remote)
    {
        int recv = Server.ReceiveFrom(data, ref remote);
        string P_Info = Encoding.ASCII.GetString(data, 0, recv);
        P1 = JsonUtility.FromJson<Player_Info>(P_Info);
    }
    
}

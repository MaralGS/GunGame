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
    Server S;
    Player_Info P1;
    Socket _socket;
    EndPoint _endPoint;
    // Start is called before the first frame update
    void Start()
    {
        S = GameObject.Find("Server_UDP").GetComponent<Server>(); 
    }

    // Update is called once per frame
    void Update()
    {
        SendInfo(P1,S.newsock,S.Remote);
        //ReciveInfo();
    }

    void SendInfo(Player_Info Player, Socket sock, EndPoint remote)
    {
        byte[] data = new byte[1024];
        string P_Info = JsonUtility.ToJson(Player);
        data = Encoding.ASCII.GetBytes(P_Info);
        sock.SendTo(data, data.Length, SocketFlags.None,remote);
        Debug.Log(data +" HHAAAAAAAAAAAA");
    }

    void ReciveInfo(byte[] data, Socket Server, EndPoint remote)
    {
        int recv = Server.ReceiveFrom(data, ref remote);
        string P_Info = Encoding.ASCII.GetString(data, 0, recv);
        P1 = JsonUtility.FromJson<Player_Info>(P_Info);
    }
    
}

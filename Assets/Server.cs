using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
C# Network Programming 
by Richard Blum

Publisher: Sybex 
ISBN: 0782141765
*/
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.WebSockets;
using UnityEditor.PackageManager;

public class Server : MonoBehaviour
{
    public static void Main()
    {
        int recv;
        byte[] data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);

        Socket newsock = new Socket(AddressFamily.InterNetwork,
                        SocketType.Dgram, ProtocolType.Udp);

        newsock.Bind(ipep);

        newsock.Listen(10);
        Socket client = newsock.Accept();
        IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
        newsock.Connect(ipep);

        Debug.Log("Waiting for a client...");

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)(sender);

        recv = newsock.ReceiveFrom(data, ref Remote);

        Debug.Log("Message received from:" + Remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));

        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
        while (true)
        {
            data = new byte[1024];
            recv = newsock.ReceiveFrom(data, ref Remote);

            Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
            newsock.SendTo(data, recv, SocketFlags.None, Remote);
        }
    }
}
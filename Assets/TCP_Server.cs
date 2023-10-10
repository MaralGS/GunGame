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

public class TCP_Server : MonoBehaviour
{
    public static void start()
    {
        int recv;
        byte[] data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("10.0.103.37"),
                               9050);

        Socket newsock = new
            Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

        newsock.Bind(ipep);
        newsock.Listen(10);
        Console.WriteLine("Waiting for a client...");
        Socket client = newsock.Accept();
        IPEndPoint clientep =
                     (IPEndPoint)client.RemoteEndPoint;
        Console.WriteLine("Connected with {0} at port {1}",
                        clientep.Address, clientep.Port);


        string welcome = "Welcome to my test server";
        data = Encoding.ASCII.GetBytes(welcome);
        client.Send(data, data.Length,
                          SocketFlags.None);
        while (true)
        {
            data = new byte[1024];
            recv = client.Receive(data);
            if (recv == 0)
                break;

            Console.WriteLine(
                     Encoding.ASCII.GetString(data, 0, recv));
            client.Send(data, recv, SocketFlags.None);
        }
        Console.WriteLine("Disconnected from {0}",
                          clientep.Address);
        client.Close();
        newsock.Close();
    }
}

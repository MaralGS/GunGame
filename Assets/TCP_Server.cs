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
using System.Threading;

public class TCP_Server : MonoBehaviour
{

    int recv;
    byte[] data = new byte[1024];
    public int port = 0;
    private IPEndPoint ipep;
    private Socket newsock;
    private Socket client;
    private Thread myThreadTCP;
    public int connections = 10;
    public String WelcomeText = "";

    void Start()
    {

        ipep = new IPEndPoint(IPAddress.Any, port);

        Listen(connections);

        Accept(newsock, client);

        myThreadTCP = new Thread(TCPServer);
        myThreadTCP.Start();
    }

    private void Update()
    {

    }


    void Listen(int Connexions)
    {
        newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        newsock.Bind(ipep);

        newsock.Listen(Connexions);
    } 

    void Accept(Socket newSocket, Socket sclient) {
        Debug.Log("Waiting for a client...");
        sclient = newSocket.Accept();
    }

    void TCPServer()
    {

        IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
        newsock.Connect(clientep);
        Debug.Log("Connected with {0} at port {1}" + " " + clientep.Address + " " + clientep.Port);

        data = Encoding.ASCII.GetBytes(WelcomeText);
        client.Send(data, data.Length, SocketFlags.None);

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
       Debug.Log("Disconnected from {0}" + clientep.Address);
        client.Close();
        newsock.Close();
    }
}



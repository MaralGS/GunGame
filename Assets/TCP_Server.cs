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
    private IPEndPoint ipep;
    private Socket newsock;
    TCP_Client client;
    private Thread myThreadTCP;
    public int connections = 10;
    public String WelcomeText = "";
    IPEndPoint clientep;

    void Start()
    {

        ipep = new IPEndPoint(IPAddress.Any, 9050);

        newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        newsock.Bind(ipep);

        myThreadTCP = new Thread(TCPServer);
        myThreadTCP.Start();
    }

    void TCPServer()
    {
        try
        {
            newsock.Listen(connections);
            Debug.Log("Waiting for a client...");
            client.ClientS = newsock.Accept();

            clientep = (IPEndPoint)client.ClientS.RemoteEndPoint;
            Debug.Log("Connected with {0} at port {1}" + " " + clientep.Address + " " + clientep.Port);

        }
        catch (Exception)   
        {
            Debug.Log("Connected failed... try again...");
            throw;
        }
         
        data = Encoding.ASCII.GetBytes(WelcomeText);
        client.ClientS.Send(data, data.Length, SocketFlags.None);

               
        Debug.Log("Disconnected from {0}" + clientep.Address);
        client.ClientS.Close();
        newsock.Close();
    }
}



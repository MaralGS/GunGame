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

public class TCP_Client : MonoBehaviour
{
    byte[] data = new byte[1024];
    [SerializeField] string ip = "127.0.0.1";
   void Start()
    {
    
        string input, stringData;
        IPEndPoint ipep = new IPEndPoint(
                        IPAddress.Parse(ip), 9050);

        Socket server = new Socket(AddressFamily.InterNetwork,
                       SocketType.Stream, ProtocolType.Tcp);

        try
        {
            server.Connect(ipep);
        }
        catch (SocketException e)
        {
            Console.WriteLine("Unable to connect to server.");
            Console.WriteLine(e.ToString());
            return;
        }


        int recv = server.Receive(data);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Console.WriteLine(stringData);

    
       input = Console.ReadLine();

       server.Send(Encoding.ASCII.GetBytes(input));
       data = new byte[1024];
       recv = server.Receive(data);
       stringData = Encoding.ASCII.GetString(data, 0, recv);
       Console.WriteLine(stringData);
 
        Console.WriteLine("Disconnecting from server...");
        server.Shutdown(SocketShutdown.Both);
        server.Close();
    }
}
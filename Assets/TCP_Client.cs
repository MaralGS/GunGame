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
using UnityEditor.PackageManager;

public class TCP_Client : MonoBehaviour
{
    byte[] data = new byte[1024];
    [SerializeField] string ip = "10.0.103.26";
    public Socket ClientS;

    private void Start()
    {
        string input, stringData;
        IPEndPoint ipep = new IPEndPoint(
                        IPAddress.Parse(ip), 9050);

        ClientS = new Socket(AddressFamily.InterNetwork,
                       SocketType.Stream, ProtocolType.Tcp);
        try
        {   
            ClientS.Connect(ipep);
        }
        catch (Exception e)
        {
            Debug.Log("Unable to connect to server.");
            Console.WriteLine(e.ToString());
         
        }
 

        int recv = ClientS.Receive(data);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Console.WriteLine(stringData);


        input = Console.ReadLine();

        ClientS.Send(Encoding.ASCII.GetBytes(input));
        data = new byte[1024];
        recv = ClientS.Receive(data);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Console.WriteLine(stringData);

        Console.WriteLine("Disconnecting from server...");
        ClientS.Shutdown(SocketShutdown.Both);
        ClientS.Close();
    }
}
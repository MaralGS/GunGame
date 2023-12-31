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
//using UnityEditor.PackageManager;
using TMPro;

public class TCP_Client : MonoBehaviour
{
    byte[] data = new byte[1024];
    [SerializeField] string ip = "127.0.0.1";
    public Socket ClientS;
    public GameObject TextName;
    string UserName;
    public void ConnectPlayer()
    {
        UserName = TextName.GetComponent<TMP_InputField>().text;

        string stringData;
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
            Debug.Log(e.ToString());

        }

        string welcome = "Hello, are you there?, I'm " + UserName;
        data = Encoding.ASCII.GetBytes(welcome);
        ClientS.Send(data, data.Length, SocketFlags.None);

        data = new byte[1024];
        int recv = ClientS.Receive(data);
        stringData = Encoding.ASCII.GetString(data, 0, recv);
        Debug.Log(stringData);

        Debug.Log("Disconnecting from server...");
        ClientS.Shutdown(SocketShutdown.Both);
        ClientS.Close();
    }

    public void Check()
    {
        
    }

}
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
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;
using UnityEngine.tvOS;

public class Client : MonoBehaviour
{
    byte[] data = new byte[1024];
    public GameObject ip;
    public GameObject TextName;
    string ServerM;
    string usingIP;
    Server_Info info;

    Socket client;
    IPEndPoint ipep;
    EndPoint remote;
    void Start()
    {

       
    }
    public void ConnectPlayer()
    {

        //userName = TextName.GetComponent<TMP_InputField>().text;
        usingIP = ip.GetComponent<TMP_InputField>().text;

        ipep = new IPEndPoint(
                       IPAddress.Parse(usingIP), 9050);

        client = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);

        try
        {
            string welcome = "Connected";
            data = Encoding.ASCII.GetBytes(welcome);
            client.SendTo(data, data.Length, SocketFlags.None, ipep);
        }
        catch (Exception e)
        {
            Debug.Log("Unable to connect to server.");
            Debug.Log(e.ToString());

        }


        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        remote = (EndPoint)sender;

        data = new byte[1024];
        int recv = client.ReceiveFrom(data, ref remote);

        Debug.Log("Message received from:" + remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));
        ServerM = Encoding.ASCII.GetString(data, 0, recv);

        TryConnection();

        

        //Debug.Log("Stopping client");
        //server.Close();
    }

    void TryConnection()
    {
       if(ServerM == "StartServer")
        {
            info.SaveInfo(client, remote, 1);
            SceneManager.LoadScene(2);
        }
    }
}

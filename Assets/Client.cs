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

public class Client : MonoBehaviour
{
    byte[] data = new byte[1024];
    public GameObject ip;
    public GameObject TextName;
    string userName;
    string usingIP;
    Server_Info info;
    void Start()
    {

       
    }
    public void ConnectPlayer()
    {

        userName = TextName.GetComponent<TMP_InputField>().text;
        usingIP = ip.GetComponent<TMP_InputField>().text;

        IPEndPoint ipep = new IPEndPoint(
                       IPAddress.Parse(usingIP), 9050);

        Socket client = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);

        try
        {
            string welcome = "Hello, are you there?, I'm " + userName;
            data = Encoding.ASCII.GetBytes(welcome);
            client.SendTo(data, data.Length, SocketFlags.None, ipep);
        }
        catch (Exception e)
        {
            Debug.Log("Unable to connect to server.");
            Debug.Log(e.ToString());

        }


        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)sender;

        data = new byte[1024];
        int recv = client.ReceiveFrom(data, ref remote);

        Debug.Log("Message received from:" + remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));


        //info.SaveInfo(client, remote, true);

        SceneManager.LoadScene(2);

        //Debug.Log("Stopping client");
        //server.Close();
    }
}

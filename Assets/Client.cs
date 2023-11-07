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

public class Client : MonoBehaviour
{
    byte[] data = new byte[1024];
    public GameObject ip;
    public GameObject TextName;
    string userName;
    string usingIP;

    void Start()
    {

       
    }
    public void ConnectPlayer()
    {

        userName = TextName.GetComponent<TMP_InputField>().text;
        usingIP = ip.GetComponent<TMP_InputField>().text;

        IPEndPoint ipep = new IPEndPoint(
                       IPAddress.Parse(usingIP), 9050);

        Socket server = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);


        string welcome = "Hello, are you there?, I'm " + userName;
        data = Encoding.ASCII.GetBytes(welcome);
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        EndPoint Remote = (EndPoint)sender;

        data = new byte[1024];
        int recv = server.ReceiveFrom(data, ref Remote);

        Debug.Log("Message received from:" + Remote.ToString());
        Debug.Log(Encoding.ASCII.GetString(data, 0, recv));



        SceneManager.LoadScene(2);

        //Debug.Log("Stopping client");
        //server.Close();
    }
}

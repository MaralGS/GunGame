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
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class TCP_Server : MonoBehaviour
{

    int recv;
    byte[] data = new byte[1024];
    public int port = 9050;
    private IPEndPoint ipep;
    private Socket newsock;
    private Thread myThreadTCP;
    public int connections = 10;
    public String WelcomeText = "";
    public Socket client;
    public IPEndPoint clientep;
    public GameObject TextName;
    string UserName;


    void Start()
    {

      
    }


    public void StartServer()
    {

        UserName = TextName.GetComponent<TMP_InputField>().text;

        ipep = new IPEndPoint(IPAddress.Any, port);

        newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        newsock.Bind(ipep);

        myThreadTCP = new Thread(TCPServer);
        myThreadTCP.Start();

        SceneManager.LoadScene(0);
    } 

    void TCPServer()
    {

            try
            {
                string stringData;
                newsock.Listen(connections);
                Debug.Log("Waiting for a client...");
                client = newsock.Accept();

                clientep = (IPEndPoint)client.RemoteEndPoint;
                Debug.Log("Connected with {0} at port {1}" + " " + clientep.Address + " " + clientep.Port);

                data = new byte[1024];
                int recv = client.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Debug.Log(stringData);

        }
            catch (Exception)
            {
                Debug.Log("Connected failed... try again...");
                throw;
            }
            Debug.Log("Disconnected from {0}" + clientep.Address);
            data = Encoding.ASCII.GetBytes(WelcomeText + " " + UserName);
            client.Send(data, data.Length, SocketFlags.None);
            
            
   
        client.Close();
        newsock.Close();

    }
}



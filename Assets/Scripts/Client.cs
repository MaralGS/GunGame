using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
//using UnityEditor.PackageManager;
//using UnityEngine.tvOS;

public class Client : MonoBehaviour
{

    public static Client _instance;

    public static Client Instanace => _instance;

    byte[] data = new byte[1024];
    public GameObject ip;
    public GameObject TextName;
    string ServerM;
    string usingIP;
    Server_Info info;

    Socket client;
    IPEndPoint ipep;
    EndPoint remote;
    [HideInInspector] public string type = "Client";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        info = GameObject.Find("Perma_server").GetComponent<Server_Info>();
    }

    private void Update()
    {
        if (ServerM == "StartServer")
        {
            info.SaveInfo(client, remote, 1);
            SceneManager.LoadScene(1);
        }
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

        //Debug.Log("Stopping client");
        //server.Close();
    }
}

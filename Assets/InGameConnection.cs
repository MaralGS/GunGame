using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class InGameConnection : MonoBehaviour
{
    struct Player_Info 
    {
        public string Name;
        public Vector2 Position;
        public bool shoot;
    }
    Player_Info P1;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        SendInfo(P1);
    }

    void SendInfo(Player_Info Player)
    {
        byte[] data = new byte[1024];
        string P_Info = JsonUtility.ToJson(Player);
        data = Encoding.ASCII.GetBytes(P_Info);
        
    }
    
}

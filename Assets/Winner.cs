using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public Server_Info S_info;


    // Start is called before the first frame update
    void Start()
    {
        S_info = FindObjectOfType<Server_Info>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = S_info?.winner;
    }
}

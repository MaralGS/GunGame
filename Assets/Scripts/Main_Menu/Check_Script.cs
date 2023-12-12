using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Check_Script : MonoBehaviour
{

    Color green = new Color(0, 0.741f, 0);
    Color white = new Color(255,255,255);
    bool check = false;
    public void ChangeColorCheck()
    {
        check =! check;
        gameObject.GetComponentInChildren<Image>().color = get_color();
        Debug.Log("CHANGEEE");
    }
    Color get_color()
    {
        return check ?  green : green;
    }
}

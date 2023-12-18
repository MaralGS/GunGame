using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update

    public bool shieldActive = false;
    public float shieldTimer = 0.0f;
    public float shieldCooldown = 0.0f;
    public GameObject shield;
    void Start()
    {
        shield.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.S) && shieldCooldown >= 1.0f)
        {
            shieldActive = true;
            shield.SetActive(true);

        }

        if (shieldActive == true)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer > 1.0f)
            {
                shieldTimer = 0.0f;
                shieldActive = false;
                shield.SetActive(false);

                shieldCooldown = 0.0f;
            }
        }
        if (shieldActive == false)
        {
            shieldCooldown += Time.deltaTime;
        }

    }
}

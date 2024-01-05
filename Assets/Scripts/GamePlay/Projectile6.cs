using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile6 : MonoBehaviour
{
    public float projectileLifeTime = 1.0f;
    private float currentTimer = 0.0f;
    public int pID;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        currentTimer += Time.deltaTime;
        if (currentTimer > projectileLifeTime)
        {
            Destroy(gameObject);
        }



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HpHandler>().hp -= 1;
            if (collision.gameObject.GetComponent<HpHandler>().hp <= 0)
            {
                GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().player[pID - 1].GetComponent<PlayerShoot>().gunType += 1;
            }
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile4 : MonoBehaviour
{
    public float projectileLifeTime = 2.5f;
    private float currentTimer = 0.0f;
    private float stuckTimer = 0.0f;
    public int pID;
    public bool stuck = false;
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

        if(stuck == true)
        {
            stuckTimer += Time.deltaTime;
            if(stuckTimer < 2.0f)
            {
                gameObject.transform.localScale += Vector3.one * (Time.deltaTime * 0.5f);
            }
            else
            {
                Destroy(gameObject );
                stuckTimer = 0.0f;
                stuck = false;

            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
            stuckTimer = 0;
            stuck = false;
        }
        else if (collision.gameObject.CompareTag("Ground") && stuck == false)
        {
            stuck = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if(stuck == true)
            {
                collision.gameObject.GetComponent<HpHandler>().hp -= 10;
            }
            else
            {
                collision.gameObject.GetComponent<HpHandler>().hp -= 2;
            }
            if (collision.gameObject.GetComponent<HpHandler>().hp <= 0)
            {
                if (pID == 0)
                {
                    GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().player[pID].GetComponent<PlayerShoot>().gunType += 1;
                }
                else
                {
                    GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().player[pID - 1].GetComponent<PlayerShoot>().gunType += 1;
                }

            }
            Destroy(gameObject);
            stuckTimer = 0;
            stuck = false;

        }
    }
}

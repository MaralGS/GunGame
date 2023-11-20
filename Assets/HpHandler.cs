using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HpHandler : MonoBehaviour
{
    public int hp = 10;
    public float dieTimer = 0.0f;
    public float deathTime = 3.0f;
    public GameObject respawnPosition1;
    public GameObject respawnPosition2;
    public GameObject respawnPosition3;
    public GameObject respawnPosition4;
    int randomInt;
    public GameObject enemy;
    InGameConnection playerInfo;

    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Die();
            if (dieTimer > deathTime)
            {
                Respawn();
            }
        }

    }

    private void ChangeEnemyWeapon()
    {
        enemy.GetComponent<PlayerShoot>().gunType += 1;
    }

    private void Respawn()
    {
        gameObject.GetComponent<PlayerMovment>().enabled = true;
        gameObject.GetComponent<PlayerShoot>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
        alive = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;

        dieTimer = 0.0f;
        hp = 10;
        randomInt = UnityEngine.Random.Range(1, 3 + 1);
        switch(randomInt)
        {
            case 1:
                gameObject.transform.position = respawnPosition1.transform.position;
                break;
            case 2:
                gameObject.transform.position = respawnPosition2.transform.position;
                break;

            case 3:
                gameObject.transform.position = respawnPosition3.transform.position;
                break;

            case 4:
                gameObject.transform.position = respawnPosition4.transform.position;
                break;

            default:
                gameObject.transform.position = respawnPosition1.transform.position;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile1"))
        {
            hp -= 5;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Projectile2"))
        {
            hp -= 10;
            Destroy(collision.gameObject);
        }
    }

    private void Die()
    {
        gameObject.GetComponent<PlayerMovment>().enabled = false;
        gameObject.GetComponent<PlayerShoot>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        alive = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        dieTimer += Time.deltaTime;
        ChangeEnemyWeapon();

    }
}

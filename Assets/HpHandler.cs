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

    public bool alive = true;
    public List<Vector3> respawnPositions;
    public bool enemyAlive = true;
    public int whichRespawn = 0;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(respawnPosition1);
        Instantiate(respawnPosition2);
        Instantiate(respawnPosition3);
        Instantiate(respawnPosition4);
        respawnPositions = new List<Vector3>
        {
            respawnPosition1.transform.position,
            respawnPosition2.transform.position,
            respawnPosition3.transform.position,
            respawnPosition4.transform.position
        };
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

        if(enemy.GetComponent<HpHandler>().hp <= 0)
        {
            enemyAlive = false;
        }

        if(gameObject.transform.position.y < -100.0f)
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
        
        float mayorDistance = 0.0f;
        
        for(int i = 0; i < 4; i++) {
            float calculatedPosition = Mathf.Abs(Vector3.Distance(enemy.transform.position, respawnPositions[i]));
            if (calculatedPosition > mayorDistance)
            {
                mayorDistance = calculatedPosition;
                whichRespawn = i;
            }
        }

        gameObject.transform.position = respawnPositions[whichRespawn];
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.gameObject.CompareTag("Projectile1"))
        {
            hp -= 5;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Projectile2"))
        {
            hp -= 10;
            Destroy(collision.gameObject);
        }/*/
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile5 : MonoBehaviour
{
    public float projectileLifeTime = 1.0f;
    private float currentTimer = 0.0f;
    public int pID;
    public bool destination = false;
    public GameObject player;
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
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            MoveTowardsTarget();
        }
        if(player.transform.position.x > 900.0f)
        {
            Destroy(gameObject);
        }
    }
    private void MoveTowardsTarget()
    {
        // Calculate the direction to the target
        Vector2 direction = (player.transform.position - transform.position);

        // Move towards the target position
        transform.Translate(direction * Time.deltaTime);


        if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
        {
            // Arrived at the target position
            Destroy(gameObject);
            // You can perform additional actions or trigger events here
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {

            Destroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.GetComponent<HpHandler>().hp -= 5;
            
            if (collision.gameObject.GetComponent<HpHandler>().hp <= 0)
            {
               
                    GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().player[pID-1].GetComponent<PlayerShoot>().gunType += 1;
                
            }
            Destroy(gameObject);


        }
    }
}

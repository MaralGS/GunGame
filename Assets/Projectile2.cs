using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    public float projectileLifeTime = 5.5f;
    private float currentTimer = 0.0f;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
                collision.gameObject.transform.position += new Vector3(100, 0, 0);
                Debug.Log("Hit with projectile");

            
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        
    }

}

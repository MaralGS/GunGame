using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectilePrefab1;
    public GameObject projectilePrefab2;
    public GameObject projectilePrefab3;
    public float projectileSpeed = 10f;
    public Camera cam;
    public int gunType = 0;
    public float coolDownTimer = 0.0f;
    public float coolDown = 0.2f;
    public bool activeCoolDown = false;
    public float coolDown1 = 5.5f;
    public bool activeCoolDown1 = false;
    public float coolDown2 = 1.65f;
    public bool activeCoolDown2 = false;
    public float coolDown3 = 2.5f;
    public bool activeCoolDown3 = false;
    public bool imShooting = false;
    public Vector3 shootDirection;
    InGameConnection player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert screen coordinates to world coordinates
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            worldMousePosition.z = 0;
            // Spawn projectile at player position

            // Calculate direction towards the mouse position
            shootDirection = (worldMousePosition - transform.position).normalized;
            
            Shoot(shootDirection, transform.position, gunType, player._info.clientID, false);

            if (gameObject.transform.position.x > worldMousePosition.x)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                Debug.Log("Left");
                Debug.Log(worldMousePosition.x);
                Debug.Log(gameObject.transform.position.x);
            }
            else
            {
                Debug.Log("Right");
                Debug.Log(worldMousePosition.x);
                Debug.Log(gameObject.transform.position.x);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

        }
        else
        {
            imShooting = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<Animator>().SetBool("attack", false);
        }

        // if (player._info.type == 1 && player.P1_S.shot == true)
        // {
        //
        //     Shoot(GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().v2, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.position, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.gunNum);
        //     imShooting = false;
        //     player.P2_S.shot = false;
        //
        // }
        // if (player._info.type == 0 && player.P2_S.shot == true)
        // {
        //
        //     Shoot(GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().v2, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.position, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.gunNum);
        //     
        //     activeCoolDown = true;
        //     imShooting = false;
        //
        //     player.P1_S.shot = false;
        // }


        if (activeCoolDown)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDown)
            {
                activeCoolDown = false;
                coolDownTimer = 0.0f;
            }
        }
        if (activeCoolDown1)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDown1)
            {
                activeCoolDown1 = false;
                coolDownTimer = 0.0f;
            }
        }
        if (activeCoolDown2)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDown2)
            {
                activeCoolDown2 = false;
                coolDownTimer = 0.0f;
            }
        }
        if (activeCoolDown3)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDown3)
            {
                activeCoolDown3 = false;
                coolDownTimer = 0.0f;
            }
        }
    }

    public void Shoot(Vector3 shootDirection, Vector3 go, int gunType, int playerID, bool enemy)
    {
        gameObject.GetComponent<Animator>().SetBool("attack", true);
       


        switch (gunType)
        {
            case 0:
                if (activeCoolDown == false || enemy == true)
                {
                    imShooting = true;

                    GameObject projectile = Instantiate(projectilePrefab, (go + shootDirection), Quaternion.identity);
                    projectile.GetComponent<Projectile1>().pID = playerID;
                    // Apply force to the projectile in the shoot direction
                    projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed;
                    activeCoolDown = true;
                }
                
                break;
            case 1:
                if (activeCoolDown1 == false || enemy == true)
                {
                    imShooting = true;

                    GameObject projectile1 = Instantiate(projectilePrefab1, (go + shootDirection), Quaternion.identity);

                    // Apply force to the projectile in the shoot direction
                    projectile1.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed / 3;
                    activeCoolDown1 = true;
                }
                break;
            case 2:
                if (activeCoolDown2 == false || enemy == true)
                {
                    imShooting = true;

                    GameObject projectile2_0 = Instantiate(projectilePrefab2, (go + shootDirection), Quaternion.identity);
                    GameObject projectile2_1 = Instantiate(projectilePrefab2, (go + shootDirection), Quaternion.identity);
                    GameObject projectile2_2 = Instantiate(projectilePrefab2, (go + shootDirection), Quaternion.identity);
                    GameObject projectile2_3 = Instantiate(projectilePrefab2, (go + shootDirection), Quaternion.identity);
                    GameObject projectile2_4 = Instantiate(projectilePrefab2, (go + shootDirection), Quaternion.identity);
                    projectile2_0.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                    projectile2_1.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x - 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                    projectile2_2.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y + 0.3f, 0f) * projectileSpeed / 3;
                    projectile2_3.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y - 0.3f, 0f) * projectileSpeed / 3;
                    projectile2_4.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.25f, shootDirection.y + 0.25f, 0f) * projectileSpeed / 3;
                    activeCoolDown2 = true;
                }
                    
                break;
            default:

                if (activeCoolDown3 == false || enemy == true)
                {
                    imShooting = true;

                    GameObject projectile3 = Instantiate(projectilePrefab3, (go + shootDirection), Quaternion.identity);
                    projectile3.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed;
                    activeCoolDown3 = true;
                }
                
                break;
        }
        
    }
}

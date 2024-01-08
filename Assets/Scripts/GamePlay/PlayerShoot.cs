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
    public GameObject projectilePrefab4;
    public GameObject projectilePrefab5;
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
    public float coolDown4 = 3.5f;
    public bool activeCoolDown4 = false;
    public float coolDown5 = 0.0f;
    public bool activeCoolDown5 = false;
    public bool imShooting = false;
    public Vector3 shootDirection;
    InGameConnection player;
    private string isLookingAt;
    public float messageInterval = 0.0f;
    [HideInInspector] public bool endGame = false;// Set the interval in seconds

    private float lastMessageTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && gunType == 5)
        {
            if (Time.time - lastMessageTime > messageInterval)
            {
                Vector3 mousePosition = Input.mousePosition;

                // Convert screen coordinates to world coordinates
                Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

                worldMousePosition.z = 0;
                // Spawn projectile at player position

                // Calculate direction towards the mouse position
                if (gameObject.transform.position.x > worldMousePosition.x)
                {
                    Shoot(Vector3.left, transform.position, gunType, player._info.clientID, false);
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    isLookingAt = "left";
                }
                if (gameObject.transform.position.x < worldMousePosition.x)
                {
                    Shoot(Vector3.right, transform.position, gunType, player._info.clientID, false);
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    isLookingAt = "right";
                }

                lastMessageTime = Time.time; // Update the last message time
            }
        }
        else if (Input.GetMouseButtonDown(0) && gunType != 5)
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

        }
        else
        {
            imShooting = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<Animator>().SetBool("attack", false);
        }

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
        if (activeCoolDown4)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDown4)
            {
                activeCoolDown4 = false;
                coolDownTimer = 0.0f;
            }
        }
        if (activeCoolDown5)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDown5)
            {
                activeCoolDown5 = false;
                coolDownTimer = 0.0f;
            }
        }
        if(gunType > 0)
        {
            endGame = true;
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
                    projectile1.GetComponent<Projectile2>().pID = playerID;

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
                    projectile2_0.GetComponent<Projectile3>().pID = playerID;

                    projectile2_1.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x - 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                    projectile2_1.GetComponent<Projectile3>().pID = playerID;

                    projectile2_2.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y + 0.3f, 0f) * projectileSpeed / 3;
                    projectile2_2.GetComponent<Projectile3>().pID = playerID;

                    projectile2_3.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y - 0.3f, 0f) * projectileSpeed / 3;
                    projectile2_3.GetComponent<Projectile3>().pID = playerID;

                    projectile2_4.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.25f, shootDirection.y + 0.25f, 0f) * projectileSpeed / 3;
                    projectile2_4.GetComponent<Projectile3>().pID = playerID;

                    activeCoolDown2 = true;
                }
                    
                break;
            

            case 3:

                if (activeCoolDown3 == false || enemy == true)
                {
                    imShooting = true;

                    GameObject projectile3 = Instantiate(projectilePrefab3, (go + shootDirection), Quaternion.identity);
                    projectile3.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed;
                    projectile3.GetComponent<Projectile4>().pID = playerID;

                    activeCoolDown3 = true;
                }
                
                break;
            case 4:
                if (activeCoolDown4 == false || enemy == true)
                {
                    imShooting = true;
                    GameObject projectile4 = Instantiate(projectilePrefab4, (go + shootDirection), Quaternion.identity);

                    projectile4.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                    projectile4.GetComponent<Projectile5>().pID = playerID;
                    projectile4.GetComponent<Projectile5>().player = gameObject;
                    activeCoolDown4 = true;
                }
                break;
            default:

                if (activeCoolDown4 == false || enemy == true)
                {
                    imShooting = true;
                    
                    if(isLookingAt == "right")
                    {
                        GameObject projectile5 = Instantiate(projectilePrefab5, (go + shootDirection), Quaternion.identity);
                        shootDirection.y -= 0.1f;
                        shootDirection.x -= 0.1f;
                        GameObject projectile5_0 = Instantiate(projectilePrefab5, (go + shootDirection), Quaternion.identity);
                        shootDirection.y += 0.1f;
                        shootDirection.x -= 0.1f;
                        GameObject projectile5_1 = Instantiate(projectilePrefab5, (go + shootDirection), Quaternion.identity);

                        projectile5.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                        projectile5.GetComponent<Projectile6>().pID = playerID;
                        projectile5_0.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                        projectile5_0.GetComponent<Projectile6>().pID = playerID;
                        projectile5_1.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 3;
                        projectile5_1.GetComponent<Projectile6>().pID = playerID;
                        transform.Translate(Vector2.left*0.1f);
                    }
                    else if(isLookingAt == "left")
                    {
                        GameObject projectile5 = Instantiate(projectilePrefab5, (go + shootDirection), Quaternion.identity);
                        shootDirection.y += 0.1f;
                        shootDirection.x += 0.1f;
                        GameObject projectile5_0 = Instantiate(projectilePrefab5, (go + shootDirection), Quaternion.identity);
                        shootDirection.y -= 0.1f;
                        shootDirection.x += 0.1f;
                        GameObject projectile5_1 = Instantiate(projectilePrefab5, (go + shootDirection), Quaternion.identity);

                        projectile5.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 1.5f;
                        projectile5.GetComponent<Projectile6>().pID = playerID;
                        projectile5_0.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 1.5f;
                        projectile5_0.GetComponent<Projectile6>().pID = playerID;
                        projectile5_1.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDirection.x + 0.3f, shootDirection.y, 0f) * projectileSpeed / 1.5f;
                        projectile5_1.GetComponent<Projectile6>().pID = playerID;
                        transform.Translate(Vector2.right*0.1f);

                    }
                }

                break;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject projectilePrefab1;
    public float projectileSpeed = 10f;
    public Camera cam;
    public int gunType = 0;
    public float coolDownTimer = 0.0f;
    public float coolDown = 0.2f;
    public bool activeCoolDown = false;
    public float coolDown1 = 5.5f;
    public bool activeCoolDown1 = false;
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
            imShooting=true;
            // Get mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert screen coordinates to world coordinates
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            worldMousePosition.z = 0;
            // Spawn projectile at player position

            // Calculate direction towards the mouse position
            shootDirection = (worldMousePosition - transform.position).normalized;

            Shoot(shootDirection, transform.position, gunType);
    
            
        }
        else
        {
            imShooting = false;
        }
        

        if (player._info.type == 1 && player.P2_S.shot == true)
        {

            Shoot(GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().v2, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.position, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.gunNum);
            imShooting = false;
            player.P2_S.shot = false;

        }
        if (player._info.type == 0 && player.P2_S.shot == true)
        {

            Shoot(GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().v2, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.position, GameObject.Find("Serialization").gameObject.GetComponent<InGameConnection>().P2_S.gunNum);
            
            activeCoolDown = true;
            imShooting = false;

            player.P1_S.shot = false;
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
    }

    private void Shoot(Vector3 shootDirection, Vector3 go, int gunType)
    {
        switch (gunType)
        {
            case 0:
                if (activeCoolDown == false)
                {
                    GameObject projectile = Instantiate(projectilePrefab, (go + shootDirection), Quaternion.identity);
           
                    // Apply force to the projectile in the shoot direction
                    projectile.GetComponent<Rigidbody>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed;
                    activeCoolDown = true;
                }
                
                break;
            default:
                if (activeCoolDown1 == false)
                {
                    GameObject projectile1 = Instantiate(projectilePrefab1, (go + shootDirection), Quaternion.identity);

                    // Apply force to the projectile in the shoot direction
                    projectile1.GetComponent<Rigidbody>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed / 3;
                    activeCoolDown1 = true;
                }
                break;
        }
        
    }
}

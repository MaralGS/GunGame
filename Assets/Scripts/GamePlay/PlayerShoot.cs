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
    InGameConnection infoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        infoPlayer = GameObject.Find("Serialization").GetComponent<InGameConnection>();
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
            Vector3 shootDirection = (worldMousePosition - transform.position).normalized;

            Shoot(shootDirection);
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
        infoPlayer.GetPlayerShotInfo(projectilePrefab, transform.position);
    }

    private void Shoot(Vector3 shootDirection)
    {
        switch (gunType)
        {
            case 0:
                if (activeCoolDown == false)
                {
                    GameObject projectile = Instantiate(projectilePrefab, (transform.position + shootDirection), Quaternion.identity);

                    // Apply force to the projectile in the shoot direction
                    projectile.GetComponent<Rigidbody>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed;
                    activeCoolDown = true;
                }
                
                break;
            default:
                if (activeCoolDown1 == false)
                {
                    GameObject projectile1 = Instantiate(projectilePrefab1, (transform.position + shootDirection), Quaternion.identity);

                    // Apply force to the projectile in the shoot direction
                    projectile1.GetComponent<Rigidbody>().velocity = new Vector3(shootDirection.x, shootDirection.y, 0f) * projectileSpeed / 3;
                    activeCoolDown1 = true;
                }
                break;
        }
        
    }
}

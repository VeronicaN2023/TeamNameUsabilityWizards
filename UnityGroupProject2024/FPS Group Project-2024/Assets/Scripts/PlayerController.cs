using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int speed;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] int sprintSpeed;
    [SerializeField] int health;

    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isShooting;
    int origHealth;

    // Start is called before the first frame update
    void Start()
    {
        origHealth = health;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.gray);
        movement();
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);

        sprint();

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y += jumpSpeed;
        }

        if (Input.GetButtonDown("Fire1") && !isShooting)
        {
            StartCoroutine(shoot());
        }

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintSpeed;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintSpeed;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takedamage(shootDamage);
            }

        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHealthBar.fillAmount = (float)health / origHealth;
    }

    IEnumerator flashDamage()
    {
        gameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerFlashDamage.SetActive(false);
    }
}

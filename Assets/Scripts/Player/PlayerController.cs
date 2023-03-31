using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform aimFirePoint;
    [SerializeField] private float fireRate = 0.08f;
    [SerializeField] private float arrowFireRate = 0.2f;
    [SerializeField] private GameObject[] weapons;

    private Vector2 moveInput;
    private Camera mainCamera;
    private Rigidbody2D playerRB;
    private Animator animator;
    private Vector3 shootingVector3;
    private Vector3 shootingOffsetVector3 = new Vector3(0f, 0f, 90f);
    private Quaternion aimDir;
    private FireMode fireMode;
    private float fireTimer = 0f;
    private bool bursting;

    private void Start()
    {
        mainCamera = Camera.main;
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleWeapon();
    }

    private void HandleMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        playerRB.velocity = moveInput * moveSpeed;

        if (moveInput != Vector2.zero)
        {
            if (moveInput.x == 0 && moveInput.y < 0)
            {
                animator.SetBool("IsWalkingSide", false);
                animator.SetBool("IsWalkingUp", false);
                animator.SetBool("IsWalkingDown", true);
            }
            else if (moveInput.x == 0 && moveInput.y > 0)
            {
                animator.SetBool("IsWalkingSide", false);
                animator.SetBool("IsWalkingUp", true);
                animator.SetBool("IsWalkingDown", false);
            }
            else if (moveInput.x > 0 && moveInput.y == 0)
            {
                animator.SetBool("IsWalkingSide", true);
                animator.SetBool("IsWalkingUp", false);
                animator.SetBool("IsWalkingDown", false);
            }
            else if (moveInput.x < 0 && moveInput.y == 0)
            {
                animator.SetBool("IsWalkingSide", true);
                animator.SetBool("IsWalkingUp", false);
                animator.SetBool("IsWalkingDown", false);
            }
        }
        else
        {
            animator.SetBool("IsWalkingSide", false);
            animator.SetBool("IsWalkingUp", false);
            animator.SetBool("IsWalkingDown", false);
        }
    }

    private void HandleWeapon()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);

        if (mousePosition.x < screenPoint.x)
        {
            transform.localScale = Vector3.one;
            aimPoint.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            aimPoint.localScale = Vector3.one;
        }

        Vector2 offset = new Vector2(mousePosition.x - screenPoint.x, mousePosition.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        aimPoint.rotation = Quaternion.Euler(0f, 0f, angle);
        shootingVector3 = aimFirePoint.rotation.eulerAngles;
        aimDir = Quaternion.Euler(shootingVector3 + shootingOffsetVector3);

        // Shooting
        if (fireTimer < fireRate + 1f)
        {
            fireTimer += Time.deltaTime;
        }

        if (fireTimer > fireRate && !bursting)
        {
            fireTimer = 0f;

            //Rifle
            if (gameObject.GetComponentInChildren<WeaponController>().GetWeaponType() == WeaponController.WeaponType.Rifle &&
                gameObject.GetComponentInChildren<WeaponController>().enabled)
            {
                bursting = true;
                fireMode = FireMode.Burst;
                print(gameObject.GetComponentInChildren<WeaponController>().GetWeaponType());
                StartCoroutine(BurstFireRifle(gameObject.GetComponentInChildren<WeaponController>().projectilePrefab));
            }
            //Gun
            else if (gameObject.GetComponentInChildren<WeaponController>().GetWeaponType() == WeaponController.WeaponType.Gun &&
                     gameObject.GetComponentInChildren<WeaponController>().enabled)
            {
                fireMode = FireMode.Semi;
                ShootBullet(aimDir, gameObject.GetComponentInChildren<WeaponController>().projectilePrefab);
                print(gameObject.GetComponentInChildren<WeaponController>().GetWeaponType());
            }
            //Bow
            else if (gameObject.GetComponentInChildren<WeaponController>().GetWeaponType() == WeaponController.WeaponType.Bow &&
                     gameObject.GetComponentInChildren<WeaponController>().enabled)
            {
                bursting = true;
                fireMode = FireMode.Burst;
                print(gameObject.GetComponentInChildren<WeaponController>().GetWeaponType());
                StartCoroutine(BurstFireBow(gameObject.GetComponentInChildren<WeaponController>().projectilePrefab));
            }
        }
    }

    private IEnumerator BurstFireRifle(GameObject projectile)
    {
        yield return new WaitForSeconds(fireTimer);
        ShootBullet(aimDir, projectile);
        yield return new WaitForSeconds(fireTimer);
        ShootBullet(aimDir, projectile);
        yield return new WaitForSeconds(fireTimer);
        ShootBullet(aimDir, projectile);
        bursting = false;
    }

    private IEnumerator BurstFireBow(GameObject projectile)
    {
        yield return new WaitForSeconds(fireTimer + 0.2f);
        ShootBullet(aimDir, projectile);
        yield return new WaitForSeconds(fireTimer + 0.2f);
        ShootBullet(aimDir, projectile);
        bursting = false;
    }

    private void ShootBullet(Quaternion aimDir, GameObject projectile)
    {
        GameObject bulletGameObject = Instantiate(projectile, aimFirePoint.position, aimDir);
    }

    public Vector2 GetMovementInput()
    {
        return moveInput;
    }

}
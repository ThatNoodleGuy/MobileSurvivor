using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform aimPoint;
    [SerializeField] private Transform aimFirePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float timeBetweenShots;

    private Vector2 moveInput;
    private Camera mainCamera;
    private Rigidbody2D playerRB;
    private Animator animator;
    private float shotCounter;
    private Vector3 shootingVector3;
    private Vector3 shootingOffsetVector3 = new Vector3(0f, 0f, 90f);

    private void Start()
    {
        mainCamera = Camera.main;
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        shotCounter = timeBetweenShots;
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
        Quaternion aimDir = Quaternion.Euler(shootingVector3 + shootingOffsetVector3);

        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0f)
        {
            ShootBullet(aimDir);
        }
    }

    private void ShootBullet(Quaternion aimDir)
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, aimFirePoint.position, aimDir);
        shotCounter = timeBetweenShots;
    }

    public Vector2 GetMovementInput()
    {
        return moveInput;
    }

}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public Transform target;
    public Transform left;
    public Transform right;

    public float moveSpeed = 3f;
    public float chaseSpeed = 6f;
    private bool isChasing = true;
    private bool isAttacking = false;
    public GameObject[] swordAttacks;
    private int attackNum = 0;
    private float startTime = 0f;
    private float attackLength = 0f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        target = left;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isAttacking);
        if (isAttacking)
        {
            Debug.Log("isatac");
            this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            startTime += Time.deltaTime;
            if (startTime > attackLength && attackNum == 0)
            {
                swordAttacks[0].SetActive(false);
                attackNum++;
                continueSwordAttacking();
            }
            else if (startTime > attackLength && attackNum == 1)
            {
                swordAttacks[1].SetActive(false);
                attackNum++;
                startCooldown();
            }
            else if (startTime > attackLength && attackNum == 2)
            {
                stopAttacking();
            }
        }
        else if (isChasing && !isAttacking)
        {
            this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(target.transform.position.x, target.transform.position.y, 0),
                    moveSpeed * Time.deltaTime
                );
        }
        if (!isAttacking)
        {
            if (transform.position.x - target.transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.position.x - target.transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            if (target == left && transform.position.x == target.transform.position.x)
            {
                changeTarget(right);
            }
            else if (target == right && transform.position.x == target.transform.position.x)
            {
                changeTarget(left);
            }
        }
        
    }

    public void startSwordAttacking()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            startTime = 0f;
            swordAttacks[0].SetActive(true);
            attackLength = .5f;
        }
    }

    public void continueSwordAttacking()
    {
        startTime = 0f;
        swordAttacks[1].SetActive(true);
        attackLength = .5f;
    }

    public void stopAttacking()
    {
        isAttacking = false;
        attackNum = 0;
    }

    public void startCooldown()
    {
        startTime = 0f;
        attackLength = 1.5f;
    }

    void changeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("hit");
            changeTarget(collision.gameObject.transform);
            moveSpeed = chaseSpeed;
            isChasing = true;
        }
    }
}
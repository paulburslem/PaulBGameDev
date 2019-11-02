using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wraith : MonoBehaviour
{
    // Start is called before the first frame update
    public float attackTimer = 5f;
    public float currentTime = 0f;
    public float headTimer = 7f;
    float strafeDistance = 1.5f;
    public float moveSpeed = 1f;
    public Transform target;
    public Transform fireLocation;
    public Transform fireLocation2;
    public Transform fireLocation3;
    public GameObject bullet;
    public GameObject laser;
    public GameObject explosionEffect;
    public LayerMask LaserMask;
    public GameObject crosshairGameObject;
    SpriteRenderer crosshair;
    public bool canMove = true;


    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        crosshair = crosshairGameObject.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            moveTowardsTarget();
        }
        aimAtTarget();
        if (target != null)
        {
            currentTime += Time.deltaTime;
            headTimer -= Time.deltaTime;
        }
        if (currentTime >= attackTimer)
        {
            shootArms();
            
            currentTime = 0f;
        }
        if (headTimer > 0.5f && headTimer < 2.5f)
        {
            delayedAim();
            canMove = false;
        }
        if (headTimer <= 0)
        {
            StartCoroutine(shootHead());
            headTimer = 7f;
            crosshair.enabled = false;
            canMove = true;
        }
    }
    void moveTowardsTarget()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) > strafeDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
            }
            else if (Vector2.Distance(transform.position, target.position) < strafeDistance * 0.75f)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up * 5f, Time.deltaTime * moveSpeed);
                
            }
        }
    }
    void aimAtTarget()
    {
        if (target != null)
        {
            fireLocation.right =  -fireLocation.position + target.position;
            fireLocation2.right = -fireLocation2.position + target.position;
            
        }
    }
    void delayedAim()
    {
        if (target != null)
        {
            crosshair.enabled = true;
            fireLocation3.right = -fireLocation3.position + target.position;
            crosshairGameObject.transform.position = target.position;
        }
        else
        {
            crosshair.enabled = false;
        }
    }
    void shootArms()
    {
        if (target != null)
        {
            GameObject temp = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
            temp.GetComponent<bullet>().playerName = name;
            GameObject temp2 = Instantiate(bullet, fireLocation2.position, fireLocation2.rotation);
            temp2.GetComponent<bullet>().playerName = name;
        }
    }
    IEnumerator shootHead()
    {
        animator.SetBool("Firing", true);
        if (target != null)
        {
            RaycastHit2D hitinfo = Physics2D.Raycast(fireLocation3.position, fireLocation3.right, 100f, LaserMask);
            
            Debug.Log(target.name);
            
            if (hitinfo.collider)
            {
                Instantiate(explosionEffect, hitinfo.point, Quaternion.identity);
                Debug.Log(hitinfo.collider.name);
                Health hitHealth = hitinfo.transform.GetComponent<Health>();
                if (hitHealth)
                {
                    hitHealth.takeDamage(30f);
                }
            }
            float width = hitinfo.distance;
            if (width == 0)
            {
                width = 100f;
            }
            Debug.Log(width);
            //width = 5f;
            GameObject tempLaser = Instantiate(laser, fireLocation3.position + fireLocation3.right * -0.1f, fireLocation3.rotation);
            tempLaser.GetComponent<SpriteRenderer>().size = new Vector2(width * 2f + 0.3f, 3.2f);
            
            yield return new WaitForSeconds(0.2f);
            Destroy(tempLaser);
            animator.SetBool("Firing", false);


        }
    }
}

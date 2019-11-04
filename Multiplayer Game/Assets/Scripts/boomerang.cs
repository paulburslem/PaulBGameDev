using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boomerang : MonoBehaviour
{
    Rigidbody2D rb;
    public string playerName;
    public float speed = 1f;
    public float verticalForce = 0f;
    public float lifetime = 10f;
    float timer = 0f;
    Animator animator;
    public GameObject impactExplosion;
    public float direction = 1f;
    public float damage = 10f;
    public bool teamAttack = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        direction = transform.rotation.y > 0 ? 1f : -1f;
        rb.velocity = transform.right * speed;
        rb.AddTorque(720f * direction);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            this.Destroy();
        }
    }

    private void FixedUpdate()
    {
        if (timer <= 1.8f)
        {
            rb.AddForce(new Vector2(direction * 5f, verticalForce));
            timer += Time.deltaTime;
        }
        
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.tag == "Entity" || collision.tag == "Player" || collision.tag == "Collidable" || collision.tag == "Shield")
        {

            if (playerName != collision.name)
            {
                if (collision.tag == "Entity" || collision.tag == "Player" || collision.tag == "Shield")
                {
                    if (collision.tag == "Shield")
                    {
                        if (collision.GetComponent<Shield>().parentName != playerName)
                        {
                            Instantiate(impactExplosion, transform.position, transform.rotation);
                            Destroy(gameObject);
                            Health enemyHealth = collision.GetComponent<Health>();
                            enemyHealth.takeDamage(damage);
                        }
                    }
                    else if (collision.name != playerName)
                    {
                        if (collision.tag == "Player" && !teamAttack)
                        {
                            return;
                        }

                        Instantiate(impactExplosion, transform.position, transform.rotation);
                        Destroy(gameObject);
                        if (collision.GetComponent<Health>())
                        {
                            Health enemyHealth = collision.GetComponent<Health>();
                            enemyHealth.takeDamage(damage);
                        }
                    }
                }
                else
                {
                    Instantiate(impactExplosion, transform.position, transform.rotation);
                    Destroy(gameObject);
                }


            }
        }

        
        if (collision != null && collision.tag == "Projectile")
        {
            if (collision.GetComponent<bullet>() && playerName != collision.GetComponent<bullet>().playerName)
            {
                Instantiate(impactExplosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }


    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

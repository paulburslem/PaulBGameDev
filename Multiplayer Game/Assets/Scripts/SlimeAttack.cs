using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    public float damage = 1f;
    public float knockbackForce = 300f;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.GetComponent<Health>().isShield)
        {
            collision.GetComponent<Health>().takeDamage(damage);
            float x = 90f;
            float y = 50f;
            if (collision.transform.position.x > rb.transform.position.x)
            {
                x *= -1f;
            }
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(x, y));
        }
    }
    // Update is called once per frame
    
}

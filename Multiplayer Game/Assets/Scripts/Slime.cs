using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slime : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask m_WhatIsGround;
    public Transform groundCheck;
    public Transform target;
    bool m_Grounded = false;
    public UnityEvent OnLandEvent;
    public float k_GroundedRadius = 0.02f;
    Animator animator;
    Rigidbody2D rb;
    float jumpTimer = 2f;
    float timer = 0f;


    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        timer = Random.Range(0f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();
        if (m_Grounded)
        {
            timer += Time.deltaTime;
        }
        if (timer >= jumpTimer)
        {
            timer = 0f;
            jump();
        }
        
        
    }
    public void setDead()
    {
        animator.SetTrigger("dead");
    }
    void jump()
    {
        animator.SetTrigger("jump");
        
    }
    public void addJumpForce()
    {
        if (target != null)
        {
            float distanceX = Vector2.Distance(transform.position, target.position);
            float ratio = 1 + distanceX / 1.4f;

            if (ratio > 2.5)
            {
                ratio = 2;
            }
            float x = 35f;
            float y = 100f;
            if (target.position.x < transform.position.x)
            {
                x *= -1;
            }
            rb.AddForce(new Vector2(x, y) * ratio);
        }
        else
        {
            rb.AddForce(new Vector2(Random.Range(60f, 100f) * Random.Range(-1, 1), 200f));
        }
    }
   
    public void disableGravity()
    {
        rb.gravityScale = 0;
    }
    private void checkGrounded()
    {

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    animator.SetTrigger("land");
                }
            }
        }
        animator.SetBool("grounded", m_Grounded);
    }
    
}

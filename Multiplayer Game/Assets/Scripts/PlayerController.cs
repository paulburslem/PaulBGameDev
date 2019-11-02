using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    CircleCollider2D footCollider;
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    public float maximumSpeed = 7f;
    public float movementSmooth = 0.8f;
    float axisHorizontal, axisVertical;
    public bool m_Grounded;
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private LayerMask m_WhatIsGround;
    public UnityEvent OnLandEvent;
    public Transform groundCheck;
    bool jumping = false;
    Animator animator;
    public float k_GroundedRadius = 0.02f;
    private bool m_FacingRight = true;
    public float gasTankCapacity = 2f;
    public float currentCapacity = 2f;
    public bool hovering = false;
    public bool charging = false;
    public float dashForce = 3f;
    public float dashDuration = 0.3f;
    float currentDashTimer = 0f;
    Vector2 shootDirection;
    public Transform fireLocation;
    public bool firing = false;
    public GameObject bullet;
    public GameObject boomerang;
    public bool dashing = false;
    public bool weaponSwitch = false;
    Shield shield;
    GameObject temp;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        footCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        shield = GetComponentInChildren<Shield>();
        
    }

    // Update is called once per frame
    
    void Update()
    {
        shield.grounded = m_Grounded;
        axisHorizontal = Input.GetAxisRaw("Horizontal");
        axisVertical = Input.GetAxisRaw("Vertical");
        firing = Input.GetButtonDown("Fire1");
        if (Input.GetButtonDown("Fire2"))
        {
            weaponSwitch = !weaponSwitch;
        }
        if (Input.GetButtonDown("Dash"))
        {
            dashing = true;
        }        
        shootDirection = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));
        checkGrounded();
        animator.SetBool("Grounded", m_Grounded);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetBool("NoInput", axisHorizontal == 0);
        animator.SetFloat("InputHorizontal", Mathf.Abs(axisHorizontal));
        animator.SetBool("Firing", firing);
        animator.SetBool("Dashing", dashing);
        if (Input.GetButtonDown("Fire1"))
        {
            if (!weaponSwitch)
            {
                temp = Instantiate(bullet, fireLocation.position, fireLocation.rotation);
                temp.GetComponent<bullet>().playerName = gameObject.name;
                
            }
            else
            {
                temp = Instantiate(boomerang, fireLocation.position, fireLocation.rotation);
                temp.GetComponent<boomerang>().playerName = gameObject.name;
            }

            firing = false;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (m_Grounded)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                rb.velocity = Vector2.up * jumpForce;
            }            
        }
        hovering = false;
        charging = false;
        if (Input.GetButton("Hover"))
        {
            if (!m_Grounded)
            {
                if (currentCapacity > 0 && rb.velocity.y < 0)
                {
                    hovering = true;
                }
                else
                {
                    hovering = false;
                }
            }
            else
            {
                if (Mathf.Abs(axisHorizontal) == 0)
                {
                    currentCapacity += Time.deltaTime;
                    charging = true;
                    if (currentCapacity >= gasTankCapacity)
                    {
                        currentCapacity = gasTankCapacity;
                        charging = false;
                    }
                }
                else
                {
                    charging = false;
                }
            }
        }
        if (m_Grounded)
        {
            currentCapacity += Time.deltaTime * 0.25f;
        }
        if (currentCapacity >= gasTankCapacity)
        {
            currentCapacity = gasTankCapacity;
            charging = false;
        }
        animator.SetBool("Charging", charging);
        animator.SetBool("Hovering", hovering);
        shield.boosting = dashing;
        shield.grounded = m_Grounded;

        float speedY = Mathf.Abs(rb.velocity.y);  // test current object speed

        if (speedY > maximumSpeed)
        {
            float ratio = maximumSpeed / speedY;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * ratio);
        }
        if (temp != null && temp.GetComponent<boomerang>())
        {
            temp.GetComponent<boomerang>().verticalForce = -Input.GetAxis("Vertical") * 2f;
        }

    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            rb.gravityScale = 0f;
            currentDashTimer += Time.deltaTime;
            if (currentDashTimer < dashDuration)
            {
                if (m_FacingRight)
                {
                    move(dashForce, 0f);
                }
                else
                {
                    move(-dashForce, 0f);
                }
            }
            else
            {
                dashing = false;
                rb.gravityScale = 1f;
                currentDashTimer = 0f;
            }

        }
        else
        {
            move(axisHorizontal, 0f);
        }
           
        if (hovering)
        {
            currentCapacity -= 1f * Time.deltaTime;
            if (currentCapacity > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.1f);
            }
            else
            {
                currentCapacity = 0f;
            }
            

        }
        
    }

    private void move(float horizontal, float vertical)
    {
        Vector3 targetVelocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        // And then smoothing it out and applying it to the character
        
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmooth);
        if (horizontal > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (horizontal < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
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
                    OnLandEvent.Invoke();
            }
        }

    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        
        theScale.x *= -1;
        
        transform.localScale = theScale;
        fireLocation.transform.rotation = fireLocation.transform.rotation.y == 0 ? 
            Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }

    


}

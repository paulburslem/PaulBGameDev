using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpGravityModifierP2 : MonoBehaviour
{
    Rigidbody2D rb;
    public float fallSpeed = 2.5f;
    public float lowJumpFallSpeed = 2f;
    PlayerController controller;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("JumpP2"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpFallSpeed - 1) * Time.deltaTime;
        }
        
    }
}

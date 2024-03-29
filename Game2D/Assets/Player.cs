﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	public float moveForce = 100;
	public float jumpForce = 100;
	Vector2 moveVector;
	Vector2 aimVector;
	Rigidbody2D body;
	PlayerInput playerInput;
	bool firing;
	bool onGround;
	bool onPlayer;
	bool jumpPressed;
	float jumpTime;
	public Transform arm;
	public Transform hand;
    public GameObject projectile;
    public float fireStrength = 3000f;
    bool facingRight = true;
    GameObject shotItem;
	public SpringJoint2D grappleJoint;
	float grappleVelocityIn;
	float grappleVelocityOut;
    // Start is called before the first frame update
    void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
		body = GetComponent<Rigidbody2D>();
		playerInput.currentActionMap.FindAction("Move").performed += ctx => moveVector = ctx.ReadValue<Vector2>();
		playerInput.currentActionMap.FindAction("Move").canceled += ctx => moveVector = Vector2.zero;
		playerInput.currentActionMap.FindAction("LookLeftRight").performed += ctx => aimVector.x = ctx.ReadValue<float>();
		playerInput.currentActionMap.FindAction("LookUpDown").performed += ctx => aimVector.y = -ctx.ReadValue<float>();
		playerInput.currentActionMap.FindAction("Jump").started += ctx => JumpStart();
		playerInput.currentActionMap.FindAction("Jump").canceled += ctx => JumpEnd();
		playerInput.currentActionMap.FindAction("Grapple").started += ctx => GrappleFire();
		playerInput.currentActionMap.FindAction("Grapple").canceled += ctx => GrappleEnd();
		playerInput.currentActionMap.FindAction("GrappleIn").performed += ctx => grappleVelocityIn = ctx.ReadValue<float>();
		playerInput.currentActionMap.FindAction("GrappleOut").performed += ctx => grappleVelocityOut = ctx.ReadValue<float>();
		playerInput.currentActionMap.FindAction("GrappleIn").canceled += ctx => grappleVelocityIn = 0;
		playerInput.currentActionMap.FindAction("GrappleOut").canceled += ctx => grappleVelocityOut = 0;

		UnityEngine.Random.InitState(playerInput.playerIndex * 834652);

		//GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
	}


	private void GrappleEnd()
	{
		Destroy(shotItem);
		grappleJoint.enabled = false;
	}

	internal void SetupGrapple(Vector3 position, Rigidbody2D other)
	{
//		grappleJoint.connectedBody = other;
		grappleJoint.connectedAnchor = position;
		grappleJoint.distance = (hand.position - position).magnitude * .5f;
///		grappleJoint.anchor = Vector2.zero;
		grappleJoint.enabled = true;
	}

	private void JumpEnd()
	{
		body.gravityScale = 1;

	}
	private void JumpStart()
	{
		if (onGround || onPlayer)
		{
			body.gravityScale = .5f;
			jumpPressed = true;
			jumpTime = Time.timeSinceLevelLoad;
			if(onGround)
				body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
			if (onPlayer)
			{
				body.AddForce(new Vector2(0, jumpForce * .75f), ForceMode2D.Impulse);

				foreach (var r in ray)
				{
					if(r != null && r.gameObject != gameObject)
						r.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce * -.25f), ForceMode2D.Impulse);
				}
			}
			onPlayer = false;
			onGround = false;
            
		}
	}

	private void GrappleFire()
	{
        if (shotItem == null)
        {
            shotItem = Instantiate(projectile, hand.position, arm.rotation * Quaternion.Euler(0, 0, 90));
            shotItem.GetComponent<Rigidbody2D>().AddForce(shotItem.transform.up * -fireStrength);
            shotItem.GetComponent<CanePhysics>().fireLocation = hand;
            shotItem.GetComponent<CanePhysics>().Player = gameObject;
        }

    }

	private void Fire()
	{
        Debug.Log("FIRING");
       if (shotItem && GetComponent<SpringJoint2D>().enabled && GetComponent<SpringJoint2D>().distance >= 0.5f)
        {
            GetComponent<SpringJoint2D>().distance -= Time.deltaTime;
        }
    }

	// Update is called once per frame
	void Update()
    {
        if (moveVector.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveVector.x < 0 && facingRight)
        {
            Flip();
        }
        if (firing)
        {
            Fire();
        }
	//	Debug.Log($"ground={onGround}");
    if (Mathf.Abs(body.velocity.x) >= 0.7f && (onGround || onPlayer))
        {
            GetComponent<Animator>().SetBool("Running", true);
            if (!GetComponent<AudioSource>().isPlaying)
            {
                playAudio();
            }
        }
    else
        {
            GetComponent<Animator>().SetBool("Running", false);
            stopAudio();
        }
        GetComponent<Animator>().SetBool("Grounded", onGround);
        
    }
    
    Collider2D[] ray = new Collider2D[2];
	private void FixedUpdate()
	{
		if (body.velocity.y < 0)
			body.gravityScale = 1;
		onGround = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x, transform.position.y), .1f, ray, LayerMask.GetMask("ground"), float.MinValue, float.MaxValue) > 0;
		onPlayer = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x, transform.position.y), .1f, ray, LayerMask.GetMask("player"), float.MinValue, float.MaxValue) > 1;
		if (onGround || onPlayer)
		{
			body.drag = 10f;
			body.AddForce(new Vector2(moveVector.x * moveForce, 0));
		}
		else
		{
			body.drag = .2f;
			body.AddForce(new Vector2((moveVector * moveForce * .1f).x, 0));
			/*
			if (jumpPressed && Time.timeSinceLevelLoad - jumpTime < .3f)
			{
				var jp = (.3f - (Time.timeSinceLevelLoad - jumpTime)) * 10;
				body.AddForce(new Vector2(0, jp * 50), ForceMode2D.Force);
			}*/
		}

		if (aimVector.sqrMagnitude > .25f)
		{
			aimVector.Normalize();
			var ab = arm.GetComponent<Rigidbody2D>();
            
			var da = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;
			var ca = ab.rotation;
			var dv = Mathf.DeltaAngle(ca, da) * 20;
			var cv = ab.angularVelocity;
			var vd = (dv - cv);
			var t = vd;
			ab.AddTorque(t);
//			Debug.Log($"a: {da}, ca {ca}, dv {dv}, cv {cv}, vd {vd}, t {t}");
		}
		else
		{
			arm.GetComponent<Rigidbody2D>().angularVelocity = 0;
		}
		if (grappleJoint.enabled)
		{
			grappleJoint.distance += (grappleVelocityOut - grappleVelocityIn) * Time.deltaTime * 10;
		}
	}
    public void playAudio()
    {
        GetComponent<AudioSource>().Play();
    }
    public void stopAudio()
    {
        GetComponent<AudioSource>().Stop();
    }
    private void Flip()
    {
        facingRight = !facingRight;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }
}


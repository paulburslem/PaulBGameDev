using System;
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

    // Start is called before the first frame update
    void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
		body = GetComponent<Rigidbody2D>();
		playerInput.currentActionMap.FindAction("Move").performed += ctx => moveVector = ctx.ReadValue<Vector2>();
		playerInput.currentActionMap.FindAction("Look").performed += ctx => aimVector = ctx.ReadValue<Vector2>();
		playerInput.currentActionMap.FindAction("Jump").started += ctx => Jump();
		playerInput.currentActionMap.FindAction("Fire").started += ctx => firing = true;
		playerInput.currentActionMap.FindAction("Fire").canceled += ctx => firing = false;
		playerInput.currentActionMap.FindAction("Grapple").started += ctx => Grapple();
		UnityEngine.Random.InitState(playerInput.playerIndex * 834652);

		GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
	}

	private void Jump()
	{
		if (onGround)
			body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	private void Grapple()
	{
	
	}

	private void Fire()
	{
		
	}

	// Update is called once per frame
	void Update()
    {

	//	Debug.Log($"ground={onGround}");
	}

	private void FixedUpdate()
	{
		if (onGround)
		{
			moveForce = 2000;
			body.drag = 50;
		}
		else
		{
			moveForce = 50;
			body.drag = .2f;
		}
		moveVector.y = 0;
		body.AddForce(moveVector * moveForce);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.CompareTag("ground"))
			onGround = true;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("ground"))
			onGround = false;
	}
}

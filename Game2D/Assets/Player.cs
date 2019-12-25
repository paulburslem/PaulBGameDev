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
	bool onPlayer;
	bool jumpPressed;
	float jumpTime;
	public Transform arm;
	public Transform hand;
    public GameObject projectile;
    public float fireStrength = 3000f;
    GameObject shotItem;
    // Start is called before the first frame update
    void Awake()
    {
		playerInput = GetComponent<PlayerInput>();
		body = GetComponent<Rigidbody2D>();
		playerInput.currentActionMap.FindAction("Move").performed += ctx => moveVector = ctx.ReadValue<Vector2>();
		playerInput.currentActionMap.FindAction("Move").canceled += ctx => moveVector = Vector2.zero;
		playerInput.currentActionMap.FindAction("LookLeftRight").performed += ctx => aimVector.x = ctx.ReadValue<float>();
		playerInput.currentActionMap.FindAction("LookUpDown").performed += ctx => aimVector.y = -ctx.ReadValue<float>();
		playerInput.currentActionMap.FindAction("Jump").started += ctx => Jump();
		playerInput.currentActionMap.FindAction("Jump").canceled += ctx => jumpPressed = false;
		playerInput.currentActionMap.FindAction("Fire").started += ctx => firing = true;
		playerInput.currentActionMap.FindAction("Fire").canceled += ctx => firing = false;
		playerInput.currentActionMap.FindAction("Grapple").started += ctx => Grapple();
		UnityEngine.Random.InitState(playerInput.playerIndex * 834652);

		GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
	}

	private void Jump()
	{
		if (onGround || onPlayer)
		{
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

	private void Grapple()
	{
        if (shotItem == null)
        {
            shotItem = Instantiate(projectile, hand.position, arm.rotation);
            shotItem.GetComponent<Rigidbody2D>().AddForce(shotItem.transform.up * fireStrength);
            shotItem.GetComponent<CanePhysics>().fireLocation = hand;
            shotItem.GetComponent<CanePhysics>().Player = gameObject;
        }
        if (shotItem.GetComponent<CanePhysics>().attached)
        {
            Destroy(shotItem);
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
        if (firing)
        {
            Fire();
        }
	//	Debug.Log($"ground={onGround}");
	}
	Collider2D[] ray = new Collider2D[2];
	private void FixedUpdate()
	{
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
			body.AddForce(moveVector * moveForce * .05f);

			if (jumpPressed && Time.timeSinceLevelLoad - jumpTime < .25f)
			{
				var jp = (.25f - (Time.timeSinceLevelLoad - jumpTime)) * 2;
				body.AddForce(new Vector2(0, jp * 1.5f), ForceMode2D.Impulse);
			}
		}

		if (aimVector.sqrMagnitude > .25f)
		{
			aimVector.Normalize();
			var a = Mathf.Atan2(aimVector.y, aimVector.x) * Mathf.Rad2Deg;
			var ab = arm.GetComponent<Rigidbody2D>();
			float cv = 0;
			var aa = Mathf.SmoothDampAngle(ab.rotation, a, ref cv, .25f, 10, Time.deltaTime);
			ab.rotation = a;
			//ab.AddTorque(a - ab.rotation);
			//arm.localEulerAngles = new Vector3(0, 0, a);
		}

	}
}

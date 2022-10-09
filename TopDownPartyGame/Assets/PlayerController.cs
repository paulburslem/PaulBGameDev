using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	Rigidbody rb;
	
	public float rotation;
	public Vector3 movement = Vector3.zero;
	public Vector3 swing = Vector3.zero;
	public int playerIndex = 1;
	string xMove, yMove, xSwing, ySwing;
	public float movementSpeed = 8.0f;
	public Transform armTransform;


    // Start is called before the first frame update
    void Awake()
    {
		xMove = $"xMove{playerIndex}";
		yMove = $"yMove{playerIndex}";
		xSwing = $"xSwing{playerIndex}";
		ySwing = $"ySwing{playerIndex}";

		rb = GetComponent<Rigidbody>();

    }

	
	// Update is called once per frame
	void FixedUpdate()
    {
		//movement = new Vector3(Input.GetAxisRaw(xMove), 0.0f, Input.GetAxisRaw(yMove));
		rb.AddForce(movement * movementSpeed * Time.fixedDeltaTime);
		armTransform.rotation = new Quaternion(swing.x * 90.0f, 0.0f, swing.z * 90.0f, 1.0f);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanePhysics : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public GameObject Player;
    public Transform connectLocation;
    public Transform fireLocation;
    public Rigidbody2D playerRB;
    //public TrailRenderer TR;
    LineRenderer LR;
    float lifespan = 4f;
    public bool attached = false;
    CircleCollider2D collider;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        LR = GetComponent<LineRenderer>();
        /* GameObject link = Instantiate(headLink, connectLocation.position, connectLocation.rotation);
         link.GetComponent<HingeJoint2D>().connectedBody = rb;
         */

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"collision tag: {collision.tag}");
        if (collision.tag != "Player")
        {

           // this.transform.position = collision.transform.position;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            //TR.enabled = false;
            LR.enabled = true;
            
            Quaternion rotation = Quaternion.LookRotation
             (fireLocation.transform.position - transform.position, transform.TransformDirection(Vector3.down));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
			Player.GetComponent<Player>().SetupGrapple(transform.position, rb);
			/*
			Player.GetComponent<SpringJoint2D>().connectedBody = collider.GetComponent<Rigidbody2D>();
            Vector2 offset = transform.position - Player.GetComponent<Transform>().position;
            Player.GetComponent<SpringJoint2D>().autoConfigureConnectedAnchor = true;
            Player.GetComponent<SpringJoint2D>().enabled = true;
            Player.GetComponent<SpringJoint2D>().autoConfigureConnectedAnchor = false;
            Player.GetComponent<SpringJoint2D>().distance = offset.magnitude * 0.4f;
            Debug.Log($"spring distance: {Player.GetComponent<SpringJoint2D>().distance}");
            
            Player.GetComponent<SpringJoint2D>().connectedAnchor *= new Vector2(1, 1f);
			*/
            attached = true;



        }

    }
    // Update is called once per frame
    private void Update()
    {
        LR.SetPositions(new Vector3[] {connectLocation.position, fireLocation.position });
        if (!attached)
        {
            lifespan -= Time.deltaTime;
        }
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }

        
       
        
    }
    void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            rb.rotation = Mathf.Rad2Deg * Mathf.Atan(rb.velocity.y / rb.velocity.x) - 90;
        }
            //transform.localRotation = Quaternion.LookRotation(rb.velocity.normalized, new Vector3(0, 0, -1));
       
    }
}

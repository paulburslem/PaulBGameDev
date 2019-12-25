using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject projectile;
    public float fireStrength = 400f;
    public GameObject player;
    void Start()
    {
        
    }
    float timer = 2f;
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            shoot();
            timer = 100f;
        }
    }

    void shoot()
    {
        GameObject shotItem = Instantiate(projectile, this.transform.position, this.transform.rotation);
        shotItem.GetComponent<Rigidbody2D>().AddForce(shotItem.transform.up * fireStrength);
        shotItem.GetComponent<CanePhysics>().fireLocation = this.transform;
        shotItem.GetComponent<CanePhysics>().Player = player;
        
    }
}

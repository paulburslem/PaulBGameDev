using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Player" || collision.tag == "Entity")
        {
            collision.GetComponent<Health>().takeDamage(collision.GetComponent<Health>().maxHealth);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

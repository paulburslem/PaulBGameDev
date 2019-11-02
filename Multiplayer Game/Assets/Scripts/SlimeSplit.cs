using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlimeSplit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject splitPrefab;
    void Start()
    {
        
    }
    public void Split()
    {
        GameObject leftSlime = Instantiate(splitPrefab, transform.position, transform.rotation);
        GameObject rightSlime = Instantiate(splitPrefab, transform.position, transform.rotation);
        leftSlime.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 40));
        rightSlime.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 40));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

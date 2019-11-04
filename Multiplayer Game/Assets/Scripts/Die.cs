using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer = 0.2f;
    bool enabled = true;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
            timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
            enabled = false;
        }
    }
}

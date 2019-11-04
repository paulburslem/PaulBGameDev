using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    private float scaleX;
    Health health;
    public float healthPercentage = 1f;
    Vector3 scale;
    void Awake()
    {
        scaleX = transform.localScale.x;
        scale = transform.localScale;
        health = GetComponentInParent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health != null)
        {
            healthPercentage = health.currentHealth / health.maxHealth;
            scale.x = scaleX * healthPercentage;
            transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * 5f);
        }
    }
}

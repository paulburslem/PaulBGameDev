using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    Health health;
    
    public float decayRate = 0.1f;
    public float moveScale = 1f;
    Collider2D collider;
    SpriteRenderer sprite;
    public bool player1 = true;
    string shieldButton, axisH, axisV;
    Vector3 originalScale, originalPosition;
    Transform parentPos;
    public bool grounded = true;
    public bool boosting = false;
    public string parentName;
    void Start()
    {
        health = GetComponent<Health>();
        shieldButton = player1 ? "Shield" : "ShieldP2";
        axisH = player1 ? "Horizontal2" : "Horizontal2P2";
        axisV = player1 ? "Vertical2" : "Vertical2P2";
        parentName = player1 ? "Player 1" : "Player 2";
        collider = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        parentPos = GetComponentInParent<Transform>();
        originalScale = transform.localScale;
        originalPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        originalPosition = parentPos.position;
        if (Input.GetButton(shieldButton) && grounded && !boosting)
        {
            collider.enabled = true;
            sprite.enabled = true;
            health.takeDamage(Time.deltaTime * decayRate);

        }
        else
        {
            collider.enabled = false;
            sprite.enabled = false;
            
            health.takeDamage(Time.deltaTime * -decayRate * 2f);
        }
        scaleSize();
        moveShield();
    }

    void scaleSize()
    {
        float healthPercentage = health.currentHealth / health.maxHealth;
        transform.localScale = originalScale * (0.3f + healthPercentage);
    }
    void moveShield()
    {
        transform.rotation = Quaternion.identity;
        if (parentPos.lossyScale.x > 0 && moveScale < 0)
        {
            moveScale *= -1;
        }
        else if (parentPos.lossyScale.x < 0 && moveScale > 0)
        {
            moveScale *= -1;
        }
        Vector3 shieldPos = new Vector3(Input.GetAxis(axisH) * moveScale, Input.GetAxis(axisV) * -Mathf.Abs(moveScale), 0);
        transform.localPosition = shieldPos;
    }
}

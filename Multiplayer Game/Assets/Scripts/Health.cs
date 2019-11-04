using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 100f;
    public bool isShield = false;
    public float currentHealth = 100f;
    public bool isDead = false;
    public Animator animator;
    public UnityEvent OnDeathEvent;



    public void takeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            if (!isShield)
            {
                isDead = true;
                OnDeathEvent.Invoke();
                if (animator != null)
                {
                    //animator.SetTrigger("dead");
                }
                else
                {
                    Destroy(gameObject);
                }

            }
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    // Update is called once per frame
   
}

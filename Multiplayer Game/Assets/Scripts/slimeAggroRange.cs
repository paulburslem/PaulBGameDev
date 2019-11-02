using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeAggroRange : MonoBehaviour
{
    Slime slime;
    float timeToSwitchTarget = 5f;
    float timer = 0f;
    bool needToSwitch;
    int index = 0;
    public List<Collider2D> enemiesInLookRange;


    void Awake()
    {
        slime = GetComponentInParent<Slime>();
        enemiesInLookRange = new List<Collider2D>();
        Collider2D[] allOverlappingColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
        foreach (Collider2D p in allOverlappingColliders)
        {
            if (p.tag == "Player")
            {
                enemiesInLookRange.Add(p);
            }
        }
        if (enemiesInLookRange.Count != 0)
        {
            slime.target = enemiesInLookRange[0].GetComponent<Transform>();
        }
    }
    public void Update()
    {
        if (enemiesInLookRange.Count != 0)
        {
            timer += Time.deltaTime;
            if (timer >= timeToSwitchTarget && enemiesInLookRange.Count > 0)
            {
                timer = 0f;
                index++;
                if (index >= enemiesInLookRange.Count)
                {
                    index = 0;
                }
                if (enemiesInLookRange[index] != null)
                {
                    slime.target = enemiesInLookRange[index].GetComponent<Transform>();
                }
            }
            if (enemiesInLookRange.Count > 0 && enemiesInLookRange[index] == null)
            {
                timer = timeToSwitchTarget;
            }
        }
        else
        {

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemiesInLookRange.Contains(collision) && (collision.gameObject.tag == "Player"))
        {
            enemiesInLookRange.Add(collision);
            if (slime.target == null && enemiesInLookRange.Count > 0)
            {
                
                slime.target = enemiesInLookRange[0].GetComponent<Transform>();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemiesInLookRange.Contains(collision))
        {
            enemiesInLookRange.Remove(collision);
        }
    }
}

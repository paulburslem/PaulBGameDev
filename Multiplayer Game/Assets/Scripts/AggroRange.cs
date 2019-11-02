using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; //Needed to use Lists


public class AggroRange : MonoBehaviour
{
    // Start is called before the first frame update
    Wraith wraith;
    float timeToSwitchTarget = 15f;
    float timer = 0f;
    bool needToSwitch;
    int index = 0;
    public List<Collider2D> enemiesInLookRange;


    void Awake()
    {
        wraith = GetComponentInParent<Wraith>();
        enemiesInLookRange = new List<Collider2D>();
        Collider2D[] allOverlappingColliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
        foreach(Collider2D p in allOverlappingColliders)
        {
            if (p.tag == "Player")
            {
                enemiesInLookRange.Add(p);
            }
        }
        if (enemiesInLookRange.Count != 0)
        {
            wraith.target = enemiesInLookRange[0].GetComponent<Transform>();
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
                    wraith.target = enemiesInLookRange[index].GetComponent<Transform>();
                }
            }
            if (enemiesInLookRange[index] == null)
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
            if (wraith.target == null)
            {
                wraith.target = enemiesInLookRange[0].GetComponent<Transform>();
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

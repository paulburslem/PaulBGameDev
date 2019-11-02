using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform shootLocation;
    float angle = 90f;
    public GameObject bullet;
    void Awake()
    {

        StartCoroutine(sprayShot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator sprayShot()
    {
        Transform originalPos = shootLocation;
        Vector3 endRotation = originalPos.rotation.eulerAngles + new Vector3(0, 0, angle);
        int i = 0;
        float speed = 4;
        while (i < 40)
        {

            GameObject temp = Instantiate(bullet, shootLocation.position, shootLocation.rotation);
            Vector3 newRotation = shootLocation.rotation.eulerAngles + new Vector3(0, 0, 5f);
            if (i == 20)
            {
                speed *= -1;
            }
            shootLocation.rotation = Quaternion.Euler(0,0, speed + originalPos.rotation.eulerAngles.z);
            i += 1;
            yield return new WaitForSeconds(0.1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player1, player2;
    Camera camera;
    float minSizeY = 1f;
    public bool perspective = false;
    void Awake()
    {
        camera = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player2 == null)
        {
            player2 = player1;
        }
        if (player1 == null)
        {
            player1 = player2;
        }
        SetCameraPos();
        if (!perspective)
        {
            SetCameraSize();
        }
        else
        {
            zoomOut();
        }
        
    }
    void SetCameraPos()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 middle = (player1.position + player2.position) * 0.5f;
            camera.transform.position = Vector3.Lerp(new Vector3(
                middle.x,
                middle.y,
                camera.transform.position.z
            ), camera.transform.position, 0.1f);
        }
    }

    float lerp(float a, float b, float f)
    {
        return a + f * (b - a);
    }
    void SetCameraSize()
    {
        if (player1 == null && player2 == null)
        {
            return;
        }
        //horizontal size is based on actual screen ratio
        float minSizeX = minSizeY * Screen.width / Screen.height;
        //multiplying by 0.5, because the ortographicSize is actually half the height
        float width = Mathf.Abs(player1.position.x - player2.position.x) * 0.5f + 2.8f;
        float height = Mathf.Abs(player1.position.y - player2.position.y) * 0.5f + 2.8f;
        //computing the size
        float camSizeX = Mathf.Max(width, minSizeX);
        
        camera.orthographicSize = lerp(Mathf.Max(height,
            camSizeX * Screen.height / Screen.width, minSizeY), camera.orthographicSize, 0.01f * Time.deltaTime);
    }
    void zoomOut()
    {
        if (player1 == null && player2 == null)
        {
            return;
        }
        float targetZ = -7f - 0.8f * Vector2.Distance(player1.position, player2.position);
        camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(camera.transform.position.x, camera.transform.position.y, targetZ), 5.2f * Time.deltaTime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    private float viewHeight;

    private void Awake()
    {
        viewHeight = 12.75f; //Camera.main.orthographicSize * 2;
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if(transform.position.y <= -viewHeight)
            transform.position = new Vector2(0, viewHeight);
    }
}

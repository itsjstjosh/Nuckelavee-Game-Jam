using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wisp : MonoBehaviour
{
    public float height = 0.3f;
    public float speed = 0.4f;
    public Vector2 startPos;

    public void Start()
    {
        startPos = transform.position;
    }
    public void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector2(transform.position.x, newY);
    }



}

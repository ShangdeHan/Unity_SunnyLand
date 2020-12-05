using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cam;
    public float moveRate;
    private float startPointx, startPointy;
    public bool locky;

    // Start is called before the first frame update
    void Start()
    {
        startPointx = transform.position.x;
        startPointy = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (locky)
        {
            transform.position = new Vector2(startPointx + cam.position.x * moveRate, transform.position.y);
        } else
        {
            transform.position = new Vector2(startPointx + cam.position.x * moveRate, startPointy + cam.position.y * moveRate);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_eagle : Enemy {
    public Rigidbody2D rb;
    public Transform up, down;
    public float upy, downy;
    public float speed;
    public int peek;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        peek = 1;
        upy = up.position.y;
        downy = down.position.y;
        Destroy(up.gameObject);
        Destroy(down.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void Move()
    {
        if (peek == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > upy)
            {
                peek = -1;
            }
        }
        else if (peek == -1)
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < downy)
            {
                peek = 1;
            }
        }
    }
}

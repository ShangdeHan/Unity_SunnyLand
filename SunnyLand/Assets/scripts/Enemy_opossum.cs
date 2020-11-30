using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_opossum : Enemy {
    public Rigidbody2D rb;
    public Transform left, right;
    //public float leftx, rightx;
    public bool faceleft;
    public float speed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        faceleft = true;
        //first method: 
        transform.DetachChildren();
        //second method: leftx = left.position.x;
        //               rightx = right.position.x;
        //               Destroy(left.gameObject);
        //               Destroy(right.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void Move()
    {
        if (faceleft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < left.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceleft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > right.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceleft = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    protected Animator animt;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animt = GetComponent<Animator>();
    }

    public void Destroythis()
    {
        Destroy(gameObject);
    }

    public void Death()
    {
        animt.SetTrigger("death");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    protected Animator animt;
    protected AudioSource deathAudio;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animt = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void Destroythis()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }

    public void Death()
    {
        deathAudio.Play();
        animt.SetTrigger("death");
    }
}

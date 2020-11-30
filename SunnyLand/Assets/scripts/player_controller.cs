using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_controller : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    private Animator animat;
    public LayerMask ground;
    public Collider2D coll;
    public int Cherry;
    public int Gem;
    public int jumpCount;
    public Text CherryNum;
    public Text GemNum;
    public bool IsHurt = false;
    public AudioSource jumpAudio, hurtAudio, collectionAudio;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animat = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsHurt == false) 
        {
            Movement();
        }
        SwitchAnim();
    }

    void Movement() 
    {
        float horizontalmove;
        float facedirection;
        horizontalmove = Input.GetAxis("Horizontal");
        facedirection = Input.GetAxisRaw("Horizontal");
        if (horizontalmove != 0) {
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            animat.SetFloat("Running", Mathf.Abs(facedirection));
        }
        if (facedirection != 0) {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        if (Input.GetButtonDown("Jump")) 
        {
            jumpAudio.Play();
            if (Cherry < 3)
            {
                if (jumpCount < 3)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                    animat.SetBool("Jumping", true);
                    jumpCount++;
                }
                else if (coll.IsTouchingLayers(ground))
                {
                    jumpCount = 0;
                }
            }
            else 
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                animat.SetBool("Jumping", true);
            }
        }
    }

    void SwitchAnim() 
    {
        animat.SetBool("idle", false);
        if(IsHurt == true) 
        {
            animat.SetBool("Hurt", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f) 
            {
                IsHurt = false;
                animat.SetBool("Hurt", false);
                animat.SetBool("idle", true);
            }
        }
        if ( rb.velocity.y < 0.1f)
        {
            animat.SetBool("Falling", true);
            animat.SetBool("idle", false);
        }
        if (rb.velocity.y > 1.0f && !coll.IsTouchingLayers(ground))
        {
            animat.SetBool("Jumping", true);
            animat.SetBool("idle", false);
        }
        if (animat.GetBool("Jumping")) {
            if (rb.velocity.y < 0) {
                animat.SetBool("Jumping", false);
                animat.SetBool("Falling", true);
            }
        } else if (coll.IsTouchingLayers(ground)) 
        {
            animat.SetBool("Falling", false);
            animat.SetBool("idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collistion) 
    {
        if (collistion.tag == "Collection") 
        {
            Destroy(collistion.gameObject);
            Cherry++;
            CherryNum.text = Cherry.ToString();
            collectionAudio.Play();
        }
        else if (collistion.tag == "Gem")
        {
            Destroy(collistion.gameObject);
            Gem++;
            GemNum.text = Gem.ToString();
            collectionAudio.Play();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (animat.GetBool("Falling"))
            {
                enemy.Death();
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                animat.SetBool("Jumping", true);
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            { 
                rb.velocity = new Vector2(-7, rb.velocity.y);
                hurtAudio.Play();
                IsHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(7, rb.velocity.y);
                hurtAudio.Play();
                IsHurt = true;
            }
        }
    }
}

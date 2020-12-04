using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_controller : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    public float speed, jumpForce;
    private Animator animat;
    public LayerMask ground, ladder;
    public Collider2D coll, disColl;
    public Transform ceilingCheck, groundCheck;
    private int Cherry, Gem;
    private int jumpCount;
    public Text CherryNum, GemNum;
    public bool IsHurt, jumpPress, isGround, isJump;
    public AudioSource jumpAudio, hurtAudio, collectionAudio, gameoverAudio;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animat = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPress = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (IsHurt == false) 
        {
            Movement();
        }
        Jump();
        SwitchAnim();
    }

    void Movement() 
    {
        float horizontalmove = Input.GetAxisRaw("Horizontal"); ;
        rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);
        if (horizontalmove != 0) {
            transform.localScale = new Vector3(horizontalmove, 1, 1);
        }
        Crouch();
        Climb();
    }

    void SwitchAnim() 
    {
        animat.SetFloat("Running", Mathf.Abs(rb.velocity.x));
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
        if (isGround)
        {
            animat.SetBool("Falling", false);
            animat.SetBool("idle", true);
        } else if (!isGround && rb.velocity.y > 0)
        {
            animat.SetBool("Jumping", true);
        } else if (rb.velocity.y < 0)
        {
            animat.SetBool("Jumping", false);
            animat.SetBool("Falling", true);
        }
        if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(ladder))
        {
            animat.SetBool("Jumping", false);
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
        if (collistion.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke(nameof(Restart), 2f);
            gameoverAudio.Play();
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
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
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

    void Jump()
    {
        if (isGround)
        {
            if(Gem >= 3)
            {
                jumpCount = 4;
            } else
            {
                jumpCount = 2;
            }
            isJump = false;
        }
        if (jumpPress && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpAudio.Play();
            jumpCount--;
            jumpPress = false;
        } else if (jumpPress && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpAudio.Play();
            jumpCount--;
            jumpPress = false;
        }
    }

    private void Crouch()
    {
        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch"))
            {
                animat.SetBool("Crouching", true);
                disColl.enabled = false;
            } else
            {
                animat.SetBool("Crouching", false);
                disColl.enabled = true;
            }
        }
    }

    private void Climb()
    {
        if (coll.IsTouchingLayers(ladder))
        {
            if(animat.GetFloat("Running") > 0)
            {
                animat.SetFloat("Running", 0);
            }
            rb.gravityScale = 0.0f;
            animat.SetBool("Falling", false);
            animat.SetBool("Jumping", false);
            animat.SetBool("idle", true);
            float verticalMove = Input.GetAxis("Vertical");
            if(verticalMove != 0)
            {
                animat.SetBool("Climbing", true);
                rb.velocity = new Vector2(rb.velocity.x, verticalMove * 4);
            }
        } else
        {
            rb.gravityScale = 3.0f;
            animat.SetBool("Climbing", false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

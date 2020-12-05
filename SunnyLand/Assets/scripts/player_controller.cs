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
    public Joystick joy;

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
        }else if(joy.Vertical > 0.5f && jumpCount > 0)
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
        float horizontalmove;
        if (joy.Horizontal != 0)
        {
            horizontalmove = joy.Horizontal;
        } else
        {
            horizontalmove = Input.GetAxisRaw("Horizontal");
        }
        rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);
        if (horizontalmove > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }else if (horizontalmove < 0){
            transform.localScale = new Vector3(-1, 1, 1);
        }
        Crouch();
        Climb();
    }

    void SwitchAnim() 
    {
        animat.SetFloat("Running", Mathf.Abs(rb.velocity.x));
        if(IsHurt == true) 
        {
            animat.SetBool("Hurt", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f) 
            {
                IsHurt = false;
                animat.SetBool("Hurt", false);
            }
        }
        if (isGround)
        {
            animat.SetBool("Falling", false);
        } else if (!isGround && rb.velocity.y > 0.5f)
        {
            animat.SetBool("Jumping", true);
        } else if (rb.velocity.y < 0)
        {
            animat.SetBool("Jumping", false);
            animat.SetBool("Falling", true);
        }
        if (coll.IsTouchingLayers(ground) || coll.IsTouchingLayers(ladder) || disColl.IsTouchingLayers(ground))
        {
            animat.SetBool("Jumping", false);
            animat.SetBool("Falling", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collistion) 
    {
        if (collistion.tag == "Collection") 
        {
            Destroy(collistion.gameObject);
            Cherry++;
            CherryNum.text = Cherry.ToString();
            SoundManager.instance.ColleAudio();
        }
        else if (collistion.tag == "Gem")
        {
            Destroy(collistion.gameObject);
            Gem++;
            GemNum.text = Gem.ToString();
            SoundManager.instance.ColleAudio();
        }
        if (collistion.tag == "DeadLine")
        {
            SoundManager.instance.BGM.enabled = false;
            Invoke(nameof(Restart), 2f);
            SoundManager.instance.OverAudio();
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
                SoundManager.instance.HurtAudio();
                IsHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(7, rb.velocity.y);
                SoundManager.instance.HurtAudio();
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
            SoundManager.instance.JumpAudio();
            jumpCount--;
            jumpPress = false;
        } else if (jumpPress && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SoundManager.instance.JumpAudio();
            jumpCount--;
            jumpPress = false;
        }
    }

    private void Crouch()
    {
        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, ground))
        {
            if(joy.Vertical < -0.5f)
            {
                animat.SetBool("Crouching", true);
                disColl.enabled = false;
            }
            else if (Input.GetButton("Crouch"))
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
            float verticalMove;
            if(joy.Vertical != 0)
            {
                verticalMove = joy.Vertical;
            } else
            {
                verticalMove = Input.GetAxis("Vertical");
            }
            if(verticalMove > 0.5f || verticalMove < 0.5f)
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

    public bool joyTrue()
    {
        if(joy.Horizontal != 0)
        {
            return true;
        }else if(joy.Vertical != 0)
        {
            return true;
        } else
        {
            return false;
        }
    }
}

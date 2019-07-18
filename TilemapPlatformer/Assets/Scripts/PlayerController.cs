using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Movement Variables
    private Rigidbody2D rb2d;
    public float speed;
    public float jumpforce;

    
    // Text Variables
    public Text winText;
    public Text countText;
    public Text livesText;    
    private int count;
    private int lives;
    private bool hasReset=false;
    private bool hasWon=false;

    // Camera, Scene Jumping Variables
    public Camera sceneCamera;
    public Vector3 scene2;
    public Vector3 newOrigin;

    // Audio Variables
    public AudioClip backgroundMusic;
    public AudioClip winMusic;
    public AudioSource musicSource;

    // animation variables

    Animator anim;
    private bool facingRight = true;


    // ground checking variables
    public Transform groundCheckPoint; // used to prevent 'jumping' up the side of platforms
    public float groundCheckRadius;
    private bool isTouchingGround;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        count = 0;
        lives = 3;

        winText.text = "";

        SetCountText();

        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;  


    }

    // Setting Animation States
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }


    }

    void FixedUpdate()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;


        rb2d.AddForce(movement * speed);

        

        if(Input.GetButtonDown("Vertical") && isTouchingGround)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpforce);
            anim.SetTrigger("Jump");   
        }

        HandleLayers();


        // Flipping Code
        if (facingRight == false && moveHorizontal > 0)
            {
            Flip();
            }
        else if (facingRight == true && moveHorizontal < 0)
            {
            Flip();
            }
    

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }



    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
          other.gameObject.SetActive(false);
          count++;
          SetCountText();
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives--;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        livesText.text = "Lives: " + lives.ToString();
        
        if(count == 6 && hasReset == false)
        {
            lives = 3;
            livesText.text = "Lives: " + lives.ToString();
            sceneCamera.transform.position = scene2;
            gameObject.transform.position = newOrigin;
            hasReset = true;
        }
        if(count >= 10 && hasWon == false)
        {
            winText.text = "You win!";
            musicSource.clip = winMusic;
            musicSource.Play();
            hasWon = true;
        }
        if(lives == 0)
        {
            winText.text = "You lost! :(";
            gameObject.SetActive(false);
        }
    }
    void Flip()
    {
    facingRight = !facingRight;
    Vector2 Scaler = transform.localScale;
    Scaler.x = Scaler.x * -1;
    transform.localScale = Scaler;
    }

    private void HandleLayers()
    {
        if(!isTouchingGround)
        {
            anim.SetLayerWeight(1,1);
        }
        else
        {
            anim.SetLayerWeight(1,0);
        }
    }
}
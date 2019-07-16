using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;

    private int count;
    private int lives;

    public float speed;
    public float jumpforce;

    public Text winText;
    public Text countText;
    public Text livesText;


    public Transform groundCheckPoint; // used to prevent 'jumping' up the side of platforms
    public float groundCheckRadius;
    private bool isTouchingGround;
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        count = 0;
        lives = 3;

        winText.text = "";

        SetCountText();


    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;


        rb2d.AddForce(movement * speed);

        if(Input.GetButtonDown("Jump") && isTouchingGround)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpforce);
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
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        livesText.text = "Lives: " + lives.ToString();
        
        if(count >= 6)
        {
            winText.text = "You win!";
        }
        if (lives == 0)
        {
            winText.text = "You lost! :(";
            gameObject.SetActive(false);
        }
    }
}
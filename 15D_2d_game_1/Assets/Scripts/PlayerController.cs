using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Moving */
    Rigidbody2D rbody;
    float axisH = 0.0f;
    public float speed = 3.0f;
    bool isMoving = false;

    /* Jumping */
    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJump = false;
    bool onGround = false;

    /* Animator */
    Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerWin";
    public string deadAnime = "PlayerLose";
    string nowAnime = "";
    string oldAnime = "";

    /* Others */
    public static string gameState = "playing";
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        nowAnime = stopAnime;
        oldAnime = stopAnime;

        gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != "playing")
        {
            return;
        }
        /* Moving */
        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }
        if (axisH > 0.0f)
        {
            // Go right
            Debug.Log("Go right.");
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            // Go left
            Debug.Log("Go left.");
            transform.localScale = new Vector2(-1, 1);
        }
        /* Jump */
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Start Jump!");
            Jump();
        }
    }

    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }
        // check on-ground
        onGround = Physics2D.Linecast(transform.position,
                                      transform.position - (transform.up * 0.6f),
                                      groundLayer);
        // speed update 
        if (onGround || axisH != 0)
        {
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }
        Debug.Log("Jump: " + goJump + "; onGround: " + onGround);
        // jump update
        if (onGround && goJump)
        {
            Debug.Log("Jump");
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }
        // // Anime update
        // if (onGround)
        // {
        //     if (axisH == 0)
        //     {
        //         nowAnime = stopAnime;
        //     }
        //     else
        //     {
        //         nowAnime = moveAnime;
        //     }
        // }
        // else
        // {
        //     nowAnime = jumpAnime;
        // }

        // if (nowAnime != oldAnime)
        // {
        //     oldAnime = nowAnime;
        //     animator.Play(nowAnime);
        // }
    }

    public void Jump()
    {
        goJump = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Win")
        {
            Goal();
        }
        else if (collision.gameObject.tag == "Lose")
        {
            GameOver();
        }
        // else if (collision.gameObject.tag == "ScoreItem")
        // {
        //     ItemData item = collision.gameObject.GetComponent<ItemData>();
        //     score = item.value;
        //     Destroy(collision.gameObject);
        // }
    }

    // win
    public void Goal()
    {
        animator.Play(goalAnime);
        gameState = "gameclear";
        GameStop();
    }
    // lose
    public void GameOver()
    {
        animator.Play(deadAnime);
        gameState = "gameover";
        GameStop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
    }
    // stop moving when win || over
    void GameStop()
    {
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = new Vector2(0, 0);
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        if (axisH == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }
}

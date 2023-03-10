using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    float horizontalMove;
    public float setSpeed;
    public float speed = 0;
    public Sprite jump1Sprite;
    public Sprite jump2Sprite;
    public Sprite fallSprite;
    public Sprite defeatSprite;
    public bool respawn = true;
    public float respawnTime = 4f;
    float respawnTimer;
    bool respawning = false;
    public float respawnJump = 2f;

    Vector3 initialPos;


    Rigidbody2D myBody;

    bool jumping = false;

    public float castDist = 2f;
    public float gravityScale = 4.5f;
    public float gravityFall = 8f;
    public float jumpLimit = 1f;
    //bool bounce = false;

    public GameObject otherPlayer;
    public bool defeat = false;
    public int score;

    SpriteRenderer myRenderer;

    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        respawnTimer = respawnTime;
        initialPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!respawn && !defeat)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            speed = setSpeed;
            //Debug.Log(horizontalMove);

            Vector3 newScale = transform.localScale;
            if (horizontalMove > 0)
            {
                if (gameObject.name == "player1" && Input.GetKey(KeyCode.RightArrow))
                {
                    newScale.x = 1;
                } else if (gameObject.name == "player2" && Input.GetKey(KeyCode.D))
                {
                    newScale.x = 1;
                } else
                {
                    horizontalMove = 0;
                }
            }
            else if (horizontalMove < 0)
            {
                if (gameObject.name == "player1" && Input.GetKey(KeyCode.LeftArrow))
                {
                    newScale.x = -1;
                }
                else if (gameObject.name == "player2" && Input.GetKey(KeyCode.A))
                {
                    newScale.x = -1;
                }
                else
                {
                    horizontalMove = 0;
                }

            }
            Debug.Log(horizontalMove);
            transform.localScale = newScale;
        } else if (respawn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                respawnTimer = respawnTime;
                respawn = false;
                respawning = true;

            }
        } else if (defeat)
        {
            myRenderer.sprite = defeatSprite;
        }
    }

    void FixedUpdate()
    {
        
        Vector3 currentPos = transform.position;

        if (currentPos.x >= 1.6f)
        {
            horizontalMove = -1;
        } else if (currentPos.x <= -1.6f)
        {
            horizontalMove = 1;
        }

        float moveSpeed = horizontalMove * speed;

        if (jumping)
        {
            Vector3 newVelocity = myBody.velocity;
            newVelocity.y = 0;
            myBody.velocity= newVelocity;
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jumping = false;
        }

        if (respawning)
        {
            myBody.AddForce(Vector2.up * respawnJump, ForceMode2D.Impulse);
            respawning = false;
        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
            myRenderer.sprite = jump1Sprite;

        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
            myRenderer.sprite = fallSprite;
        }

        /*

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        if (hit.collider != null && hit.transform.name == otherPlayer.name)
        {
            score += 1;
            jumping = true;
            myRenderer.sprite = jump2Sprite;
            otherPlayer.GetComponent<playerManager>().defeat = true;
            otherPlayer.GetComponent<playerManager>().speed = 0;
        }
        */

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cloud"))
        {
            if (myBody.velocity.y < 0 && !defeat)
            {
                collision.gameObject.GetComponent<cloudManager>().animator.SetBool("gone", true);
                jumping = true;
                myRenderer.sprite = jump2Sprite;

            }
        }

        if (collision.gameObject.CompareTag("Floor") && (myBody.velocity.y < 0))
        {
            score -= 1;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "respawn")
        {
            respawn = true;
            Vector3 newPos = transform.position;
            newPos.x = initialPos.x;
            transform.position = newPos;
            speed = 0;
            defeat = false;
        }

        if (collision.gameObject.CompareTag("Player") && (myBody.velocity.y < 0))
        {
            score += 1;
            jumping = true;
            myRenderer.sprite = jump2Sprite;
            otherPlayer.GetComponent<playerManager>().defeat = true;
            otherPlayer.GetComponent<playerManager>().speed = 0;
        }
    }
}

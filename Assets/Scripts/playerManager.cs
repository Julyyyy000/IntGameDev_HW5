using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    float horizontalMove;
    public float setSpeed;
    float speed = 0;
    public Sprite jump1Sprite;
    public Sprite jump2Sprite;
    public Sprite fallSprite;
    bool respawn = true;
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
        if (!respawn)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            speed = setSpeed;
            //Debug.Log(horizontalMove);

            Vector3 newScale = transform.localScale;
            if (horizontalMove > 0)
            {
                newScale.x = 1;
            }
            else if (horizontalMove < 0)
            {
                newScale.x = -1;
            }
            transform.localScale = newScale;
        } else
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                respawnTimer = respawnTime;
                respawn = false;
                respawning = true;

            }
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

        if (hit.collider != null && hit.transform.CompareTag("Cloud"))
        {

        }
        */
        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cloud"))
        {
            if (myBody.velocity.y < 0)
            {
                Destroy(collision.gameObject);
                jumping = true;
                myRenderer.sprite = jump2Sprite;

            }
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
        }
    }
}

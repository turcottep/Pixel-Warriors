using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxSpeed = 2.5f;
    public float speed = 15f;
    public float jumpPower = 175f;
    public float maxJump = 2f;
    public float percentage = 0f;
    public int lives = 3;

    public bool grounded;
    public bool attack_1;
    public bool charge;
    public bool goingDown;
    public bool dead;

    private int x = 0;
    private bool isRight;
    private bool isDead;
    private float stun = 0f;

    private Rigidbody2D rb2d;
    private Animator anim;
    private Player player;

    private Vector2 pos;
    private Vector2 knockback;

    void Start()
    {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();

        //Solution temporaire. À changer selon la direction de l'attaque de l'autre joueur
        knockback.Set(-2, 0);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball2")
        {
            //Hit by an ennemy ball
            Destroy(col.gameObject);
            rb2d.AddForce(col.rigidbody.velocity * percentage, ForceMode2D.Impulse);
            percentage += 0.75f;
            stun = 10 + (.5f*percentage);
            Debug.Log("pourcentage P1: " + percentage);
        }

        //Hit by melee
        if (col.gameObject.tag == "Melee2")
        {
            player.transform.position = pos;
            Destroy(col.gameObject);
            rb2d.AddForce(knockback, ForceMode2D.Impulse);
            percentage += 0.3f;
            stun = 10 + (.5f * percentage);
            Debug.Log("pourcentage P1: " + percentage);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        player.grounded = true;
        maxJump = 2;
        maxSpeed = 2.5f;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        player.grounded = true;
        maxJump = 2;
        maxSpeed = 2.5f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.grounded = false;
    }

    void Update()
    {
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Attack", attack_1);
        anim.SetBool("Charge", charge);
        anim.SetBool("GoingDown", goingDown);
        anim.SetBool("Dead", dead);

        //Flip character L/R
        if (isRight == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (isRight == true)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //Double jump
        if (Input.GetKeyDown(KeyCode.W) && maxJump > 0)
        {
            maxSpeed = 2f;

            Vector2 temp = rb2d.velocity;
            temp.y = 0;
            rb2d.velocity = new Vector2(temp.x, temp.y);
            rb2d.AddForce(new Vector2(0, jumpPower));
            maxJump--;
        }

        //Attack 1
        if (Input.GetKeyDown(KeyCode.R))
        {
            attack_1 = true;
        }
        else { attack_1 = false; }

        //Attack 2
        if (Input.GetKey(KeyCode.F)) { charge = true; }
        else { charge = false; }

        //Gauche/Droite
        if (Input.GetKey(KeyCode.A) && rb2d.velocity.x > -maxSpeed) { x = -1; isRight = false; }
        else if (Input.GetKey(KeyCode.D) && rb2d.velocity.x < maxSpeed) { x = 1; isRight = true; }
        else { x = 0; }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float decay = 0.8f;

        pos = transform.position;

        //Out of map
        if (rb2d.transform.position.y < -1 || rb2d.transform.position.y > 3.2 || rb2d.transform.position.x > 2.7 || rb2d.transform.position.x < -4.5)
        {
            player.isDead = true;
            lives--;
        }
        if (player.isDead == true)
        {
            if (lives == 0)
            {
                Destroy(player);
            }
            else
            {
                player.dead = true;
                percentage = 0;
                player.transform.position = new Vector3(-2, 1.6f, 0);
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                    player.isDead = false;
                    player.dead = false;
                }
            }
        }

        //Going down
        if (rb2d.velocity.y < 0) { player.goingDown = true; }
        else { player.goingDown = false; }

        //Move player
        if (stun < 1)
        { rb2d.AddForce(Vector2.right * x * 10 * speed, ForceMode2D.Force); }
        else { stun--; }
        rb2d.velocity = new Vector2(rb2d.velocity.x * decay, rb2d.velocity.y);

        //Limit speed
        //if (rb2d.velocity.x > maxSpeed)
        //{
        //    rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        //}

        //if(rb2d.velocity.x < -maxSpeed)
        //{
        //    rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        //}
    }


}

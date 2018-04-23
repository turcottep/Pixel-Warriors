using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    public int playerType;

    public float maxSpeed = 2.5f;
    public float speed = 15f;
    public float jumpPower = 175f;
    public float maxJump = 2f;
    public float percentage = 0f;

    //Animations
    public bool grounded;
    public bool basic_1;
    public bool basic_2;
    public bool basic_3;
    public bool special_1;
    public bool special_2;
    public bool special_3;
    public bool charge;
    public bool goingDown;
    public bool dead;
    public bool stunned;


    //controls
    public KeyCode up = KeyCode.W;
    public bool pressUp = false;
    public KeyCode jump = KeyCode.Space;
    public KeyCode left = KeyCode.A;
    public KeyCode down = KeyCode.S;
    public bool pressDown = false;
    public KeyCode right = KeyCode.D;
    public KeyCode A = KeyCode.R;
    public KeyCode B = KeyCode.F;

    //ui
    public bool isButtonLeftPointerDown;
    public bool isButtonRightPointerDown;
    public bool isButtonDownPointerDown;
    public bool isButtonAttackBPointerDown;

    public bool aiON = true;
    public int x = 0;

    public Vector3 initialPosition = new Vector3(-2, 1.6f, 0);

    public bool isRight;
    public bool isDead;
    private float stun = 0f;

    public Rigidbody2D rb2d;
    private Animator anim;
    private Player player;

    private Vector2 pos;
    private Vector2 knockback;

    private int playerNum;

    private GameObject manager;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();
        manager = GameObject.FindGameObjectWithTag("Manager");

        if (player.tag == "Player 1")
        {
            playerNum = 1;
        }
        else if (player.tag == "Player 2")
        {
            playerNum = 2;
        }
        else
        {
            Debug.Log("ERREUR TAG JOUEURS");
        }


        //Solution temporaire. À changer selon la direction de l'attaque de l'autre joueur
        knockback.Set(-2, 1);
        manager.GetComponent<Manager>().UpdatePercentages();
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        //Hit by attacks
        if ((player.tag == "Player 1" && col.gameObject.tag == "AttPlayer2") || (player.tag == "Player 2" && col.gameObject.tag == "AttPlayer1"))
        {
            //player.transform.position = pos;
            if (col.gameObject.name == "Ninja_Bomb(Clone)")
            {
                player.GetComponent<Attacks>().Explode(col.gameObject);
            }
            else if (!(col.gameObject.name == "Demon_Small_Bone(Clone)") && !(col.gameObject.name == "Demon_Big_Bone(Clone)")) Destroy(col.gameObject);

            float d = col.gameObject.GetComponent<Damage>().getDamage();
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine("whitecolor");
            this.ReceiveDamage(10, d);
            basic_1 = false;
            basic_2 = false;
            basic_3 = false;
            special_1 = false;
            special_2 = false;
            special_3 = false;
            charge = false;

        }

        //Lava
        if (col.gameObject.tag == "Lava")
        {
            charge = false;
            this.rb2d.velocity = new Vector2(0, 6);
            this.maxJump = 2;
            this.percentage += 0.5f;
            manager.GetComponent<Manager>().UpdatePercentages();
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine("whitecolor");
        }
    }

    IEnumerator whitecolor()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (rb2d.velocity.y <= 0)
        {
            player.grounded = true;
            maxJump = 2;
            maxSpeed = 2.5f;
        }
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
        anim.SetBool("Basic1", basic_1);
        anim.SetBool("Basic2", basic_2);
        anim.SetBool("Basic3", basic_3);
        anim.SetBool("Special1", special_1);
        anim.SetBool("Special2", special_2);
        anim.SetBool("Special3", special_3);
        anim.SetBool("Charge", charge);
        anim.SetBool("GoingDown", goingDown);
        anim.SetBool("Dead", dead);
        anim.SetBool("Stunned", stunned);

        //Flip character L/R
        if (isRight == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (isRight == true)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKeyDown(jump))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(down))
        {
            MoveDown();
        }


        //////////////////////////////////ATTACKS

        //A
        if (Input.GetKeyDown(A) && pressUp) // A + ↑
        {
            Basic2();
        }
        else if (Input.GetKeyDown(A) && pressDown) // A + ↓
        {
            basic_2 = false;
            Basic3();
        }
        else if (Input.GetKeyDown(A)) // A + ← →
        {
            basic_3 = false;
            Basic1();
        }
        else
        {
            basic_1 = false;
            basic_2 = false;
            basic_3 = false;
        }

        //B
        if ((Input.GetKeyDown(B) || isButtonAttackBPointerDown) && pressUp) // B + ↑
        {
            Special2();
        }
        else if ((Input.GetKeyDown(B) || isButtonAttackBPointerDown) && pressDown) // B + ↓
        {
            special_2 = false;
            Special3();
        }
        else if ((Input.GetKeyDown(B) || isButtonAttackBPointerDown)) // B + ← →
        {
            special_3 = false;
            Special1(true);
        }
        else if (Input.GetKeyUp(B)) // B + ← →
        {
            Special1(false);
            special_1 = false;
        }
        

        if (Input.GetKeyDown(down)) // B + ↓
        {
            pressDown = true;
        }
        else if (Input.GetKeyUp(down)) // B + ↓
        {
            pressDown = false;
        }

        if (Input.GetKeyDown(up)) // B + ↓
        {
            pressUp = true;
        }
        else if (Input.GetKeyUp(up)) // B + ↓
        {
            pressUp = false;
        }

        //Gauche/Droite
        if ((Input.GetKey(left) || isButtonLeftPointerDown) && rb2d.velocity.x > -maxSpeed) { MoveLeft(); }
        else if ((Input.GetKey(right) || isButtonRightPointerDown) && rb2d.velocity.x < maxSpeed) { MoveRight(); }
        else { x = 0; }//if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) { x = 0; }

    }


    private void FixedUpdate()
    {

        if (aiON) player.GetComponent<AI>().AIUpdate();


        float h = Input.GetAxisRaw("Horizontal");
        float decay = 0.8f;

        pos = transform.position;

        //Out of map
        if (rb2d.transform.position.y < -2.1f || rb2d.transform.position.y > 3.2 || rb2d.transform.position.x > 4.5f || rb2d.transform.position.x < -4.5)
        {
            player.isDead = true;
            manager.GetComponent<Manager>().PlayerDeath(playerNum);
        }
        if (player.isDead)
        {
            this.Reset();
        }


        //Going down
        if (rb2d.velocity.y < 0) { player.goingDown = true; }
        else { player.goingDown = false; }

        //Move player
        if (stun < 1)
        { rb2d.AddForce(Vector2.right * x * 10 * speed, ForceMode2D.Force); }
        else { stun--; }
        rb2d.velocity = new Vector2(rb2d.velocity.x * decay, rb2d.velocity.y);

    }


    //MOVES

    public void MoveUp()
    {
        if (this.isDead) this.Revive();
        if (maxJump > 0)
        {
            maxSpeed = 2f; //ENLEVER?
            Vector2 temp = rb2d.velocity;
            temp.y = 0;
            rb2d.velocity = new Vector2(temp.x, temp.y);
            rb2d.AddForce(new Vector2(0, jumpPower));
            maxJump--;
        }

    }
    public void MoveLeft()
    {
        x = -1; isRight = false;
    }

    public void MoveRight()
    {
        x = 1; isRight = true;
    }

    public void MoveDown()
    {
        if (this.isDead) this.Revive();
    }

    public void Basic1()
    {
        basic_1 = true;
        player.GetComponent<Attacks>().LaunchBasic1(playerNum);
    }
    public void Basic2()
    {
        basic_2 = true;
        player.GetComponent<Attacks>().LaunchBasic2(playerNum);
    }
    public void Basic3()
    {
        basic_3 = true;
        player.GetComponent<Attacks>().LaunchBasic3(playerNum);
    }

    public void Special1(bool state)
    {

        player.GetComponent<Attacks>().LaunchSpecial1(playerNum, state);

    }
    public void Special2()
    {
        special_2 = true;
        player.GetComponent<Attacks>().LaunchSpecial2(playerNum);
    }
    public void Special3()
    {
        special_3 = true;
        player.GetComponent<Attacks>().LaunchSpecial3(playerNum);
    }

    //Ajouter l'effet stun pour l'animation
    public void ReceiveDamage(int stunReceived, float damage)
    {
        int dir = 0;
        if (isRight) dir = 1;
        else dir = -1;
        rb2d.AddForce(knockback * dir * percentage, ForceMode2D.Impulse);
        percentage += damage;
        stun = stunReceived + (percentage);
        //stunned = true;
        manager.GetComponent<Manager>().UpdatePercentages();
    }

    public void Reset()
    {
        player.dead = true;
        percentage = 0;
        manager.GetComponent<Manager>().UpdatePercentages();
        player.transform.position = initialPosition;
        rb2d.velocity = new Vector2(0, 0);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        if (Input.GetKey(jump) || Input.GetKey(down))
        {
            Debug.Log("revit");
            rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            player.isDead = false;
            player.dead = false;
        }
    }

    public void Revive()
    {

    }


}
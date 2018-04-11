﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public bool shootCharge;
    public bool canShoot = true;

    //controls
    public KeyCode up = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode down = KeyCode.S;
    public KeyCode right = KeyCode.D;
    public KeyCode attack1 = KeyCode.R;
    public KeyCode attack2 = KeyCode.F;
    public KeyCode attack3 = KeyCode.C;

    //ui
    public bool isButtonLeftPointerDown;
    public bool isButtonRightPointerDown;
    public bool isButtonDownPointerDown;
    public TextMeshProUGUI textPercentage;
    public TextMeshProUGUI initElo;
    public GameObject panelWin;
    public GameObject panelLost;
    private int elo= 1200;
    private int eloEnnemi = 1400;
    public TextMeshProUGUI deltaElo;
    public TextMeshProUGUI sumElo;
    public TextMeshProUGUI cash;
    public GameObject canvas;
    public float timeLeft = 60;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    public bool aiON = true;
    public int x = 0;

    public Vector3 initialPosition = new Vector3(-2, 1.6f, 0);

    public bool isRight;
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
        knockback.Set(-2, 1);
        
        setPercentageText();
        updateLifeDisplay();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Hit by an ennemy projectile
        if ((player.tag == "Player 1" && col.gameObject.tag == "Ball2") || (player.tag == "Player 2" && col.gameObject.tag == "Ball1"))
        {
            Destroy(col.gameObject);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine("whitecolor");
            this.ReceiveDamage(10, 0.75f);
            //rb2d.AddForce(col.rigidbody.velocity * percentage, ForceMode2D.Impulse);
            //percentage += 0.75f;
            //stun = 10 + (.5f * percentage);
            //Debug.Log("pourcentage P1: " + percentage);
        }

        //Hit by melee
        if ((player.tag == "Player 1" && col.gameObject.tag == "Melee2") || (player.tag == "Player 2" && col.gameObject.tag == "Melee1"))
        {
            //player.transform.position = pos;
            Destroy(col.gameObject);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine("whitecolor");
            this.ReceiveDamage(10, 0.75f);

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
        if (Input.GetKeyDown(up))
        {
            MoveUp();
        }

        //Attack 1
        if (Input.GetKeyDown(attack1))
        {
            Attack1();
        }
        else { attack_1 = false; }

        //Attack 2
        if (Input.GetKeyDown(attack2)) { Attack2(true); }
        else if (Input.GetKeyUp(attack2)) { Attack2(false); }

        //Gauche/Droite
        if ((Input.GetKey(left) || isButtonLeftPointerDown) && rb2d.velocity.x > -maxSpeed) { MoveLeft(); }
        else if ((Input.GetKey(right) || isButtonRightPointerDown) && rb2d.velocity.x < maxSpeed) { MoveRight(); }
        else { x = 0; }//if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) { x = 0; }

        //Timer
        timeLeft -= Time.deltaTime;
        //timer.text = timeLeft.ToString("f0");
    }


    private void FixedUpdate()
    {

        //if (aiON) player.GetComponent<AI>().AIUpdate();


        float h = Input.GetAxisRaw("Horizontal");
        float decay = 0.8f;

        pos = transform.position;

        //Out of map
        if (rb2d.transform.position.y < -2.1f || rb2d.transform.position.y > 3.2 || rb2d.transform.position.x > 4.5f || rb2d.transform.position.x < -4.5)
        {
            player.isDead = true;
            lives--;
            updateLifeDisplay();
        }
        if (player.isDead == true)
        {
            if (lives == 0)
            {
                Destroy(player);
            }
            else
            {
                this.Reset();
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

    }

    
    //MOVES

    public void MoveUp()
    {
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

    }

    public void Attack1()
    {
        attack_1 = true;
        player.GetComponent<Melee1_p1>().launch();
    }
    public void Attack2(bool state)
    {
        if (state)
        {
            shootCharge = true;
            player.GetComponent<Shoot>().animate();
        }
        else if (shootCharge)
        {
            shootCharge = false;
            player.GetComponent<Shoot>().shoot();
        }
    }

    public void Attack3()
    {

    }

    public void ReceiveDamage(int stunReceived, float damage)
    {
        int dir = 0;
        if (isRight) dir = 1;
        else dir = -1;
        rb2d.AddForce(knockback * dir * percentage, ForceMode2D.Impulse);
        percentage += damage;
        stun = stunReceived + (percentage);
        setPercentageText();
    }

    public void Reset()
    {
        player.dead = true;
        percentage = 0;
        setPercentageText();
        player.transform.position = initialPosition;
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        if (Input.GetKey(up) || Input.GetKey(down))
        {
            rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            player.isDead = false;
            player.dead = false;
        }
    }

    ////UI Will
    //public void buttonJumpPointerDown()
    //{
    //    rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
    //    player.isDead = false;
    //    player.dead = false;

    //    this.MoveUp();
    //}

    //public void buttonAttackAPointerDown()
    //{
    //    this.Attack1();
    //}

    //public void buttonAttackBPointerDown()
    //{
    //    this.Attack2(true);
    //}

    //public void buttonAttackBPointerUp()
    //{
    //    Attack2(false);
    //}

    //public void buttonLeftPointerDown()
    //{
    //    isButtonLeftPointerDown = true;
    //}

    //public void buttonLeftPointerUp()
    //{
    //    isButtonLeftPointerDown = false;
    //}

    //public void buttonRightPointerDown()
    //{
    //    isButtonRightPointerDown = true;
    //}

    //public void buttonRightPointerUp()
    //{
    //    isButtonRightPointerDown = false;
    //}

    //public void buttonDownPointerDown()
    //{
    //    this.MoveDown();
    //}

    //public void buttonUpPointerDown()
    //{
    //    this.MoveUp();
    //}

    public void setPercentageText()
    {
        //textPercentage.text = (20 * percentage).ToString() + "%";
    }

    public void updateLifeDisplay()
    {
        if (lives == 3)
        {
            //life1.SetActive(true);
            //life2.SetActive(true);
            //life3.SetActive(true);
        }
        else if (lives == 2)
        {
            //life1.SetActive(true);
            //life2.SetActive(true);
            //life3.SetActive(false);
        }
        else if (lives == 1)
        {
            //life1.SetActive(true);
            //life2.SetActive(false);
            //life3.SetActive(false);
        }
        else if (lives == 0)
        {
            
            //life1.SetActive(false);
            //life2.SetActive(false);
            //life3.SetActive(false);

            EndGame(0);
        }
    }

    public void EndGame(int result)
    {
        canvas.SetActive(true);
        if (result == 1)
        {
            panelWin.SetActive(true);
        }
        else
        {
            panelLost.SetActive(true);
        }
        initElo.SetText(elo.ToString());
        float temp = Mathf.Floor(20 * (result - 1 / (1 + Mathf.Pow(10, (-1 * (elo - eloEnnemi) / 40)))));

        deltaElo.SetText(temp.ToString());
        sumElo.SetText((elo + temp).ToString());
        cash.SetText((200 + 10 * temp).ToString());
    }


}
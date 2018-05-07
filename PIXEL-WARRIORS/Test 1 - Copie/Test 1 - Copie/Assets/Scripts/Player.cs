﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : Photon.PunBehaviour, IPunObservable
{
    public int playerType;
    public int mapNumber;

    public float maxSpeed = 2.5f;
    public float speed = 15f;
    public float jumpPower = 175f;
    public float maxJump = 2f;
    public float percentage = 0f;
    public float multiplier = 1f;

    private float chargeTime = 0;
    private float chargePercentage = 0;
    private bool isCharging = false;

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

    private GameObject chargeBar1;
    private bool isCreated1 = false;
    private GameObject chargeBar2;
    private bool isCreated2 = false;
    private GameObject chargeBar3;
    private bool isCreated3 = false;

    //Audio
    private AudioManager audioManager;

    //controls
    public bool controls = true;
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

    public int playerNum;

    public GameObject manager;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();
        manager = GameObject.FindGameObjectWithTag("Manager");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        mapNumber = manager.GetComponent<Manager>().mapN;
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
            Debug.Log("ERREUR TAG JOUEURS"); //fixed by manually changing playernumber in manager
        }


        //Solution temporaire. À changer selon la direction de l'attaque de l'autre joueur
        knockback.Set(-2, 1);
        manager.GetComponent<Manager>().UpdatePercentages(playerNum);
    }



    IEnumerator Poison(int time)
    {
        yield return new WaitForSeconds(time);
        this.GetComponent<SpriteRenderer>().color = Color.green;
        StartCoroutine("Whitecolor");
        this.ReceiveDamage(0, 0.25f, false, new Vector2(0, 0));
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        Color damageColor = Color.red;
        //Hit by attacks
        if ((player.tag == "Player 1" && col.gameObject.tag == "AttPlayer2") || (player.tag == "Player 2" && col.gameObject.tag == "AttPlayer1"))
        {

            //TEST
            // CRASH HIT BOMB EXPLOSION
            Vector2 vecteurTest = new Vector2(0, 0);
            if (!(col.gameObject.name == "Scientist_Poison(Clone)") && !(col.gameObject.name == "Ninja_Bomb(Clone)" && !(col.gameObject.name == "Ninja_Explosion(Clone)")))
            {
                //Debug.Log("NAME:" + col.gameObject.name);
                vecteurTest = rb2d.position - col.rigidbody.position;
                vecteurTest = new Vector2(vecteurTest.x, (0.1f));
                vecteurTest = vecteurTest * multiplier;
            }
            //

            player.transform.position = pos;
            if (col.gameObject.name == "Ninja_Bomb(Clone)")
            {
                player.GetComponent<Attacks>().Explode(col.gameObject);
            }
            else if (col.gameObject.name == "Scientist_Poison(Clone)")
            {
                damageColor = Color.green;
                StartCoroutine("Poison", 1);
                StartCoroutine("Poison", 2);
                StartCoroutine("Poison", 3);
            }
            else if (!(col.gameObject.name == "Demon_Small_Bone(Clone)") && !(col.gameObject.name == "Demon_Big_Bone(Clone)"))
            {
                Destroy(col.gameObject);
            }

            float damage = col.gameObject.GetComponentInParent<Damage>().getDamage();
            this.GetComponent<SpriteRenderer>().color = damageColor;
            StartCoroutine("Whitecolor");

            if (col.gameObject.GetComponent<Damage>().attackType == 1)
            {
                this.ReceiveDamage(0, damage, true, new Vector2(0, 0));
            }
            else
            {
                this.ReceiveDamage(10, damage, true, vecteurTest);
            }

            basic_1 = false;
            basic_2 = false;
            basic_3 = false;
            special_1 = false;
            special_2 = false;
            special_3 = false;
            charge = false;

            audioManager.Play("Hit", 0);
        }

        //Lava
        if (col.gameObject.tag == "Lava")
        {

            charge = false;
            this.rb2d.velocity = new Vector2(0, 6);
            this.maxJump = 2;
            this.percentage += 0.5f;
            manager.GetComponent<Manager>().UpdatePercentages(playerNum);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine("Whitecolor");

            audioManager.Play("Lava", 0);

        }

        //if (col.gameObject.tag == "Bounce")
        //{
        //    Debug.Log("x=:" + rb2d.velocity.x + " y=" + rb2d.velocity.y);
        //    this.rb2d.velocity = this.rb2d.velocity * -1;
        //    Debug.Log("x=:" + rb2d.velocity.x + " y=" + rb2d.velocity.y);
        //    StartCoroutine("Whitecolor");
        //}
    }

    IEnumerator Whitecolor()
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


        if (!player.stunned && ((photonView.isMine && PhotonNetwork.connected == true) || manager.GetComponent<Manager>().getGameMode() == 1))
        {

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

            //Work in progress (à finir Mercredi)
            if (isCharging)
            {
                chargeTime += Time.deltaTime;
                Debug.Log("Charging : " + chargeTime);

                if(chargeTime < 1 && !isCreated1)
                {
                    GameObject bar1 = Instantiate(Resources.Load("ChargeBar1"), new Vector2(player.pos.x - 0.034655f, player.pos.y + 0.2713515f), Quaternion.identity) as GameObject;
                    chargeBar1 = bar1;
                    chargeBar1.transform.position = new Vector2(player.pos.x - 0.034655f, player.pos.y + 0.2713515f);
                    isCreated1 = true;
                }
                if (chargeTime > 1 && chargeTime < 2 && !isCreated2)
                {
                    GameObject bar2 = Instantiate(Resources.Load("ChargeBar2"), new Vector2(player.pos.x + 0.015345f, player.pos.y + 0.2713515f), Quaternion.identity) as GameObject;
                    chargeBar2 = bar2;
                    isCreated2 = true;
                }
                if (chargeTime > 2 && !isCreated3)
                {
                    GameObject bar3 = Instantiate(Resources.Load("ChargeBar3"), new Vector2(player.pos.x + 0.065345f, player.pos.y + 0.2713515f), Quaternion.identity) as GameObject;
                    chargeBar3 = bar3;
                    isCreated3 = true;
                }

                if (chargeTime > 2.7f && chargeTime < 2.8f)
                {
                    chargePercentage = 2.8f;
                    isCharging = false;
                }
                else
                {
                    chargePercentage = chargeTime;
                }
            }


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
                isCharging = true;

                special_3 = false;
                Special1(true, chargePercentage);
            }
            else if (Input.GetKeyUp(B) || chargePercentage == 2.8f) // B + ← →
            {
                isCharging = false;

                Special1(false, chargePercentage);
                special_1 = false;

                if (chargeBar1 != null) { Destroy(chargeBar1); }
                isCreated1 = false;
                if (chargeBar2 != null) { Destroy(chargeBar2); }
                isCreated2 = false;
                if (chargeBar3 != null) { Destroy(chargeBar3); }
                isCreated3 = false;

                chargePercentage = 0;
                chargeTime = 0;
            }

            if (Input.GetKeyDown(down)) // B + ↓
            {
                pressDown = true;
            }
            else if (Input.GetKeyUp(down)) // B + ↓
            {
                pressDown = false;
            }

            if (Input.GetKeyDown(up)) // B + ↑
            {
                pressUp = true;
            }
            else if (Input.GetKeyUp(up)) // B + ↑
            {
                pressUp = false;
            }

            //Gauche/Droite
            if ((Input.GetKey(left) || isButtonLeftPointerDown) && rb2d.velocity.x > -maxSpeed) { MoveLeft(); }
            else if ((Input.GetKey(right) || isButtonRightPointerDown) && rb2d.velocity.x < maxSpeed) { MoveRight(); }
            else { x = 0; }//if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) { x = 0; }

        }
    }

    private void FixedUpdate()
    {

        if (aiON) player.GetComponent<AI>().AIUpdate();


        //float h = Input.GetAxisRaw("Horizontal");
        float decay = 0.8f;

        pos = transform.position;

        //Out of map
        if (((mapNumber == 1) && (rb2d.transform.position.y < -2.4f || rb2d.transform.position.y > 3.2f || rb2d.transform.position.x > 4.6f || rb2d.transform.position.x < -4.6f)) || ((mapNumber == 2) && (rb2d.transform.position.y < -2.5f || rb2d.transform.position.y > 3.4f || rb2d.transform.position.x > 4.8f || rb2d.transform.position.x < -4.8f)))
        {

            player.isDead = true;
            manager.GetComponent<Manager>().PlayerDeath(playerNum);
            audioManager.Play("Death", 0);

        }
        if (player.isDead)
        {
            this.Reset();
        }


        //Going down
        if (rb2d.velocity.y < 0) { player.goingDown = true; }
        else { player.goingDown = false; }

        //Move player

        if (!player.stunned)
        {
            rb2d.AddForce(Vector2.right * x * 10 * speed, ForceMode2D.Force);
            rb2d.velocity = new Vector2(rb2d.velocity.x * decay, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y);
        }

    }


    //MOVES

    public void MoveUp()
    {
        if (this.isDead) this.Revive();
        if (maxJump > 0)
        {

            maxSpeed = 2f; //ENLEVER?
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0, jumpPower));
            maxJump--;

            audioManager.Play("Jump", 0);

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

    public void Special1(bool state, float charge)
    {

        player.GetComponent<Attacks>().LaunchSpecial1(playerNum, state, charge);

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

    public void ReceiveDamage(int stunReceived, float damage, bool knocksback, Vector2 vecteurTest)
    {
        int dir;
        if (isRight) dir = 1;
        else dir = -1;
        //Debug.Log("knockbackof" + knockback * dir * percentage * stun / 10);
        rb2d.velocity = Vector2.zero;
        if (knocksback)
        {
            rb2d.AddForce(new Vector2(damage * vecteurTest.x * (percentage + 20), vecteurTest.y * (percentage + 20)), ForceMode2D.Impulse);
        }
        percentage += 10 * damage;
        stun = stunReceived * (percentage) / 10;
        player.stunned = true;
        this.gameObject.layer = 14 + playerNum;
        StartCoroutine("Stun", stun);
        manager.GetComponent<Manager>().UpdatePercentages(playerNum);
    }
    IEnumerator Stun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration / 30);
        player.stunned = false;
        player.gameObject.layer = 7 + playerNum;
        Debug.Log("unstunned");
    }

    public void Reset()
    {
        player.dead = true;
        player.special_1 = false;
        player.special_2 = false;
        player.special_3 = false;
        player.stunned = false;
        percentage = 0;
        manager.GetComponent<Manager>().UpdatePercentages(playerNum);
        player.transform.position = initialPosition;
        rb2d.velocity = new Vector2(0, 0);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        if (Input.GetKey(jump) || Input.GetKey(down))
        {
            //Debug.Log("revit");
            rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            player.isDead = false;
            player.dead = false;
        }
    }

    public void Revive()
    {

    }

    private void Awake()
    {

        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.isMine)
        {
            Player.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        //DontDestroyOnLoad(this.gameObject);

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("marche un peu");
        if (stream.isWriting)
        {
            //Debug.Log("ENOVYE");
            // We own this player: send the others our data
            //stream.SendNext(basic_1);
            //stream.SendNext(basic_2);
            //stream.SendNext(basic_3);
            //stream.SendNext(special_1);
            //stream.SendNext(special_2);
            //stream.SendNext(special_3);
            //stream.SendNext(isDead);
            //stream.SendNext(isRight);
            bool isPressingUp = false;
            bool isPressingLeft = false;
            bool isPressingDown = false;
            bool isPressingRight = false;
            bool isPressingJump = false;
            bool isPressingA = false;
            bool isPressingB = false;

            if (Input.GetKeyDown(jump))
            {
                isPressingJump = true;
            }
            if (Input.GetKeyDown(up))
            {
                isPressingUp = true;
            }
            if (Input.GetKeyDown(left))
            {
                isPressingLeft = true;
            }
            if (Input.GetKeyDown(down))
            {
                isPressingDown = true;
            }
            if (Input.GetKeyDown(right))
            {
                isPressingRight = true;
            }
            if (Input.GetKeyDown(A))
            {
                isPressingA = true;
            }
            if (Input.GetKeyDown(B))
            {
                isPressingB = true;
            }

            stream.SendNext(isPressingUp);
            stream.SendNext(isPressingLeft);
            stream.SendNext(isPressingDown);
            stream.SendNext(isPressingRight);
            stream.SendNext(isPressingJump);
            stream.SendNext(isPressingA);
            stream.SendNext(isPressingB);

        }
        else
        {
            //Debug.Log("RECOIT");

            // Network player, receive data
            bool isPressingUp = (bool)stream.ReceiveNext();
            bool isPressingLeft = (bool)stream.ReceiveNext();
            bool isPressingDown = (bool)stream.ReceiveNext();
            bool isPressingRight = (bool)stream.ReceiveNext();
            bool isPressingJump = (bool)stream.ReceiveNext();
            bool isPressingA = (bool)stream.ReceiveNext();
            bool isPressingB = (bool)stream.ReceiveNext();

            if (isPressingJump)
            {
                MoveUp();
            }
            if (isPressingDown)
            {
                MoveDown();
            }


            //////////////////////////////////ATTACKS

            //A

            if (isPressingA && isPressingUp) // A + ↑
            {
                Basic2();
            }
            else if (isPressingA && isPressingDown) // A + ↓
            {
                basic_2 = false;
                Basic3();
            }
            else if (isPressingA) // A + ← →
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
            if (isPressingB && isPressingUp) // B + ↑
            {
                Special2();
            }
            else if (isPressingB && isPressingDown) // B + ↓
            {
                special_2 = false;
                Special3();
            }
            else if (isPressingB) // B + ← →
            {
                special_3 = false;
                Special1(true, chargePercentage);
            }
            else
            {
                Special1(false, chargePercentage);
                special_1 = false;
            }


            if (isPressingDown) // B + ↓
            {
                pressDown = true;
            }
            else
            {
                pressDown = false;
            }

            if (isPressingUp) // B + ↑
            {
                pressUp = true;
            }
            else
            {
                pressUp = false;
            }

            //Gauche/Droite
            if (isPressingLeft && rb2d.velocity.x > -maxSpeed) { MoveLeft(); }
            else if (isPressingRight && rb2d.velocity.x < maxSpeed) { MoveRight(); }
            else { x = 0; }//if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) { x = 0; }


        }

    }
}
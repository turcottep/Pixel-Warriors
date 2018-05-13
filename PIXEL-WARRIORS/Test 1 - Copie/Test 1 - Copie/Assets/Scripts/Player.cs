using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Player : Photon.PunBehaviour, IPunObservable
{
    #region public variables
    public int playerType;
    public int mapNumber;

    public float maxSpeed = 2.5f;
    public float speed = 8f;
    public float jumpPower = 175f;
    public float maxJump = 2f;
    public float percentage = 0f;
    public float multiplier = 1f;

    //Charge
    public static float chargeTime = 0;
    private float chargePercentage = 0;
    public static bool isCharging = false;
    public static bool fullyCharged = false;
    private bool canShoot = true;

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

    //Multiplayer
    public bool isPressingUp = false;
    public bool isPressingLeft = false;
    public bool isPressingDown = false;
    public bool isPressingRight = false;
    public bool isPressingJump = false;
    public bool isPressingA = false;
    public bool isPressingB = false;

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

    //audio
    [HideInInspector]
    public bool muteSound;
    [HideInInspector]
    public bool music;

    //misc variables
    public bool aiON = true;
    public int x = 0;
    public bool isRight;
    public bool isDead;
    public int playerNum;
    public Manager gameManager;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    #endregion

    #region private variables

    //Audio
    private AudioManager audioManager;

    private float stun = 0f;

    public Rigidbody2D rb2d;
    private Animator anim;
    private Player player;

    private Vector2 pos;
    private Vector2 knockback;


    #endregion

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Manager>();

        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        muteSound = audioManager.soundOn;

        mapNumber = gameManager.mapN;
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
        gameManager.UpdatePercentages(playerNum);

        PhotonNetwork.sendRate = 100;
        PhotonNetwork.sendRateOnSerialize = 100;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Color damageColor = Color.red;

        //Hit by attacks
        if ((player.tag == "Player 1" && col.gameObject.tag == "AttPlayer2" && !player.isDead) || (player.tag == "Player 2" && col.gameObject.tag == "AttPlayer1" && !player.isDead))
        {

            //TEST
            Vector2 vecteurTest = new Vector2(0, 0);
            if (!(col.gameObject.name == "Scientist_Poison(Clone)") && !(col.gameObject.name == "Ninja_Bomb(Clone)"))
            {
                if (col.gameObject.name == "Ninja_Explosion(Clone)")
                {
                    vecteurTest = player.transform.position - col.transform.position;
                    vecteurTest = new Vector2(vecteurTest.x, 0.1f);
                    vecteurTest = vecteurTest * multiplier;
                }
                else
                {
                    vecteurTest = rb2d.position - col.rigidbody.position;
                    vecteurTest = new Vector2(vecteurTest.x, (0.1f));
                    vecteurTest = vecteurTest * multiplier;
                }
            }
            //Debug.Log("vecteur test x :" + vecteurTest.x);
            if (Mathf.Abs(vecteurTest.x) < 0.1)
            {
                if (vecteurTest.x < 0) vecteurTest.x = -0.1f;
                else vecteurTest.x = 0.1f;
            }

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
            else if (!(col.gameObject.name == "Demon_Small_Col(Clone)") && !(col.gameObject.name == "Demon_Big_Col(Clone)"))
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

            audioManager.Play("Hit", 0, muteSound);
        }

        //Lava
        if (col.gameObject.tag == "Lava")
        {

            charge = false;
            this.rb2d.velocity = new Vector2(rb2d.velocity.x, percentage / 4 + 6);
            this.maxJump = 2;
            //Debug.Log("bounce =" + percentage);
            if (percentage == 0) percentage = 1;
            else percentage = 1.5f * percentage;
            gameManager.UpdatePercentages(playerNum);
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine("Whitecolor");

            audioManager.Play("Lava", 0, muteSound);

        }

        //if (col.gameObject.tag == "Bounce")
        //{
        //    Debug.Log("x=:" + rb2d.velocity.x + " y=" + rb2d.velocity.y);
        //    this.rb2d.velocity = this.rb2d.velocity * -1;
        //    Debug.Log("x=:" + rb2d.velocity.x + " y=" + rb2d.velocity.y);
        //    StartCoroutine("Whitecolor");
        //}
    }

    IEnumerator CooldownSpecial1()
    {
        canShoot = false;
        yield return new WaitForSeconds(2);
        canShoot = true;
    }

    void Update()
    {
        if (gameManager.isStarted)
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

            #region singleplayer input
            if (!player.stunned && gameManager.getGameMode() == 1)
            {
                //if in singleplayer, manage inputs normally
                if (Input.GetKeyDown(jump))
                {
                    MoveUp();
                }
                if (Input.GetKeyDown(down))
                {
                    //MoveDown();
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

                if (isCharging)
                {
                    isCharging = true;
                    chargeTime += Time.deltaTime;
                    if (chargeTime > 2.7f) { fullyCharged = true; }
                    else { fullyCharged = false; }
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
                else if (((Input.GetKeyDown(B) || isButtonAttackBPointerDown) && canShoot)) // B + ← →
                {
                    isCharging = true;

                    special_3 = false;
                    Special1(true);
                }
                else if (Input.GetKeyUp(B)) // B + ← →
                {
                    Special1(false);
                    special_1 = false;

                    StartCoroutine("CooldownSpecial1");

                    Charge.color = "green";
                    chargePercentage = 0;
                    chargeTime = 0;
                    isCharging = false;
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
            #endregion
            #region multiplayer input
            else if (!player.stunned && (photonView.isMine && PhotonNetwork.connected == true))
            {
                //manage inputs
                isPressingUp = false;
                isPressingLeft = false;
                isPressingDown = false;
                isPressingRight = false;
                isPressingJump = false;
                isPressingA = false;
                isPressingB = false;

                if (Input.GetKeyDown(jump))
                {
                    isPressingJump = true;
                }
                if (Input.GetKeyDown(up))
                {
                    isPressingUp = true;
                }

                if (Input.GetKeyDown(down))
                {
                    isPressingDown = true;
                }


                if (Input.GetKeyDown(A))
                {
                    isPressingA = true;
                }
                if (Input.GetKeyDown(B))
                {
                    isPressingB = true;
                }

                //Gauche/Droite
                if ((Input.GetKey(left) || isButtonLeftPointerDown) && rb2d.velocity.x > -maxSpeed) { isPressingLeft = true; isPressingRight = false; }
                else if ((Input.GetKey(right) || isButtonRightPointerDown) && rb2d.velocity.x < maxSpeed) { isPressingRight = true; isPressingLeft = false; }
                else { isPressingLeft = false; isPressingRight = false; }//if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) { x = 0; }
                #endregion
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.isStarted)
        {


            if (aiON) player.GetComponent<AI>().AIUpdate();


            //float h = Input.GetAxisRaw("Horizontal");
            float decay = 0.8f;

            pos = transform.position;

            //Out of map
            if (((mapNumber == 1) && (rb2d.transform.position.y < -2.4f || rb2d.transform.position.y > 3.2f || rb2d.transform.position.x > 4.6f || rb2d.transform.position.x < -4.6f)) || ((mapNumber == 2) && (rb2d.transform.position.y < -2.5f || rb2d.transform.position.y > 3.4f || rb2d.transform.position.x > 4.8f || rb2d.transform.position.x < -4.8f)))
            {

                player.isDead = true;
                player.GetComponent<Collider2D>().enabled = false;
                gameManager.PlayerDeath(playerNum);

                audioManager.Play("Death", 0, muteSound);
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
    }


    //MOVES

    #region action functions
    public void MoveUp()
    {
        if (this.isDead) this.Revive();
        if (maxJump > 0)
        {

            maxSpeed = 2f; //ENLEVER?
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(new Vector2(0, jumpPower));
            maxJump--;

            audioManager.Play("Jump", 0, muteSound);

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
    #endregion 

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
        gameManager.UpdatePercentages(playerNum);
    }
    IEnumerator Stun(float stunDuration)
    {
        yield return new WaitForSeconds(stunDuration / 30);
        player.stunned = false;
        player.gameObject.layer = 7 + playerNum;
        //Debug.Log("unstunned");
    }

    public void Reset()
    {
        player.dead = true;
        player.special_1 = false;
        player.special_2 = false;
        player.special_3 = false;
        player.stunned = false;
        percentage = 0;
        gameManager.UpdatePercentages(playerNum);
        rb2d.velocity = new Vector2(0, 0);
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        //if (Input.GetKey(jump) || Input.GetKey(down))
        //{
        //    //Debug.Log("revit");

        //}
    }

    public void Revive()
    {
        rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Collider2D>().enabled = true;
        player.isDead = false;
        player.dead = false;
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


    //Send and receive packets
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("marche un peu");
        if (stream.isWriting)
        {
            //Debug.Log("ENOVYE");
            // We own this player: send the others our data

            stream.SendNext(isPressingUp);
            stream.SendNext(isPressingLeft);
            stream.SendNext(isPressingDown);
            stream.SendNext(isPressingRight);
            stream.SendNext(isPressingJump);
            stream.SendNext(isPressingA);
            stream.SendNext(isPressingB);
            stream.SendNext(isDead);
            stream.SendNext(percentage);

        }
        else
        {
            //Debug.Log("RECOIT");

            // Network player, receive data
            isPressingUp = (bool)stream.ReceiveNext();
            isPressingLeft = (bool)stream.ReceiveNext();
            isPressingDown = (bool)stream.ReceiveNext();
            isPressingRight = (bool)stream.ReceiveNext();
            isPressingJump = (bool)stream.ReceiveNext();
            isPressingA = (bool)stream.ReceiveNext();
            isPressingB = (bool)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();
            percentage = (float)stream.ReceiveNext();
        }

        #region control player
        // control player
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
            Special1(true);
        }
        else
        {
            Special1(false);
            special_1 = false;
        }


        if (isPressingLeft)
        {
            MoveLeft();
        }
        else if (isPressingRight)
        {
            MoveRight();
        }
        if (isPressingLeft && rb2d.velocity.x > -maxSpeed) { MoveLeft(); }
        else if (isPressingRight && rb2d.velocity.x < maxSpeed) { MoveRight(); }
        else { x = 0; }//if (Input.GetKeyUp(left) || Input.GetKeyUp(right)) { x = 0; }

        #endregion
    }

    public bool getSound()
    {
        return muteSound;
    }
    #region misc functions
    IEnumerator Poison(int time)
    {
        yield return new WaitForSeconds(time);
        this.GetComponent<SpriteRenderer>().color = Color.green;
        StartCoroutine("Whitecolor");
        this.ReceiveDamage(0, 0.25f, false, new Vector2(0, 0));
    }

    IEnumerator Whitecolor()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (rb2d.velocity.y <= 0 && col.gameObject.tag != "Lava" && col.gameObject.tag != "AttPlayer2" && col.gameObject.tag != "AttPlayer1" && col.gameObject.tag != "ShieldPlayer1" && col.gameObject.tag != "ShieldPlayer2")
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

    public float getCharge()
    {
        return chargeTime;
    }

    #endregion
}
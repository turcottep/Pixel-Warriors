using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	public bool isButtonLeftPointerDown;
	public bool isButtonRightPointerDown;
    public Text textPercentage;
    public Button Button_right;

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
		knockback.Set(-2, 0);

        setPercentageText();
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
        if (rb2d.velocity.y<=0)
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
        attack_1     = true;
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
		player.transform.position = initialPosition;
		rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
		if (Input.GetKey(up) || Input.GetKey(down))
		{
			rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
			player.isDead = false;
			player.dead = false;
		}
	}




	//Turcotte AI



	//Boutons Will
	public void buttonJumpPointerDown()
	{
		rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		player.isDead = false;
		player.dead = false;

		this.MoveUp();


	}

	public void buttonAttackAPointerDown()
	{
		this.Attack1();
	}

	public void buttonAttackBPointerDown()
	{
		this.Attack2(true);
	}

	public void buttonAttackBPointerUp()
	{
		Attack2(false);
	}

	public void buttonLeftPointerDown()
	{
		this.MoveLeft();
		isButtonLeftPointerDown = true;
	}

	public void buttonLeftPointerUp()
	{
		this.MoveLeft();
		isButtonLeftPointerDown = false;
	}

	public void buttonRightPointerDown()
	{
		this.MoveRight();
		isButtonRightPointerDown = true;
	}

	public void buttonRightPointerUp()
	{
		isButtonRightPointerDown = false;
	}

	public void buttonDownPointerDown()
	{
		this.MoveDown();
	}

	public void buttonUpPointerDown()
	{
		this.MoveUp();
	}

    public void setPercentageText()
    {
        textPercentage.text = "% : " + percentage.ToString();
    }

}

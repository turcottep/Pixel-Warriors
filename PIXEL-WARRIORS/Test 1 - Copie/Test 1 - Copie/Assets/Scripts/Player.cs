﻿using System.Collections;
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

	//controls
	public KeyCode up = KeyCode.W;
	public KeyCode left = KeyCode.A;
	public KeyCode down = KeyCode.S;
	public KeyCode right = KeyCode.D;
	public KeyCode attack1 = KeyCode.R;
	public KeyCode attack2 = KeyCode.F;
	public KeyCode attack3 = KeyCode.C;

    public bool isButtonJumpPointerDown;
    public bool isButtonAttackAPointerDown;
    public bool isButtonAttackBPointerDown;
    public bool isButtonAttackCPointerDown;
    public bool isButtonLeftPointerDown;
    public bool isButtonRightPointerDown;
    public bool isButtonUpPointerDown;
    public bool isButtonDownPointerDown;

    public Vector3 initialPosition = new Vector3(-2, 1.6f, 0);

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
		//Hit by an ennemy projectile
		if ((player.tag == "Player 1" && col.gameObject.tag == "Ball2") || (player.tag == "Player 2" && col.gameObject.tag == "Ball1"))
		{
			Destroy(col.gameObject);
			rb2d.AddForce(col.rigidbody.velocity * percentage, ForceMode2D.Impulse);
			percentage += 0.75f;
			stun = 10 + (.5f * percentage);
			Debug.Log("pourcentage P1: " + percentage);
		}

		//Hit by melee
		if (col.gameObject.tag == "Melee2")
		{
			player.transform.position = pos;
			Destroy(col.gameObject);
			int dir = 0;
			if (isRight) dir = 1;
			else dir = -1;
			rb2d.AddForce(knockback*dir*percentage*0.2f, ForceMode2D.Impulse);
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
		if ((Input.GetKeyDown(up) || isButtonJumpPointerDown) && maxJump > 0)
		{
			maxSpeed = 2f;

			Vector2 temp = rb2d.velocity;
			temp.y = 0;
			rb2d.velocity = new Vector2(temp.x, temp.y);
			rb2d.AddForce(new Vector2(0, jumpPower));
			maxJump--;
		}

		//Attack 1
		if (Input.GetKeyDown(attack1) || isButtonAttackAPointerDown)
		{
			attack_1 = true;
		}
		else { attack_1 = false; }

		//Attack 2
		if (Input.GetKey(attack2) || isButtonAttackBPointerDown) { charge = true; }
		else { charge = false; }

		//Gauche/Droite
		if ((Input.GetKey(left) || isButtonLeftPointerDown) && rb2d.velocity.x > -maxSpeed) { x = -1; isRight = false; }
		else if ((Input.GetKey(right) || isButtonRightPointerDown) && rb2d.velocity.x < maxSpeed) { x = 1; isRight = true; }
		else { x = 0; }

		//test
		if (Input.GetKey(down) || isButtonDownPointerDown)
		{
			Debug.Log("touchevue");
		}
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
				player.transform.position = initialPosition;
				rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
				if (Input.GetKey(up) || Input.GetKey(down) || isButtonUpPointerDown || isButtonDownPointerDown)
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

    public void buttonAttackAPointerDown()
    {
        isButtonAttackAPointerDown = true;
    }

    public void buttonAttackAPointerUp()
    {
        isButtonAttackAPointerDown = false;
    }

    public void buttonAttackBPointerDown()
    {
        isButtonAttackBPointerDown = true;
    }

    public void buttonAttackBPointerUp()
    {
        isButtonAttackBPointerDown = false;
    }

    public void buttonAttackCPointerDown()
    {
        isButtonAttackCPointerDown = true;
    }

    public void buttonAttackCPointerUp()
    {
        isButtonAttackCPointerDown = false;
    }

    public void buttonLeftPointerDown()
    {
        isButtonLeftPointerDown = true;
    }

    public void buttonLeftPointerUp()
    {
        isButtonLeftPointerDown = false;
    }

    public void buttonRightPointerDown()
    {
        isButtonRightPointerDown = true;
    }

    public void buttonRightPointerUp()
    {
        isButtonRightPointerDown = false;
    }

    public void buttonDownPointerDown()
    {
        isButtonDownPointerDown = true;
    }

    public void buttonDownPointerUp()
    {
        isButtonDownPointerDown = false;
    }

    public void buttonUpPointerDown()
    {
        isButtonUpPointerDown = true;
    }

    public void buttonUpPointerUp()
    {
        isButtonUpPointerDown = false;
    }

    public void buttonJumpPointerDown()
    {
        isButtonJumpPointerDown = true;
    }

    public void buttonJumpPointerUp()
    {
        isButtonJumpPointerDown = false;
    }
}

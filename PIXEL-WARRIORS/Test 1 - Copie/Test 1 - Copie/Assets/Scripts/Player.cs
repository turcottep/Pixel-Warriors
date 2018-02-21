using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public float maxSpeed = 2.5f;
	public float speed = 8f;
	public float jumpPower = 175f;
	public float maxJump = 2f;
	public float percentage = 0f;
	public int lives = 3;

<<<<<<< HEAD
	public bool grounded;
	public bool attack_1;
	public bool charge;
	public bool goingDown;

	private int x = 0;
	private bool isRight;
=======
    public bool grounded;
    public bool attack_1;
    public bool charge;
    public bool goingDown;
    public bool dead;

	private int x = 0;
    private bool isRight;
    private bool isDead;
>>>>>>> be1d10326b80e7e294af1da508033abbf1dec029

	private Rigidbody2D rb2d;
	private Animator anim;
	private Player player;

	private Collider2D platform;
	private SpriteRenderer ordre;

	void Start()
	{

		rb2d = gameObject.GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator>();
		player = gameObject.GetComponentInParent<Player>();

		platform = gameObject.GetComponent<Collider2D>();

		ordre = GetComponent<SpriteRenderer>();

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ball2")
		{
			Destroy(col.gameObject);
			percentage += 25;
		}
		if (col.gameObject.tag == "Out")
		{
			lives--;

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

<<<<<<< HEAD
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
=======
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
>>>>>>> be1d10326b80e7e294af1da508033abbf1dec029
			maxSpeed = 2f;

			Vector2 temp = rb2d.velocity;
			temp.y = 0;
			rb2d.velocity = new Vector2(temp.x, temp.y);
			rb2d.AddForce(new Vector2(0, jumpPower));
			maxJump--;
		}

		if (Input.GetKeyDown(KeyCode.R)) { attack_1 = true; }
		else { attack_1 = false; }

		if (Input.GetKey(KeyCode.F)) { charge = true; }
		else { charge = false; }

		if (Input.GetKey(KeyCode.A)) { x = -1; isRight = false; }
		else if (Input.GetKey(KeyCode.D)) { x = 1; isRight = true; }
		else { x = 0; }

<<<<<<< HEAD
		if (Input.GetKeyDown(KeyCode.S) && player.transform.position.y > 1.1)
		{

			player.GetComponent<Collider2D>().isTrigger = true;
			StopCoroutine("Wait");
			StartCoroutine("Wait");

		}

	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(.2f);
		print("waiting...");
		player.GetComponent<Collider2D>().isTrigger = false;
	}

	private void FixedUpdate()
	{
		if (x != 0 )
		{
			ordre.sortingOrder = 1;
		}
		else
		{
			ordre.sortingOrder = 0;
		}

		float h = Input.GetAxisRaw("Horizontal");
=======
        if (Input.GetKeyDown(KeyCode.S) && player.transform.position.y > 1.1)
        {
           
            player.GetComponent<Collider2D>().isTrigger = true;
            StopCoroutine("Wait");
            StartCoroutine("Wait");
      
        }

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.2f);
        player.GetComponent<Collider2D>().isTrigger = false;
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
>>>>>>> be1d10326b80e7e294af1da508033abbf1dec029
		float decay = 0.8f;

        //Out of map
		if (rb2d.transform.position.y < -1 || rb2d.transform.position.y > 3.2 || rb2d.transform.position.x > 2.4 || rb2d.transform.position.x < -3.6)
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
		

		if (rb2d.velocity.y < 0) { player.goingDown = true; }
		else { player.goingDown = false; }

		//Move player
		rb2d.AddForce(Vector2.right * x * speed, ForceMode2D.Impulse);
		rb2d.velocity = new Vector2(rb2d.velocity.x * decay, rb2d.velocity.y);

		//Limit speed
		if (rb2d.velocity.x > maxSpeed)
		{
			rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
		}

		if (rb2d.velocity.x < -maxSpeed)
		{
			rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Transform autre = GameObject.FindGameObjectWithTag("Player").transform;
			Physics2D.IgnoreCollision(autre.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}

	public const string LAYER_NAME = "TopLayer";
	public int sortingOrder = 0;
	private SpriteRenderer sprite;

	
		


}

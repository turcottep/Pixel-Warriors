using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    public float maxSpeed = 2.5f;
    public float speed = 8f;
    public float jumpPower = 175f;
    public float maxJump = 2f;
    public float percentage = 0f;
    public int lives = 3;

    public bool grounded;
    public bool attack_1;
    public bool charge;
    public bool goingDown;

    private int x = 0;
    private bool isRight;

    private Rigidbody2D rb2d;
    private Animator anim;
    private Player2 player;

    private Collider2D platform;

    void Start()
    {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player2>();

        platform = gameObject.GetComponent<Collider2D>();

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ball1")
        {
            Destroy(col.gameObject);
            percentage += 25;
        }
        if (col.gameObject.tag == "Out")
        {
            lives--;

            //y = -1
            //y = 2.4
            //x = -3.6
            //x = 1.9


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
        maxSpeed = 1.5f;
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

        //Flip character L/R
        if (isRight == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (isRight == true)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        /*
        //Double jump
        if (Input.GetKeyDown(KeyCode.I) && maxJump > 0)
        {
            maxSpeed = 2f;

            Vector2 temp = rb2d.velocity;
            temp.y = 0;
            rb2d.velocity = new Vector2(temp.x, temp.y);
            rb2d.AddForce(new Vector2(0, jumpPower));
            maxJump--;
        }

        if (Input.GetKeyDown(KeyCode.U)) { attack_1 = true; }
        else { attack_1 = false; }

        if (Input.GetKey(KeyCode.H)) { charge = true; }
        else { charge = false; }

        if (Input.GetKey(KeyCode.J)) { x = -1; isRight = false; }
        else if (Input.GetKey(KeyCode.L)) { x = 1; isRight = true; }
        else { x = 0; }
        */

        if (Input.GetKeyDown(KeyCode.K) && player.transform.position.y > 1.1)
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

        List<Transform> trous = new List<Transform>();

        Transform autre = GameObject.FindGameObjectWithTag("Player1").transform;
        float distance = autre.position.x - this.transform.position.x;
        isRight = distance > 0;

        trous.Add(GameObject.FindGameObjectWithTag("Hole").transform);
        trous.Add(GameObject.FindGameObjectWithTag("Hole2").transform);
        List<float> distanceTrousX = new List<float>();
        List<float> distanceTrousY = new List<float>();

        foreach (Transform t in trous)
        {
            distanceTrousX.Add(t.position.x - this.transform.position.x);
            distanceTrousY.Add(t.position.y - this.transform.position.y);

        }


        //Debug.Log("x " + x);
        //Debug.Log("distanceTrou " + distanceTrou);
        //Debug.Log("test " + (distanceTrou * x)); 
        bool saute = false;
        foreach (float d in distanceTrousX)
        {
            saute = (d * x > 0 && d * x < 0.7);
            if (saute) break;

        }
        if (saute && maxJump > 0)
        {

            maxSpeed = 2f;

            Vector2 temp = rb2d.velocity;
            temp.y = 0;
            rb2d.velocity = new Vector2(temp.x, temp.y);
            rb2d.AddForce(new Vector2(0, jumpPower));
            maxJump--;

            //Debug.Log("saut");

        }

        bool avance = false;
        int trou = 0;
        for (int i = 0; i < distanceTrousX.Count; i++)
        {
            avance = Mathf.Abs(distanceTrousX[i]) < 0.8;
            if (avance)
            {
                trou = i;
                break;
            }
        }
        if (avance)
        {
            //Over Edge
            //Debug.Log("y vel = " + rb2d.velocity.y );

            //si il est plus bas que le trou
            if (rb2d.velocity.y < -4.5)
            {
                Debug.Log("saute encore");

                //saute encore
                maxSpeed = 2f;

                Vector2 temp = rb2d.velocity;
                temp.y = 0;
                rb2d.velocity = new Vector2(temp.x, temp.y);
                rb2d.AddForce(new Vector2(0, jumpPower));
                maxJump--;
            }

            if (x == 0)
            {
                if (isRight) { x = 1; }
                else { x = -1; }
            }
        }
        else if (Mathf.Abs(distance) > 0.5)
        {
            if (isRight) { x = 1; }
            else { x = -1; }

            //Debug.Log("distance élevée");
        }
        else if (Mathf.Abs(distance) > 0.1)
        {
            x = 0;

            //Debug.Log("distance faible: " + distance);
        }


        float h = Input.GetAxisRaw("Horizontal");
        float decay = 0.8f;

        if (rb2d.transform.position.y < -1 || rb2d.transform.position.y > 2.6 || rb2d.transform.position.x > 2.4 || rb2d.transform.position.x < -3.6)
        {
            lives--;
            player.transform.position = new Vector3(0.3f, 1.6f, 0);
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


}

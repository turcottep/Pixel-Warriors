using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_p2 : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public float cooldown = 1f;

    private Animator anim;
    private Player2 player;
    private Rigidbody2D rb2d;

    private bool shootCharge;
    private float direction = 0f;
    private float lifeTime = 1.5f;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player2>();
    }

    void Update()
    {
        anim.SetBool("ShootCharge", shootCharge);

        if (Input.GetKeyDown(KeyCode.Keypad3) && canShoot && player.dead == false)
        {
            shootCharge = true;
        }
        if (Input.GetKeyUp(KeyCode.Keypad3) && canShoot && player.dead == false)
        {
            shootCharge = false;
            GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.Euler(0, direction, 0));
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
            GetComponent<Animator>().SetTrigger("Shoot");
            Destroy(go, lifeTime);
            StartCoroutine("CanShoot");
        }

    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = 180f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = 0f;
        }
    }
}

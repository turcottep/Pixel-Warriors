using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public float cooldown = 1f;

    private Animator anim;
    private Player player;
    private Rigidbody2D rb2d;

    private bool shootCharge;

    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();
    }
	
	void Update () {
        anim.SetBool("ShootCharge", shootCharge);

        if (Input.GetKeyDown(KeyCode.C) && canShoot)
        {
            /*if (player.grounded == false) { rb2d.constraints = RigidbodyConstraints2D.FreezePositionX; }
            else { rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; }*/           
            shootCharge = true;
        }
        if (Input.GetKeyUp(KeyCode.C) && canShoot)
        {
            //rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            shootCharge = false;
         
            GameObject go = (GameObject) Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);

            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);

            GetComponent<Animator>().SetTrigger("Shoot");

            StartCoroutine("CanShoot");
        }

	}

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}

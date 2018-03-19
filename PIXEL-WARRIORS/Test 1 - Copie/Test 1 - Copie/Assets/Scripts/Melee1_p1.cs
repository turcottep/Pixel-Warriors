using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee1_p1 : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true;
    public Vector2 offset = new Vector2(0.061873f, 0f);
    public float cooldown = 0.3f;

    private Animator anim;
    private Player player;
    private Rigidbody2D rb2d;

    private bool attacking;
    public float lifeTime = 0.3f;

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = gameObject.GetComponentInParent<Player>();
    }

    void Update()
    {

        if (Input.GetKeyDown(player.attack1) && canShoot && !player.dead)
        {
            GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.Euler(0, 0, 0));
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), go.GetComponent<Collider2D>());
			//velocity = new Vector2(velocity.x + rb2d.velocity.x, velocity.y + rb2d.velocity.y);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
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
}

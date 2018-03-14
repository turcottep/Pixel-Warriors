using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poison : MonoBehaviour {

    public GameObject projectile;
    public Vector2 velocity;
    bool canShoot = true;

    private Vector2 offset = new Vector2(0.2f, -0.2f);

    public float cooldown = 2f;
    public float lifetime = 3f;

    private Player player;
    private Rigidbody2D rb2d;
    private Animation anim;

    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    void Update()
    {

        /*if (Input.GetKeyDown(player.attack4) && canShoot && !player.dead)
        {

        }*/

        if (Input.GetKeyUp(player.attack4) && canShoot && !player.dead)
        {
            GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.Euler(0, 0, 0));
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);

            Destroy(go, lifetime);
            StartCoroutine("CanShoot");
        }

    }


    private void FixedUpdate()
    {
        if (Input.GetKey(player.left))
        {
            offset.y = -0.2f;
        }
        else if (Input.GetKey(player.right))
        {
            offset.y = 0.2f;
        }
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}

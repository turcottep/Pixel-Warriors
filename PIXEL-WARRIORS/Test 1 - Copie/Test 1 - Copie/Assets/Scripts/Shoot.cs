using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

	public GameObject projectile;
	public Vector2 velocity;
	public Vector2 offset = new Vector2(0.4f, 0.1f);
	public float cooldown = 1f;

	private Animator anim;
	private Player player;
	private Rigidbody2D rb2d;

	private float direction = 0f;
	private float lifeTime = 1.5f;

	void Start()
	{
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator>();
		player = gameObject.GetComponentInParent<Player>();
	}

	private void FixedUpdate()
	{
		if (Input.GetKey(player.left))
		{
			direction = 0f;
		}
		else if (Input.GetKey(player.right))
		{
			direction = 180f;
		}
	}

	IEnumerator CanShoot()
	{
		player.canShoot = false;
		yield return new WaitForSeconds(cooldown);
        player.canShoot = true;
	}

	public void shoot()
	{
		if (player.canShoot && !player.dead)
		{
			anim.SetBool("ShootCharge", false);
			GameObject go = (GameObject)Instantiate(projectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.Euler(0, direction, 0));
			go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
			GetComponent<Animator>().SetTrigger("Shoot");
			Destroy(go, lifeTime);
			StartCoroutine("CanShoot");
		}

	}

	public void animate()
	{
		anim.SetBool("ShootCharge", true);
	}
}

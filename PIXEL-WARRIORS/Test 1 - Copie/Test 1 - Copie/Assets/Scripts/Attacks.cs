using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{

	public GameObject basic1;
	public GameObject basic2;
	public GameObject basic3;
	public GameObject special1;
	public GameObject special2;
	public GameObject special3;

	public Vector2 velocity_basic1;
	public Vector2 velocity_basic2;
	public Vector2 velocity_basic3;
	public Vector2 velocity_special1;
	public Vector2 velocity_special2;
	public Vector2 velocity_special3;

	public Vector2 offset_basic1 = new Vector2(0.095f, 0.01729776f);
	public Vector2 offset_basic2 = new Vector2(0f, 0f);
	public Vector2 offset_basic3 = new Vector2(0f, 0f);
	public Vector2 offset_special1 = new Vector2(0f, 0f);
	public Vector2 offset_special2 = new Vector2(0f, 0f);
	public Vector2 offset_special3 = new Vector2(0f, 0f);

	public float cooldown_special1 = 1f;
	public float cooldown_special2 = 2f;
	public float cooldown_special3 = 3f;

	private float direction = 0f;
	private float rotation = 0f;

	private float lifeTime_special1 = 2f;
	private float lifeTime_special2 = 2f;
	private float lifeTime_special3 = 2f;

	bool canShoot_basic = true;
	bool canShootSp1 = true;
	bool canShootSp2 = true;
	bool canShootSp3 = true;

	public float cooldown = 0.3f;
	private float damage;

	private Player player;
	private Rigidbody2D rb2d;
	private Animator anim;

	private bool attacking;
	public float lifeTime = 0.3f;

	void Start()
	{
		rb2d = gameObject.GetComponent<Rigidbody2D>();
		player = gameObject.GetComponentInParent<Player>();
		anim = gameObject.GetComponent<Animator>();
	}


	IEnumerator CanShootBasic()
	{
		canShoot_basic = false;
		yield return new WaitForSeconds(cooldown);
		canShoot_basic = true;
	}
	IEnumerator CanShootSpecial1()
	{
		canShootSp1 = false;
		yield return new WaitForSeconds(cooldown_special1);
		canShootSp1 = true;
	}
	IEnumerator CanShootSpecial2()
	{
		canShootSp2 = false;
		yield return new WaitForSeconds(cooldown_special2);
		canShootSp2 = true;
	}
	IEnumerator CanShootSpecial3()
	{
		canShootSp3 = false;
		yield return new WaitForSeconds(cooldown_special3);
		canShootSp3 = true;
	}

	public void LaunchBasic1(int playerNum) //A
	{
		//Basic1
		if (canShoot_basic && !player.dead)
		{
			damage = 0.25f;
			GameObject go = (GameObject)Instantiate(basic1, (Vector2)transform.position + offset_basic1 * transform.localScale.x, Quaternion.Euler(0, 0, 0));
			if (playerNum == 1)
			{
				go.tag = "AttPlayer1";
				go.layer = 11;
			}
			else if (playerNum == 2)
			{
				go.tag = "AttPlayer2";
				go.layer = 12;
			}
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), go.GetComponent<Collider2D>());
			go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity_basic1.x * transform.localScale.x, velocity_basic1.y);
			Destroy(go, lifeTime);
			StartCoroutine("CanShootBasic");

		}
	}

	public void LaunchBasic2(int playerNum) //A + ↑
	{
		//Basic2
		if (canShoot_basic && !player.dead)
		{
			damage = 0.5f;
			GameObject go = (GameObject)Instantiate(basic2, (Vector2)transform.position + offset_basic2 * transform.localScale.x, Quaternion.Euler(0, 0, 0));
			if (playerNum == 1)
			{
				go.tag = "AttPlayer1";
				go.layer = 11;
			}
			else if (playerNum == 2)
			{
				go.tag = "AttPlayer2";
				go.layer = 12;
			}
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), go.GetComponent<Collider2D>());
			go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity_basic2.x * transform.localScale.x, velocity_basic2.y);
			Destroy(go, lifeTime);
			StartCoroutine("CanShootBasic");
		}
	}

	public void LaunchBasic3(int playerNum) //A + ↓
	{
		//Basic3
		if (canShoot_basic && !player.dead)
		{
			damage = 0.5f;
			GameObject go = (GameObject)Instantiate(basic3, (Vector2)transform.position + offset_basic3 * transform.localScale.x, Quaternion.Euler(0, 0, 0));
			if (playerNum == 1)
			{
				go.tag = "AttPlayer1";
				go.layer = 11;
			}
			else if (playerNum == 2)
			{
				go.tag = "AttPlayer2";
				go.layer = 12;
			}
			Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), go.GetComponent<Collider2D>());
			go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity_basic3.x * transform.localScale.x, velocity_basic3.y);
			Destroy(go, lifeTime);
			StartCoroutine("CanShootBasic");
		}
	}

	public void LaunchSpecial1(int playerNum, bool state) //B
	{

		//Problème d'animation (reste bloqué)
		if (canShootSp1 && !player.dead)
		{
			if (state)
			{
				player.isChargingSp1 = true;
				anim.SetBool("Charge", true);
			}
			else if (player.isChargingSp1)
			{
				player.isChargingSp1 = false;
				anim.SetBool("Charge", false);
				if (player.isRight)
				{
					direction = 0f;
				}
				else { direction = 180f; }
				damage = 0.75f;
				if (player.name == "Scientist(Clone)") { rotation = 90; }
				else { rotation = 0; }
				GameObject go = (GameObject)Instantiate(special1, (Vector2)transform.position + offset_special1 * transform.localScale.x, Quaternion.Euler(0, direction, rotation));
				if (playerNum == 1)
				{
					go.tag = "AttPlayer1";
					go.layer = 11;
				}
				else if (playerNum == 2)
				{
					go.tag = "AttPlayer2";
					go.layer = 12;
				}
				go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity_special1.x * transform.localScale.x, velocity_special1.y);
				GetComponent<Animator>().SetTrigger("Special1");
				Destroy(go, lifeTime_special1);
				StartCoroutine("CanShootSpecial1");
			}
		}

	}

	public void LaunchSpecial2(int playerNum) //B + ↑
	{

		//Ne doit pas collide avec les hitbox de sa propre attaque... Pour l'instant, mis un plateform effector qui ignore Player1 TURCOTTE: Réglé par layers
		if (player.name == "Demon(Clone)" && canShootSp2 && !player.dead && player.grounded)
		{
			damage = 1.25f;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			GameObject smallBoneLeft = Instantiate(Resources.Load("Demon_Small_Bone"), new Vector2(player.GetComponent<Rigidbody2D>().position.x - 0.58499652f, player.GetComponent<Rigidbody2D>().position.y - 0.07370224f), Quaternion.identity) as GameObject;
			GameObject smallBoneRight = Instantiate(Resources.Load("Demon_Small_Bone"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + 0.58499652f, player.GetComponent<Rigidbody2D>().position.y - 0.07370224f), Quaternion.identity) as GameObject;
			if (playerNum == 1)
			{
				smallBoneLeft.tag = "AttPlayer1";
				smallBoneLeft.layer = 11;
				smallBoneRight.tag = "AttPlayer1";
				smallBoneRight.layer = 11;
			}
			else if (playerNum == 2)
			{
				smallBoneLeft.tag = "AttPlayer2";
				smallBoneLeft.layer = 12;
				smallBoneRight.tag = "AttPlayer2";
				smallBoneRight.layer = 12;
			}
			smallBoneRight.transform.localScale = new Vector3(-1.187455f, 1.187455f, 1.187455f);

			StartCoroutine("DemonBones");

			Destroy(smallBoneLeft, lifeTime_special2);
			Destroy(smallBoneRight, lifeTime_special2);

			StartCoroutine("CanShootSpecial2");
		}

		if (player.name == "Alien(Clone)" && canShootSp2 && !player.dead)
		{
			float posBallUpBottom;
			float posBallMiddle;
			Vector2 ballSpeedUp;
			Vector2 ballSpeedMiddle;
			Vector2 ballSpeedBottom;

			if (player.isRight)
			{
				direction = 0f;
				posBallUpBottom = 0.24100546f;
				posBallMiddle = 0.33900546f;
				ballSpeedUp = new Vector2(3.5f, 0.3f);
				ballSpeedMiddle = new Vector2(3.5f, 0);
				ballSpeedBottom = new Vector2(3.5f, -0.3f);
			}
			else
			{
				direction = 180f;
				posBallUpBottom = -0.24100546f;
				posBallMiddle = -0.33900546f;
				ballSpeedUp = new Vector2(-3.5f, 0.3f);
				ballSpeedMiddle = new Vector2(-3.5f, 0);
				ballSpeedBottom = new Vector2(-3.5f, -0.3f);
			}
			damage = 1.25f;
			GameObject ballUp = Instantiate(Resources.Load("Alien_TriBall"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + posBallUpBottom, player.GetComponent<Rigidbody2D>().position.y + 0.15663729f), Quaternion.Euler(0, direction, 0)) as GameObject;
			GameObject ballMiddle = Instantiate(Resources.Load("Alien_TriBall"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + posBallMiddle, player.GetComponent<Rigidbody2D>().position.y + 0.00763729f), Quaternion.Euler(0, direction, 0)) as GameObject;
			GameObject ballBottom = Instantiate(Resources.Load("Alien_TriBall"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + posBallUpBottom, player.GetComponent<Rigidbody2D>().position.y - 0.12336271f), Quaternion.Euler(0, direction, 0)) as GameObject;
			if (playerNum == 1)
			{
				ballUp.tag = "AttPlayer1";
				ballUp.layer = 11;
				ballMiddle.tag = "AttPlayer1";
				ballMiddle.layer = 11;
				ballUp.tag = "AttPlayer1";
				ballUp.layer = 11;
			}
			else if (playerNum == 2)
			{
				ballUp.tag = "AttPlayer2";
				ballUp.layer = 12;
				ballMiddle.tag = "AttPlayer2";
				ballMiddle.layer = 12;
				ballBottom.tag = "AttPlayer2";
				ballBottom.layer = 12;
			}
			ballUp.GetComponent<Rigidbody2D>().velocity = ballSpeedUp;
			ballMiddle.GetComponent<Rigidbody2D>().velocity = ballSpeedMiddle;
			ballBottom.GetComponent<Rigidbody2D>().velocity = ballSpeedBottom;


			Destroy(ballUp, 3);
			Destroy(ballMiddle, 3);
			Destroy(ballBottom, 3);

			StartCoroutine("CanShootSpecial2");
		}

		if (player.name == "Ninja(Clone)" && canShootSp2 && !player.dead)
		{
			damage = 1.25f;
			Vector2 offset;
			Vector2 velocity;
			if (player.isRight)
			{
				direction = 0f;
				offset = new Vector2(0.2f, 0.1f);
				velocity = new Vector2(1.8f, 2.3f);
			}
			else
			{
				direction = 180f;
				offset = new Vector2(0.2f, -0.1f);
				velocity = new Vector2(2f, 2);
			}
			GameObject bomb = (GameObject)Instantiate(Resources.Load("Ninja_Bomb"), (Vector2)transform.position + offset * transform.localScale.x, Quaternion.Euler(0, direction, 0));
			if (playerNum == 1)
			{
				bomb.tag = "AttPlayer1";
				bomb.layer = 11;
			}
			else if (playerNum == 2)
			{
				bomb.tag = "AttPlayer2";
				bomb.layer = 12;
			}
			bomb.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
			StartCoroutine("BombBlast", bomb);
			StartCoroutine("CanShootSpecial2");
		}

	}
	IEnumerator DemonBones()
	{
		yield return new WaitForSeconds(0.25f);
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		GameObject bigBoneLeft = Instantiate(Resources.Load("Demon_Big_Bone"), new Vector2(player.GetComponent<Rigidbody2D>().position.x - 0.26354105f, player.GetComponent<Rigidbody2D>().position.y - 0.01315464f), Quaternion.identity) as GameObject;
		GameObject bigBoneRight = Instantiate(Resources.Load("Demon_Big_Bone"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + 0.26354105f, player.GetComponent<Rigidbody2D>().position.y - 0.01315464f), Quaternion.identity) as GameObject;
		bigBoneRight.transform.localScale = new Vector3(-1, 1, 1);
		Destroy(bigBoneLeft, lifeTime_special2);
		Destroy(bigBoneRight, lifeTime_special2);
	}
	IEnumerator BombBlast(GameObject bomb)
	{
		yield return new WaitForSeconds(1.2f);
		Vector2 pos = new Vector2(bomb.transform.position.x, bomb.transform.position.y);
		Destroy(bomb);
		GameObject blast = Instantiate(Resources.Load("Ninja_Explosion"), new Vector2(pos.x, pos.y + 0.1f), Quaternion.identity) as GameObject;

		blast.tag = bomb.tag;
		blast.layer = bomb.layer;

		StartCoroutine("BlastOff", blast);
	}
	IEnumerator BlastOff(GameObject blast)
	{
		yield return new WaitForSeconds(0.3f);
		Destroy(blast);
	}

	public void LaunchSpecial3(int playerNum) //B + ↓
	{
		//Doit se destroy ainsi que l'attaque de l'ennemei
		if (player.name == "Demon(Clone)" && canShootSp3 && !player.dead)
		{
			Vector2 offsetShield = new Vector2(0.2900102f, 0.02229776f);
			float posShield;

			if (player.isRight)
			{
				direction = 0f;
				posShield = 0.2900102f;
			}
			else
			{
				direction = 180f;
				posShield = -0.2900102f;
			}

			GameObject boneShield = Instantiate(Resources.Load("Demon_Shield"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + posShield, player.GetComponent<Rigidbody2D>().position.y + 0.02229776f), Quaternion.Euler(0, direction, 0)) as GameObject;
			if (playerNum == 1)
			{
				boneShield.tag = "AttPlayer1";
				boneShield.layer = 11;
			}
			else if (playerNum == 2)
			{
				boneShield.tag = "AttPlayer2";
				boneShield.layer = 12;
			}
			Destroy(boneShield, lifeTime_special3);
			StartCoroutine("CanShootSpecial3");
		}

		//Doit se faire destroy si hit par attaque
		if (player.name == "Alien(Clone)" && canShootSp3 && !player.dead && !player.grounded)
		{
			GameObject UFO = Instantiate(Resources.Load("Alien_UFO"), new Vector2(player.GetComponent<Rigidbody2D>().position.x + 0.019f, player.GetComponent<Rigidbody2D>().position.y - 0.307f), Quaternion.identity) as GameObject;
			if (playerNum == 1)
			{
				UFO.tag = "AttPlayer1";
				UFO.layer = 11;
			}
			else if (playerNum == 2)
			{
				UFO.tag = "AttPlayer2";
				UFO.layer = 12;
			}
			Destroy(UFO, 2);
			StartCoroutine("CanShootSpecial3");
		}

		if (player.name == "Ninja(Clone)" && canShootSp3 && !player.dead)
		{
			Vector2 pos = new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y);

			GameObject log = Instantiate(Resources.Load("Ninja_Log"), new Vector2(player.GetComponent<Rigidbody2D>().position.x, player.GetComponent<Rigidbody2D>().position.y), Quaternion.identity) as GameObject;
			player.transform.position = pos;
			rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
			StartCoroutine("CanShootSpecial3");
			StartCoroutine("LogEffect");
			player.GetComponent<Collider2D>().enabled = false;
			player.GetComponent<Renderer>().enabled = false;
			Destroy(log, 1f);
		}
	}
	IEnumerator LogEffect()
	{
		yield return new WaitForSeconds(1f);
		player.GetComponent<Collider2D>().enabled = true;
		player.GetComponent<Renderer>().enabled = true;
		rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		player.maxJump = 2;
	}

}


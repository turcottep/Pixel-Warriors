using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPoison : MonoBehaviour {

    private Rigidbody2D rb2d;
    private Vector2 pos;
    public GameObject puddle;

    private float heightBreak;
    private float heightPuddle;
    private bool hit;
    

    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    //Doit get le playerNum à partir du script attacks
    public int PlayerNum()
    {
        return 1;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "PlatformMiddleGround")
        {
            heightBreak = -0.1f; heightPuddle = 0.026f; hit = true;
        } else if (col.gameObject.name == "PlatformLeftGround1" || col.gameObject.name == "PlatformLeftGround2" || col.gameObject.name == "PlatformRightGround1" || col.gameObject.name == "PlatformRightGround2")
        {
            heightBreak = 0.625f; heightPuddle = 0.736f; hit = true;
        } else if (col.gameObject.name == "Front1" || col.gameObject.name == "Front2")
        {
            heightBreak = -0.8f; heightPuddle = -0.685f; hit = true;
        } else if (col.gameObject.name == "HitBoxLeft" || col.gameObject.name == "HitBoxRight")
        {
            heightBreak = -1.35f; heightPuddle = -1.245f; hit = true;
        } else if (col.gameObject.name == "MastLeft1" || col.gameObject.name == "MastLeft2" || col.gameObject.name == "MastRight1" || col.gameObject.name == "MastRight2")
        {
            heightBreak = 0.1117f; heightPuddle = 0.2201f; hit = true;
        }
        else { hit = false; }

        PotionEffect(heightBreak, heightPuddle, hit);
    }

    private void PotionEffect(float heightBreak, float heightPuddle, bool hit)
    {
        if (hit == true)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            pos = rb2d.transform.position;
            GameObject potion = gameObject;
            GameObject potionBreak = Instantiate(Resources.Load("Scientist_PotionBreak"), new Vector2(pos.x, heightBreak), Quaternion.identity) as GameObject;
            if (PlayerNum() == 1)
            {
                potionBreak.tag = "AttPlayer1";
                potionBreak.layer = 11;
            }
            else if (PlayerNum() == 2)
            {
                potionBreak.tag = "AttPlayer2";
                potionBreak.layer = 12;
            }
            StartCoroutine("Poison", potion);
            Destroy(potionBreak, 0.2f);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    IEnumerator Poison(GameObject potion)
    {
        potion.GetComponent<Collider2D>().enabled = false;
        potion.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        pos = potion.GetComponent<Rigidbody2D>().transform.position;
        Destroy(gameObject);
        GameObject puddle = Instantiate(Resources.Load("Scientist_Poison"), new Vector2(pos.x, heightPuddle), Quaternion.identity) as GameObject;
        if (PlayerNum() == 1)
        {
            puddle.tag = "AttPlayer1";
            puddle.layer = 11;
        }
        else if (PlayerNum() == 2)
        {
            puddle.tag = "AttPlayer2";
            puddle.layer = 12;
        }
        Destroy(puddle, 3f);
        Destroy(potion);
    }
}

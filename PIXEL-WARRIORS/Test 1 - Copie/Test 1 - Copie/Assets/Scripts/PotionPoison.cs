using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPoison : MonoBehaviour {

    private Rigidbody2D rb2d;
    private Vector2 pos;
    public GameObject puddle;

    public int playerNum;
    private float heightBreak;
    private float heightPuddle;
    private float heightSlimeWall = 0;
    private string platformName;
    private bool hit = false;
    private bool isCreated = false;

    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (gameObject.name == "Scientist_Potion_Poison(Clone)")
        {
            if (col.gameObject.name == "PlatformMiddleGround")
            {
                heightBreak = -0.1f; heightPuddle = 0.026f; hit = true;
            }
            else if (col.gameObject.name == "PlatformLeftGround1" || col.gameObject.name == "PlatformLeftGround2" || col.gameObject.name == "PlatformRightGround1" || col.gameObject.name == "PlatformRightGround2")
            {
                heightBreak = 0.625f; heightPuddle = 0.736f; hit = true;
            }
            else if (col.gameObject.name == "Front1" || col.gameObject.name == "Front2")
            {
                heightBreak = 0.181f; heightPuddle = 0.29f; hit = true;
            }
            else if (col.gameObject.name == "DeckLeft" || col.gameObject.name == "DeckRight")
            {
                heightBreak = -0.316f; heightPuddle = -0.208f; hit = true;
            }
            else if (col.gameObject.name == "MastLeft1" || col.gameObject.name == "MastLeft2" || col.gameObject.name == "MastRight1" || col.gameObject.name == "MastRight2")
            {
                heightBreak = 0.99f; heightPuddle = 1.09f; hit = true;
            }
            else if (col.gameObject.name == "LavaLeft" || col.gameObject.name == "LavaRight")
            {
                Destroy(gameObject);
            }
            else { hit = false; }

            PotionEffect(heightBreak, heightPuddle, hit);
        }

        if (gameObject.name == "Scientist_Potion_SlimeWall(Clone)")
        {
            platformName = col.gameObject.name;

            switch (platformName)
            {
                case "PlatformMiddleGround":
                    heightSlimeWall = 0.14f;
                    break;
                case "PlatformLeftGround1":
                case "PlatformLeftGround2":
                case "PlatformRightGround1":
                case "PlatformRightGround2":
                    heightSlimeWall = 0.89f;
                    break;
                case "Front1":
                case "Front2":
                    heightSlimeWall = 0.45f;
                    break;
                case "DeckLeft":
                case "DeckRight":
                    heightSlimeWall = -0.05f;
                    break;
                case "MastLeft1":
                case "MastLeft2":
                case "MastRight1":
                case "MastRight2":
                    heightSlimeWall = 1.25f;
                    break;
                case "LavaLeft":
                case "LavaRight":
                    Destroy(gameObject);
                    break;
            }

            if (heightSlimeWall != 0)
            {
                hit = true;
            }

            SlimeWall(heightSlimeWall, hit);
        }
    }
    
    private void SlimeWall(float heightSlimeWall, bool hit)
    {
        GameObject potion = gameObject;
        if (hit == true)
        {
            if (!isCreated)
            {
                Destroy(potion);
                hit = false;
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                pos = rb2d.transform.position;
                GameObject slimeWall = Instantiate(Resources.Load("Scientist_SlimeWall"), new Vector2(pos.x, heightSlimeWall), Quaternion.identity) as GameObject;
                isCreated = true;
                if (playerNum == 1)
                {
                    slimeWall.tag = "ShieldPlayer1";
                    slimeWall.layer = 13;
                }
                else if (playerNum == 2)
                {
                    slimeWall.tag = "ShieldPlayer2";
                    slimeWall.layer = 14;
                }
                Destroy(slimeWall, 3);
                StartCoroutine("Created");
            }
        }
        else
        {
            Destroy(potion, 2f);
        }
    }

    private void PotionEffect(float heightBreak, float heightPuddle, bool hit)
    {
        if (hit == true)
        {
            hit = false;
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            pos = rb2d.transform.position;
            GameObject potion = gameObject;
            GameObject potionBreak = Instantiate(Resources.Load("Scientist_PotionBreak"), new Vector2(pos.x, heightBreak), Quaternion.identity) as GameObject;
            if (playerNum == 1)
            {
                potionBreak.tag = "AttPlayer1";
                potionBreak.layer = 11;
            }
            else if (playerNum == 2)
            {
                potionBreak.tag = "AttPlayer2";
                potionBreak.layer = 12;
            }
            StartCoroutine("Poison", potion);
            Destroy(potionBreak, 0.2f);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
        
    }

    IEnumerator Created()
    {
        yield return new WaitForSeconds(0);
        isCreated = false;
    }

    IEnumerator Poison(GameObject potion)
    {
        potion.GetComponent<Collider2D>().enabled = false;
        potion.GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        pos = potion.GetComponent<Rigidbody2D>().transform.position;
        Destroy(gameObject);
        GameObject puddle = Instantiate(Resources.Load("Scientist_Poison"), new Vector2(pos.x, heightPuddle), Quaternion.identity) as GameObject;
        if (playerNum == 1)
        {
            puddle.tag = "AttPlayer1";
            puddle.layer = 11;
        }
        else if (playerNum == 2)
        {
            puddle.tag = "AttPlayer2";
            puddle.layer = 12;
        }
        Destroy(puddle, 3f);
        Destroy(potion);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBreak : MonoBehaviour {

    private Rigidbody2D rb2d;
   
    public Sprite spriteBreak;
    public Sprite puddle1;
    public Sprite puddle2;
    public Sprite puddle3;
    private SpriteRenderer spriteRenderer;

    private Vector2 pos;
    private Vector2 spritePos;

    public GameObject puddle;
    public Vector2 velocity;
    private Vector2 offset = new Vector2(0.2f, -0.2f);

    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spritePos.Set(0, 0.02786f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Map" || col.gameObject.tag == "Platform1" || col.gameObject.tag == "Platform2")
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            rb2d.transform.position = (pos - spritePos);
            spriteRenderer.sprite = spriteBreak;
            StartCoroutine("Puddle");
        }
    }

    void Update () {
        pos = transform.position;
    }

    IEnumerator Puddle()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);

        GameObject go = (GameObject)Instantiate(puddle, (Vector2)transform.position * transform.localScale.x, Quaternion.Euler(0, 0, 0));
        go.transform.position = (pos + new Vector2(0, 0.0979f));
        Destroy(go, 3);
    }
}

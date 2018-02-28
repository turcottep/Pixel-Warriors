using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThrough2 : MonoBehaviour {

    private Collider2D platform;
    private Rigidbody2D rb2d;
    private Animator anim;

    void Start()
    {

        platform = gameObject.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player 2" && Input.GetKey(KeyCode.DownArrow))
        {
            platform.GetComponent<Collider2D>().isTrigger = true;
            StopCoroutine("Wait");
            StartCoroutine("Wait");
        }

    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player 2" && Input.GetKey(KeyCode.DownArrow))
        {
            platform.GetComponent<Collider2D>().isTrigger = true;
            StopCoroutine("Wait");
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        platform.GetComponent<Collider2D>().isTrigger = false;
    }
}

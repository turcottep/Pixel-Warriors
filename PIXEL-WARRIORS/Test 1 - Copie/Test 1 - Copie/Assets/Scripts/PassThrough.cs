using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassThrough : MonoBehaviour {

    private Collider2D platform;
    private Rigidbody2D rb2d;
    private Animator anim;
    private Player player;

    public Button buttonDown;

    void Start () {
  
        platform = gameObject.GetComponent<Collider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        buttonDown.onClick.AddListener(pass);

        if (col.gameObject.tag == "Player 1" && Input.GetKey(col.gameObject.GetComponent<Player>().down))
        {
            pass();
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        buttonDown.onClick.AddListener(pass);

        if (col.gameObject.tag == "Player 1" && Input.GetKey(col.gameObject.GetComponent<Player>().down))
        {
            pass();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        platform.GetComponent<Collider2D>().enabled = true;
        //platform.GetComponent<Collider2D>().isTrigger = false;
    }

    public void pass()
    {
        platform.GetComponent<Collider2D>().enabled = false;
        //platform.GetComponent<Collider2D>().isTrigger = true;
        StopCoroutine("Wait");
        StartCoroutine("Wait");
    }
}

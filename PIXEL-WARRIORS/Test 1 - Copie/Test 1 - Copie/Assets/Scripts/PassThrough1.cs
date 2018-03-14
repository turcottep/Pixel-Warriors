using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThrough1 : MonoBehaviour {

    private Collider2D platform;
	private Player player;


	void Start () {
  
        platform = gameObject.GetComponent<Collider2D>();

	}

	private void OnCollisionEnter2D(Collision2D col)
    {
		
        if (col.gameObject.tag == "Player 2" && Input.GetKey(col.gameObject.GetComponent<Player>().down))
        {
            platform.GetComponent<Collider2D>().enabled = false;
            StopCoroutine("Wait");
            StartCoroutine("Wait");
        }
      
    }

    private void OnCollisionStay2D(Collision2D col)
    {
		if (col.gameObject.tag == "Player 2" && Input.GetKey(col.gameObject.GetComponent<Player>().down))
		{
			platform.GetComponent<Collider2D>().enabled = false;
			StopCoroutine("Wait");
			StartCoroutine("Wait");
		}
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.5f);
        platform.GetComponent<Collider2D>().enabled = true;
    }
}

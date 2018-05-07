using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    private GameObject shield;
    private Rigidbody2D rb2d;
    private new AudioManager audio;

    private int lives = 2;

    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        audio = FindObjectOfType<AudioManager>();
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (shield.tag == "ShieldPlayer1" && col.gameObject.tag == "AttPlayer2")
        {
            switch (shield.name)
            {
                case "Demon_Shield(Clone)":
                case "Scientist_SlimeWall(Clone)":
                    audio.Play("Shield_Hit", 0);
                    break;
            }

            Destroy(col.gameObject);
            lives--;
            shield.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (shield.tag == "ShieldPlayer2" && col.gameObject.tag == "AttPlayer1")
        {
            Destroy(col.gameObject);
            lives--;
            shield.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void Update () {
        shield = gameObject;

        if (lives == 0)
        {
            Destroy(shield);
        }
	}
}

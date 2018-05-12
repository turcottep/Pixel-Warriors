using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {

    Vector3 localScale;
    Player player;
    Color green = new Color(0, 199, 16, 255);
    Color orange = new Color(255, 135, 0, 255);
    Color red = new Color(255, 0, 0, 255);
    public bool isCharging = false;

	void Start ()
    {
        localScale = transform.localScale;
        player = GetComponentInParent<Player>();
        this.GetComponent<SpriteRenderer>().color = Color.white;

    }
	
	void Update ()
    {
        if (Player.chargeTime != 0)
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        if (Player.chargeTime == 0)
        {
            this.GetComponent<Renderer>().enabled = false;
        }
        if (Player.c == true)
        {
            Debug.Log("REEE");
            localScale.x = Player.chargeTime * 0.0607f;
            if (localScale.x < 0.0607f) { this.GetComponent<SpriteRenderer>().color = green; }
            if (localScale.x > 0.0607f && localScale.x < 0.1214f) { this.GetComponent<SpriteRenderer>().color = orange; }
            if (localScale.x > 0.1214f) { this.GetComponent<SpriteRenderer>().color = red; }
            transform.localScale = localScale;
        }
    }
}

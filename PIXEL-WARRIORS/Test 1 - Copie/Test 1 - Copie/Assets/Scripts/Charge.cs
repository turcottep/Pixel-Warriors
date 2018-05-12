using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour {

    Vector3 localScale;
    Color green = new Color(0, 199, 0, 255);
    Color orange = new Color(255, 135, 0, 255);
    Color red = new Color(255, 0, 0, 255);
    public static string color = "green";

	void Start ()
    {
        localScale = transform.localScale;
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
        if (Player.isCharging == true)
        {
            localScale.x = Player.chargeTime * 0.0607f;
            if (localScale.x < 0.0607f) { this.GetComponent<SpriteRenderer>().color = green; color = "green"; }
            if (localScale.x > 0.0607f && localScale.x < 0.1214f) { this.GetComponent<SpriteRenderer>().color = orange; color = "yellow"; }
            if (localScale.x > 0.152f) { this.GetComponent<SpriteRenderer>().color = red; color = "red"; }
            if (Player.fullyCharged == true) { localScale.x = 0.17f; color = "red"; }
            transform.localScale = localScale;
        }
    }
}

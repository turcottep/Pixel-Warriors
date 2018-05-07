using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rb2d;

    public float distance;
    public float dAvanceMin = 0.5f;
    public float dSauteMin = 0.8f;
    public float dAttack1Min = 0.05f;
    public float speed = 0.25f;
    public int direction = 0;
    private bool avance;
    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        player.speed = player.speed * speed;

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AIUpdate()
    {
        //Bug lorsque Scientist Special3
        //if (player.tag == "Player 2")
        //{
        // Debug.Log("2");

        //Debug.Log("YOUHHOUH");
        List<Transform> trous = new List<Transform>();

        Transform autre = GameObject.FindGameObjectWithTag("Player 1").transform;
        distance = autre.position.x - player.transform.position.x;
        if (!avance) player.isRight = distance > 0;

        trous.Add(GameObject.FindGameObjectWithTag("Hole 1").transform);
        GameObject temp = GameObject.FindGameObjectWithTag("Hole 2");
        if(temp!=null) trous.Add(temp.transform);
        List<float> distanceTrousX = new List<float>();
        List<float> distanceTrousY = new List<float>();

        foreach (Transform t in trous)
        {
            distanceTrousX.Add(t.position.x - this.transform.position.x);
            distanceTrousY.Add(t.position.y - this.transform.position.y);

        }


        bool saute = false;
        foreach (float d in distanceTrousX)
        {
            //Debug.Log("distanceTrou " + d * player.x);
            if (player.isRight) direction = 1;
            else direction = -1;
            saute = (d * direction > 0 && d * direction < dSauteMin);
            if (saute)
            {
                //Debug.Log("saute");
                player.MoveUp();
                break;
            }

        }

        avance = false;
        int trou = 0;
        for (int i = 0; i < distanceTrousX.Count; i++)
        {
            avance = Mathf.Abs(distanceTrousX[i]) < dAvanceMin;
            if (avance)
            {
                trou = i;
                break;
            }
        }
        if (avance)
        {
            //Over Edge

            //si il est en train de redescendre
            if (rb2d.velocity.y < -4.5)
            {
                player.MoveUp();
            }

            if (player.x == 0)
            {
                Debug.Log("Avance");
                if (player.isRight) { player.MoveRight(); }
                else { player.MoveLeft(); }
            }

        }
        else if (Mathf.Abs(distance) > dAttack1Min)
        {

            player.Special1(!player.charge, 0);
            //Debug.Log("Avance vers joueur");
            if (player.isRight) { player.MoveRight(); }
            else { player.MoveLeft(); }

        }
        else if (Mathf.Abs(distance) > 0)
        {
            player.x = 0;
            player.Basic1();
        }

        //}
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rb2d;

    public float distance;
    public float distanceY;
    public float dResauteMin = 0.2f;

    public float dAvanceMin = 0.5f;
    public float dSauteMin = 0.8f;
    public float dAttack1Min = 0.05f;
    public float dShootYMin = 0.2f;
    public float speed = 0.25f;
    public int direction = 0;
    private bool avance;

    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        if (player.aiON)
        {
                
            player.speed = player.speed * speed;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AIUpdate()
    {


        // Debug.Log("2");

        //Debug.Log("YOUHHOUH");
        List<Transform> trous = new List<Transform>();

        Transform autre = GameObject.FindGameObjectWithTag("Player 1").transform;
        distance = autre.position.x - player.transform.position.x;
        distanceY = autre.position.y - player.transform.position.y;
        if (!avance) player.isRight = distance > 0;

        trous.Add(GameObject.FindGameObjectWithTag("Hole 1").transform);
        //trous.Add(GameObject.FindGameObjectWithTag("Hole2").transform);
        List<float> distanceTrousX = new List<float>();
        List<float> distanceTrousY = new List<float>();

        foreach (Transform t in trous)
        {
            distanceTrousX.Add(t.position.x - this.transform.position.x);
            distanceTrousY.Add(this.transform.position.y - t.position.y);

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
                //player.MoveUp();
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
            if (distanceTrousY[0]< dResauteMin)
            {
                Debug.Log("d<0.2");
                player.MoveUp();
            }

            if (player.x == 0)
            {
                //Debug.Log("Avance");
                if (player.isRight) { player.MoveRight(); }
                else { player.MoveLeft(); }
            }

        }
        else if (Mathf.Abs(distance) > dAttack1Min)
        {
            //tracking player 
            if (player.shootCharge && Mathf.Abs(distanceY) < dShootYMin)
            {
                player.Attack2(false); // false = release
            }
            else
            {
                player.Attack2(true); // true = charge

            }
            //Debug.Log("Avance vers joueur");
            if (player.isRight) { player.MoveRight(); }
            else { player.MoveLeft(); }

        }
        else if (Mathf.Abs(distance) > 0.1)
        {
            //stop if close to player
            player.x = 0;
        }
        if (Mathf.Abs(distance) < dAttack1Min)
        {
            //attack if close enough
            player.Attack1();
        }


    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{

    private Player player;
    private Rigidbody2D rb2d;

    public float distance;
    public float dAvanceMin = 0.5f;
    public float dSauteMin = 0.1f;
    public float dAttack1Min = 0.05f;
    public float speedAI = 1;
    public int direction = 0;
    public bool overEdge;

    private bool avance;

    public float distanceToLeftEdge;
    public float distanceToRightEdge;
    public float heightDistanceToPlateform;
    public float distanceTrouX1;


    private Transform edgeLeft;
    private Transform edgeRight;

    private Transform plateformRight;
    private Transform plateformLeft;

    private bool firstJump;
    private bool firstJumpEdge;
    // Use this for initialization
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        player.speed = player.speed * speedAI;

        edgeLeft = GameObject.FindGameObjectWithTag("Edge Left").transform;
        edgeRight = GameObject.FindGameObjectWithTag("Edge Right").transform;

        plateformRight = GameObject.FindGameObjectWithTag("Plateform Right").transform;
        plateformLeft = GameObject.FindGameObjectWithTag("Plateform Left").transform;
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
        List<float> distanceTrousX = new List<float>();
        List<float> distanceTrousY = new List<float>();

        trous.Add(GameObject.FindGameObjectWithTag("Hole 1").transform);
        GameObject temp = GameObject.FindGameObjectWithTag("Hole 2");
        if (temp != null) trous.Add(temp.transform);

        Transform autre = GameObject.FindGameObjectWithTag("Player 1").transform;
        distance = autre.position.x - player.transform.position.x;
        if (!avance) player.isRight = distance > 0;

        distanceToLeftEdge = this.transform.position.x - edgeLeft.position.x;
        distanceToRightEdge = edgeRight.position.x - this.transform.position.x;

        heightDistanceToPlateform = this.transform.position.y - plateformLeft.position.y;

        foreach (Transform t in trous)
        {
            distanceTrousX.Add(t.position.x - this.transform.position.x);
            distanceTrousY.Add(t.position.y - this.transform.position.y);

        }


        //bool saute = false;
        //foreach (float d in distanceTrousX)
        //{
        //    //Debug.Log("distanceTrou " + d * player.x);
        //    if (player.isRight) direction = 1;
        //    else direction = -1;
        //    saute = (d * direction > 0 && d * direction < dSauteMin);
        //    if (saute)
        //    {
        //        //Debug.Log("saute");
        //        player.MoveUp();
        //        break;
        //    }

        //}

        avance = false;
        int trou = 0;
        //Debug.Log("nb trous = " + distanceTrousX.Count);
        for (int i = 0; i < distanceTrousX.Count; i++)
        {
            //Debug.Log("distance trou de " + i + " " + Mathf.Abs(distanceTrousX[i]) + " < " + dAvanceMin);
            avance = Mathf.Abs(distanceTrousX[i]) < dAvanceMin;
            if (avance)
            {
                trou = i;
                break;
            }
        }
        if (distanceToLeftEdge < 0 || distanceToRightEdge < 0)
        {
            //Debug.Log("Hors-Map");
            if (!firstJump)
            {
                //Debug.Log("firstJump, maxJump = " + player.maxJump);
                player.MoveUp();
                firstJump = true;
            }
            if (rb2d.velocity.y < 0)
            {
                player.MoveUp();
            }
            if (distanceToLeftEdge < 0)
            {
                player.MoveRight();
            }
            else if (distanceToRightEdge < 0)
            {
                player.MoveLeft();
            }
        }
        else
        {
            firstJump = false;

            if (avance)
            {
                //Over Edge
                overEdge = true;
                //si il est en train de redescendre
                if (rb2d.velocity.y < -4.5)
                {
                    player.MoveUp();
                }
                if (heightDistanceToPlateform < dSauteMin && rb2d.velocity.y < 0)
                {
                    Debug.Log("too low!");
                    if (!firstJumpEdge)
                    {
                        //Debug.Log("firstJump, maxJump = " + player.maxJump);
                        player.MoveUp();
                        firstJumpEdge = true;
                    }
                    else if (rb2d.velocity.y < 1)
                    {
                        Debug.Log("Jumping for second timme");
                        player.MoveUp();
                    }
                }
                if (player.x == 0)
                {
                    //Debug.Log("Avance");
                    if (player.isRight) { player.MoveRight(); }
                    else { player.MoveLeft(); }
                }

            }
            else
            {
                firstJumpEdge = false;
                overEdge = false;
                if (Mathf.Abs(distance) > dAttack1Min)
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
                player.MoveUp();

            }
            //}
        }

    }
}


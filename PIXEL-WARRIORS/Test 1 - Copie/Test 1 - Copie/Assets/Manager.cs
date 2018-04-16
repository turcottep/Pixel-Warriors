using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public TextMeshProUGUI timer;
    private float timeLeftSec;
    private float timeLeftMin;

    public TextMeshProUGUI textPercentageP1;
    public GameObject life1P1;
    public GameObject life2P1;
    public GameObject life3P1;

    public TextMeshProUGUI textPercentageP2;
    public GameObject life1P2;
    public GameObject life2P2;
    public GameObject life3P2;

    public int livesP1 = 3;
    public int livesP2 = 3;

    GameObject player1;
    GameObject player2;

    // Use this for initialization
    void Start()
    {
        string Character1;
        string Character2;



        timeLeftSec = 180;

        player1 = Instantiate(Resources.Load("Character1"), new Vector2(2.7f, 0.7f), Quaternion.identity) as GameObject;
        player1.tag = "Player 1";
        player1.layer = 8;
        player1.GetComponent<Player>().A = KeyCode.R;
        player1.GetComponent<Player>().B = KeyCode.F;
        player1.GetComponent<Player>().up = KeyCode.W;
        player1.GetComponent<Player>().left = KeyCode.A;
        player1.GetComponent<Player>().down = KeyCode.S;
        player1.GetComponent<Player>().right = KeyCode.D;

        player2 = Instantiate(Resources.Load("Character2"), new Vector2(-2.7f, 0.7f), Quaternion.identity) as GameObject;
        player1.tag = "Player 2";
        player1.layer = 9;
        player1.GetComponent<Player>().A = KeyCode.Comma;
        player1.GetComponent<Player>().B = KeyCode.Period;
        player1.GetComponent<Player>().up = KeyCode.UpArrow;
        player1.GetComponent<Player>().left = KeyCode.LeftArrow;
        player1.GetComponent<Player>().down = KeyCode.DownArrow;
        player1.GetComponent<Player>().right = KeyCode.RightArrow;
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer();
        updateLifeDisplay();
    }

    public void PlayerDeath(int playerNum)
    {
        if (playerNum == 1)
        {
            livesP1--;
            if (livesP1 == 0)
            {
                GameOver(2);
            }
        }
        else if (playerNum == 2)
        {
            livesP2--;
            if (livesP2 == 0)
            {
                GameOver(1);
            }
        }
    }

    private void updateTimer()
    {
        timeLeftSec -= Time.deltaTime;
        timeLeftMin = Mathf.Floor(timeLeftSec / 60);
    
        if ((timeLeftSec - (60 * timeLeftMin)) < 9.5)
        {
            timer.text = timeLeftMin.ToString() + ":0" + (timeLeftSec - (60 * timeLeftMin)).ToString("f0");
        }
        else
        {
            timer.text = timeLeftMin.ToString() + ":" + (timeLeftSec - (60 * timeLeftMin)).ToString("f0");
        }
    }

    public void updateLifeDisplay()
    {
        if (livesP1 == 3)
        {
            life1P1.SetActive(true);
            life2P1.SetActive(true);
            life3P1.SetActive(true);
        }
        else if (livesP1 == 2)
        {
            life1P1.SetActive(true);
            life2P1.SetActive(true);
            life3P1.SetActive(false);
        }
        else if (livesP1 == 1)
        {
            life1P1.SetActive(true);
            life2P1.SetActive(false);
            life3P1.SetActive(false);
        }
        else if (livesP1 == 0)
        {
            life1P1.SetActive(false);
            life2P1.SetActive(false);
            life3P1.SetActive(false);
        }

        if (livesP2 == 3)
        {
            life1P2.SetActive(true);
            life2P2.SetActive(true);
            life3P2.SetActive(true);
        }
        else if (livesP2 == 2)
        {
            life1P2.SetActive(true);
            life2P2.SetActive(true);
            life3P2.SetActive(false);
        }
        else if (livesP2 == 1)
        {
            life1P2.SetActive(true);
            life2P2.SetActive(false);
            life3P2.SetActive(false);
        }
        else if (livesP2 == 0)
        {
            life1P2.SetActive(false);
            life2P2.SetActive(false);
            life3P2.SetActive(false);
        }
    }

    public void UpdatePercentages()
    {
        textPercentageP1.SetText((20*player1.GetComponent<Player>().percentage).ToString());
        textPercentageP2.SetText((20 * player2.GetComponent<Player>().percentage).ToString());
    }


    public void GameOver(int result)
    {
        //Turcotte: to implement

        //canvas.SetActive(true);
        //if (result == 1)
        //{
        //    panelWin.SetActive(true);
        //}
        //else
        //{
        //    panelLost.SetActive(true);
        //}
        //initElo.SetText(elo.ToString());
        //float temp = Mathf.Floor(20 * (result - 1 / (1 + Mathf.Pow(10, (-1 * (elo - eloEnnemi) / 40)))));

        //deltaElo.SetText(temp.ToString());
        //sumElo.SetText((elo + temp).ToString());
        //cash.SetText((200 + 10 * temp).ToString());
    }

}

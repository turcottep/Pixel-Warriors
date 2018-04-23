using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    private float timeLeftSec;
    private float timeLeftMin;
    public TextMeshProUGUI countdown;

    public TextMeshProUGUI textPercentageP1;
    public GameObject life1P1;
    public GameObject life2P1;
    public GameObject life3P1;

    public TextMeshProUGUI textPercentageP2;
    public GameObject life1P2;
    public GameObject life2P2;
    public GameObject life3P2;

    public bool isButtonLeftPointerDownP2;
    public bool isButtonRightPointerDownP2;
    public bool isButtonDownPointerDownP2;

    public int livesP1 = 3;
    public int livesP2 = 3;

    GameObject player1;
    GameObject player2;

    GameObject player3;
    GameObject player4;

    void Start()
    {
        timeLeftSec = 150 + 4.5f;
        timer.gameObject.SetActive(false);

        string character1 = "Ninja";
        string character2 = "Demon";

        string character3 = "Ninja";
        string character4 = "Alien";

        int playerNumber = 3;
        /*
        GameObject mainMenuManager = GameObject.FindGameObjectWithTag("MainMenuManager");
        int playerNumber = mainMenuManager.GetComponent<MainMenu>().getPlayerNumber();
		*/

        if (playerNumber == 1) character1 = "Ninja";
        if (playerNumber == 2) character1 = "Alien";
        if (playerNumber == 3) character1 = "Scientist";
        if (playerNumber == 4) character1 = "Demon";

        player1 = Instantiate(Resources.Load(character1), new Vector2(-2.7f, 0.7f), Quaternion.identity) as GameObject;
        player1.GetComponent<Player>().playerType = playerNumber;
        player1.tag = "Player 1";
        player1.layer = 8;
        player1.GetComponent<Player>().jump = KeyCode.R;
        player1.GetComponent<Player>().A = KeyCode.T;
        player1.GetComponent<Player>().B = KeyCode.Y;
        player1.GetComponent<Player>().up = KeyCode.W;
        player1.GetComponent<Player>().left = KeyCode.A;
        player1.GetComponent<Player>().down = KeyCode.S;
        player1.GetComponent<Player>().right = KeyCode.D;
        player1.GetComponent<Player>().initialPosition = new Vector3(-2.7f, 0.7f, 0);

        player2 = Instantiate(Resources.Load(character2), new Vector2(-1.116f, 0.7f), Quaternion.identity) as GameObject;
        player2.GetComponent<Player>().playerType = playerNumber;
        player2.tag = "Player 1";
        player2.layer = 8;
        player2.GetComponent<Player>().jump = KeyCode.R;
        player2.GetComponent<Player>().A = KeyCode.T;
        player2.GetComponent<Player>().B = KeyCode.Y;
        player2.GetComponent<Player>().up = KeyCode.W;
        player2.GetComponent<Player>().left = KeyCode.A;
        player2.GetComponent<Player>().down = KeyCode.S;
        player2.GetComponent<Player>().right = KeyCode.D;
        player2.GetComponent<Player>().initialPosition = new Vector3(-1.116f, 0.7f, 0);

        player3 = Instantiate(Resources.Load(character3), new Vector2(0.655f, 0.7f), Quaternion.identity) as GameObject;
        player3.GetComponent<Player>().playerType = playerNumber;
        player3.tag = "Player 1";
        player3.layer = 8;
        player3.GetComponent<Player>().jump = KeyCode.R;
        player3.GetComponent<Player>().A = KeyCode.T;
        player3.GetComponent<Player>().B = KeyCode.Y;
        player3.GetComponent<Player>().up = KeyCode.W;
        player3.GetComponent<Player>().left = KeyCode.A;
        player3.GetComponent<Player>().down = KeyCode.S;
        player3.GetComponent<Player>().right = KeyCode.D;
        player3.GetComponent<Player>().initialPosition = new Vector3(0.655f, 0.7f, 0);

        player4 = Instantiate(Resources.Load(character4), new Vector2(2.66f, 0.7f), Quaternion.identity) as GameObject;
        player4.GetComponent<Player>().playerType = playerNumber;
        player4.tag = "Player 1";
        player4.layer = 8;
        player4.GetComponent<Player>().jump = KeyCode.R;
        player4.GetComponent<Player>().A = KeyCode.T;
        player4.GetComponent<Player>().B = KeyCode.Y;
        player4.GetComponent<Player>().up = KeyCode.W;
        player4.GetComponent<Player>().left = KeyCode.A;
        player4.GetComponent<Player>().down = KeyCode.S;
        player4.GetComponent<Player>().right = KeyCode.D;
        player4.GetComponent<Player>().initialPosition = new Vector3(2.66f, 0.7f, 0);



        /*player1 = Instantiate(Resources.Load(character1), new Vector2(-2.7f, 0.7f), Quaternion.identity) as GameObject;
        player1.GetComponent<Player>().playerType = playerNumber;
        player1.tag = "Player 1";
        player1.layer = 8;
        player1.GetComponent<Player>().jump = KeyCode.R;
        player1.GetComponent<Player>().A = KeyCode.T;
        player1.GetComponent<Player>().B = KeyCode.Y;
        player1.GetComponent<Player>().up = KeyCode.W;
        player1.GetComponent<Player>().left = KeyCode.A;
        player1.GetComponent<Player>().down = KeyCode.S;
        player1.GetComponent<Player>().right = KeyCode.D;
        player1.GetComponent<Player>().initialPosition = new Vector3(-2.7f, 0.7f, 0);
        //player1.GetComponent<GameObject>().GetComponent<EdgeCollider2D>().tag = "Player 1";
        //player1.GetComponentInChildren<Player>().tag = "Player 1";

        /*foreach (Transform t in transform)
        {
            t.gameObject.tag = "Player 1";
        }
        gameObject.tag = "Player 1";*/


        /*player2 = Instantiate(Resources.Load(character2), new Vector2(2.7f, 0.7f), Quaternion.identity) as GameObject;
        player2.tag = "Player 2";
       
        player2.layer = 9;
        player2.GetComponent<Player>().jump = KeyCode.M;
        player2.GetComponent<Player>().A = KeyCode.Comma;
        player2.GetComponent<Player>().B = KeyCode.Period;
        player2.GetComponent<Player>().up = KeyCode.UpArrow;
        player2.GetComponent<Player>().left = KeyCode.LeftArrow;
        player2.GetComponent<Player>().down = KeyCode.DownArrow;
        player2.GetComponent<Player>().right = KeyCode.RightArrow;
        player2.GetComponent<Player>().initialPosition = new Vector3(2.7f, 0.7f, 0);
        //player2.GetComponentInChildren<GameObject>().tag = "Player 2";

        player2.GetComponent<Player>().aiON = true;*/
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

        if (timeLeftSec > 150.5)
        {
            if ((timeLeftSec - 151).ToString("f0") == "0") countdown.text = "GO!";
            else countdown.text = (timeLeftSec - 151).ToString("f0");
        }
        else if ((timeLeftSec - (60 * timeLeftMin)) < 9.5)
        {
            timer.text = timeLeftMin.ToString() + ":0" + (timeLeftSec - (60 * timeLeftMin)).ToString("f0");
        }
        else
        {
            countdown.gameObject.SetActive(false);
            timer.gameObject.SetActive(true);
            timer.text = timeLeftMin.ToString() + ":" + (timeLeftSec - (60 * timeLeftMin)).ToString("f0");
        }

        if (timeLeftSec == 0)
        {
            GameOver(0);
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
        textPercentageP1.SetText((20 * player1.GetComponent<Player>().percentage).ToString() + "%");
        textPercentageP2.SetText((20 * player2.GetComponent<Player>().percentage).ToString() + "%");
    }


    public void GameOver(int result)
    {
        Debug.Log("Game is over: Winner = player " + result);
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

    //UI Will
    #region UI P1
    //P1
    public void buttonJumpPointerDownP1()
    {
        //player1.GetComponent<Player>().rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        //player1.GetComponent<Player>().isDead = false;
        //player1.GetComponent<Player>().dead = false;

        player1.GetComponent<Player>().MoveUp();
    }

    public void buttonAttackAPointerDownP1()
    {
        player1.GetComponent<Player>().Basic1();
    }

    public void buttonAttackBPointerDownP1()
    {
        player1.GetComponent<Player>().isButtonAttackBPointerDown = true;
    }

    public void buttonAttackBPointerUpP1()
    {
        player1.GetComponent<Player>().isButtonAttackBPointerDown = false;
    }

    public void buttonLeftPointerDownP1()
    {
        player1.GetComponent<Player>().isButtonLeftPointerDown = true;
    }

    public void buttonLeftPointerUpP1()
    {
        player1.GetComponent<Player>().isButtonLeftPointerDown = false;
    }

    public void buttonRightPointerDownP1()
    {
        player1.GetComponent<Player>().isButtonRightPointerDown = true;
    }

    public void buttonRightPointerUpP1()
    {
        player1.GetComponent<Player>().isButtonRightPointerDown = false;
    }

    public void buttonDownPointerDownP1()
    {
        player1.GetComponent<Player>().MoveDown();
        player1.GetComponent<Player>().pressDown = true;
    }

    public void buttonDownPointerUpP1()
    {
        player1.GetComponent<Player>().pressDown = false;
    }

    public void buttonUpPointerDownP1()
    {
        player1.GetComponent<Player>().pressUp = true;
    }

    public void buttonUpPointerUpP1()
    {
        player1.GetComponent<Player>().pressUp = false;
    }
    #endregion
}
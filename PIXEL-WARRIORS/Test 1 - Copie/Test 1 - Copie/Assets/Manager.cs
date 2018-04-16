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

	public bool isButtonLeftPointerDownP2;
	public bool isButtonRightPointerDownP2;
	public bool isButtonDownPointerDownP2;

	public int livesP1 = 3;
    public int livesP2 = 3;

    GameObject player1;
    GameObject player2;

    // Use this for initialization
    void Start()
    {
        string character1 = "Ninja";
        string character2 = "Ninja";

		int playerNumber = 2;
		/*
        GameObject mainMenuManager = GameObject.FindGameObjectWithTag("MainMenuManager");
        int playerNumber = mainMenuManager.GetComponent<MainMenu>().getPlayerNumber();
		*/

        if (playerNumber == 1) character1 = "Ninja";
        if (playerNumber == 2) character1 = "Alien";
        if (playerNumber == 3) character1 = "MadScientist";
        if (playerNumber == 4) character1 = "Demon";
        

		timeLeftSec = 180;

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


		player2 = Instantiate(Resources.Load(character2), new Vector2(2.7f, 0.7f), Quaternion.identity) as GameObject;
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

		//player2.GetComponent<Player>().aiON = true;

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
		player1.GetComponent<Player>().rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		player1.GetComponent<Player>().isDead = false;
		player1.GetComponent<Player>().dead = false;

		player1.GetComponent<Player>().MoveUp();
	}

	public void buttonAttackAPointerDownP1()
	{
		player1.GetComponent<Player>().Basic1();
	}

	public void buttonAttackBPointerDownP1()
	{
		//this.Special1(true);
	}

	public void buttonAttackBPointerUpP1()
	{
		//Special1(false);
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
	}

	public void buttonUpPointerDownP1()
	{
		player1.GetComponent<Player>().MoveUp();
	}
#endregion

	#region UI P2
	//P2
	public void buttonJumpPointerDownP2()
	{
		player2.GetComponent<Player>().rb2d.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
		player2.GetComponent<Player>().isDead = false;
		player2.GetComponent<Player>().dead = false;

		player2.GetComponent<Player>().MoveUp();
	}

	public void buttonAttackAPointerDownP2()
	{
		player2.GetComponent<Player>().Basic1();
	}

	public void buttonAttackBPointerDownP2()
	{
		//this.Special1(true);
	}

	public void buttonAttackBPointerUpP2()
	{
		//Special1(false);
	}

	public void buttonLeftPointerDownP2()
	{
		player2.GetComponent<Player>().isButtonLeftPointerDown = true;
	}

	public void buttonLeftPointerUpP2()
	{
		player2.GetComponent<Player>().isButtonLeftPointerDown = false;
	}

	public void buttonRightPointerDownP2()
	{
		player2.GetComponent<Player>().isButtonRightPointerDown = true;
	}

	public void buttonRightPointerUpP2()
	{
		player2.GetComponent<Player>().isButtonRightPointerDown = false;
	}

	public void buttonDownPointerDownP2()
	{
		player2.GetComponent<Player>().MoveDown();
	}

	public void buttonUpPointerDownP2()
	{
		player2.GetComponent<Player>().MoveUp();
	}
	#endregion
}

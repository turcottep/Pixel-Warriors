using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Manager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    private float timeLeftSec;
    private float timeLeftMin;
    public TextMeshProUGUI countdown;

    public int gameMode = 0;

    public GameObject waitingScreen;

    public GameObject headP1_1;
    public GameObject headP1_2;
    public GameObject headP1_3;
    public GameObject headP1_4;

    public GameObject headP2_1;
    public GameObject headP2_2;
    public GameObject headP2_3;
    public GameObject headP2_4;

    public TextMeshProUGUI textPercentageP1;
    public GameObject life1P1;
    public GameObject life2P1;
    public GameObject life3P1;

    public TextMeshProUGUI textPercentageP2;
    public GameObject life1P2;
    public GameObject life2P2;
    public GameObject life3P2;

    public Image cooldown_image1;
    public Image cooldown_image2;
    public Image cooldown_image3;

    public bool coolingDown1;
    public bool coolingDown2;
    public bool coolingDown3;
    public float coolDownTime;

    public bool isButtonLeftPointerDownP2;
    public bool isButtonRightPointerDownP2;
    public bool isButtonDownPointerDownP2;

    public int livesP1 = 3;
    public int livesP2 = 3;

    private bool isStarted = false;

    GameObject player1;
    GameObject player2;


    void Start()
    {
        timeLeftSec = 150 + 4.5f;
        timer.gameObject.SetActive(false);

        string character1 = "Ninja";
        string character2 = "Scientist";

        int playerNumberP1 = 2;
        int playerNumberP2 = 1;

        GameObject mainMenuManager = GameObject.FindGameObjectWithTag("MainMenuManager");
        if (mainMenuManager != null)
        {
            playerNumberP1 = mainMenuManager.GetComponent<MainMenu>().getPlayerNumber();
            gameMode = mainMenuManager.GetComponent<MainMenu>().getGameMode();
            int num = PhotonNetwork.room.PlayerCount;
            Debug.Log("num = " + num);
        }

        if (playerNumberP1 == 1) character1 = "Ninja";
        if (playerNumberP1 == 2) character1 = "Alien";
        if (playerNumberP1 == 3) character1 = "Scientist";
        if (playerNumberP1 == 4) character1 = "Demon";
        if (playerNumberP2 == 1) character2 = "Ninja";
        if (playerNumberP2 == 2) character2 = "Alien";
        if (playerNumberP2 == 3) character2 = "Scientist";
        if (playerNumberP2 == 4) character2 = "Demon";

        if (gameMode == 2)
        {
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                player1 = PhotonNetwork.Instantiate(character1, new Vector2(-2.7f, 0.9f), Quaternion.identity, 0);

            }
            else
            {
                isStarted = true;
                player2 = PhotonNetwork.Instantiate(character1, new Vector2(2.7f, 0.9f), Quaternion.identity, 0);
                player2.tag = "Player 2";
                player2.layer = 9;
                GameObject piedsJ2 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP1);
                piedsJ2.layer = player2.layer;
                piedsJ2.tag = player2.tag;

            }
        }
        else if (gameMode != 2)
        {
            player1 = Instantiate(Resources.Load(character1), new Vector2(-2.7f, 0.9f), Quaternion.identity) as GameObject;
            player1.GetComponent<Player>().playerType = playerNumberP1;
            player1.tag = "Player 1";
            player1.layer = 8;
            player1.GetComponent<Player>().jump = KeyCode.R;
            player1.GetComponent<Player>().A = KeyCode.T;
            player1.GetComponent<Player>().B = KeyCode.Y;
            player1.GetComponent<Player>().up = KeyCode.W;
            player1.GetComponent<Player>().left = KeyCode.A;
            player1.GetComponent<Player>().down = KeyCode.S;
            player1.GetComponent<Player>().right = KeyCode.D;
            player1.GetComponent<Player>().initialPosition = new Vector3(-2.7f, 0.9f, 0);
            GameObject piedsJ1 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP1);
            piedsJ1.layer = player1.layer;
            piedsJ1.tag = player1.tag;

            player2 = Instantiate(Resources.Load(character2), new Vector2(2.7f, 0.9f), Quaternion.identity) as GameObject;
            player2.GetComponent<Player>().playerType = playerNumberP2;
            player2.tag = "Player 2";
            player2.layer = 9;
            player2.GetComponent<Player>().jump = KeyCode.M;
            player2.GetComponent<Player>().A = KeyCode.Comma;
            player2.GetComponent<Player>().B = KeyCode.Period;
            player2.GetComponent<Player>().up = KeyCode.UpArrow;
            player2.GetComponent<Player>().left = KeyCode.LeftArrow;
            player2.GetComponent<Player>().down = KeyCode.DownArrow;
            player2.GetComponent<Player>().right = KeyCode.RightArrow;
            player2.GetComponent<Player>().initialPosition = new Vector3(2.7f, 0.9f, 0);
            GameObject piedsJ2 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP2);
            piedsJ2.layer = player2.layer;
            piedsJ2.tag = player2.tag;
        }


        if (gameMode == 1)
        {
            player2.GetComponent<Player>().aiON = true;
        }


        setHeads(playerNumberP1, playerNumberP2);
    }

    void Update()
    {
        if (PhotonNetwork.room.PlayerCount == 1)
        {
            player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            //waitingScreen.SetActive(true);
        }
        else if (PhotonNetwork.room.PlayerCount == 2)
        {


            updateTimer();
            updateLifeDisplay();

            //Cooldown
            if (coolingDown1 == true)
            {
                cooldown_image1.gameObject.SetActive(true);
                coolDownTime = 5.0f;
                cooldown_image1.fillAmount -= 1.0f / coolDownTime * Time.deltaTime;
            }
            if (coolingDown2 == true)
            {
                cooldown_image2.gameObject.SetActive(true);
                coolDownTime = 4.0f;
                cooldown_image2.fillAmount -= 1.0f / coolDownTime * Time.deltaTime;
            }
        }
        else
        {
            //if (isStarted) OtherPlayerQuit();
        }
    }

    public void isCoolingDown(int x) /////
    {
        switch (x)
        {
            case 2:
                coolingDown2 = true;
                break;
        }
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
            if ((timeLeftSec - 151).ToString("f0") == "0")
            {
                countdown.text = "GO!";
                //waitingScreen.SetActive(false);
                player2 = GameObject.FindGameObjectWithTag("Player 2");
                player1 = GameObject.FindGameObjectWithTag("Player 1");

                player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }
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

    public int getGameMode()
    {
        return gameMode;
    }

    //UI Will

    void setHeads(int p1, int p2)
    {
        switch (p1)
        {
            case 1:
                headP1_1.SetActive(true);
                break;
            case 2:
                headP1_2.SetActive(true);
                break;
            case 3:
                headP1_3.SetActive(true);
                break;
            case 4:
                headP1_4.SetActive(true);
                break;
        }

        switch (p2)
        {
            case 1:
                headP2_1.SetActive(true);
                break;
            case 2:
                headP2_2.SetActive(true);
                break;
            case 3:
                headP2_3.SetActive(true);
                break;
            case 4:
                headP2_4.SetActive(true);
                break;
        }
    }

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
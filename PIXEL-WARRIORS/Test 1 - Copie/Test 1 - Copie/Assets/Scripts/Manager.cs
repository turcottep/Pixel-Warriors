﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Manager : MonoBehaviour
{

    #region public variables
    public TextMeshProUGUI timer;
    public TextMeshProUGUI countdown;
    public int gameMode = 1;

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

    //endgame objects
    public TextMeshProUGUI initElo;
    public GameObject imageWin;
    public GameObject imageLost;
    private int elo = 1200;      // Lafleur il faut le cop
    private int eloEnnemi = 1400;// Lafleur il faut le cop
    public TextMeshProUGUI deltaElo;
    public TextMeshProUGUI sumElo;
    public TextMeshProUGUI cash;
    public GameObject canvas;

    public bool coolingDown1;
    public bool coolingDown2;
    public bool coolingDown3;
    public float coolDownTime;

    public bool isButtonLeftPointerDownP2;
    public bool isButtonRightPointerDownP2;
    public bool isButtonDownPointerDownP2;

    public int livesP1 = 3;
    public int livesP2 = 3;

    public int mapN = 1;

    private new AudioManager audio;
    public bool sound;
    public bool music;
    public bool isStarted = false;

    public float gold;
    #endregion

    #region private variables
    private float timeLeftSec;
    private float timeLeftMin;

    private bool canCountDown = true;
    private bool isConnected = false;

    private GameObject player1;
    private GameObject player2;

    private Vector2 initialPositionP1;
    private Vector2 initialPositionP2;

    #endregion


    void Start()
    {

        timeLeftSec = 150 + 4.5f;
        timer.gameObject.SetActive(false);

        string character1 = "Ninja";
        string character2 = "Scientist";

        int playerNumberP1 = 4;
        int playerNumberP2 = 1;

        // get info from main menu
        GameObject mainMenuManager = GameObject.FindGameObjectWithTag("MainMenuManager");

        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        music = audio.musicOn;
        sound = audio.soundOn;
        Debug.Log("Music is : " + music);

        if (mainMenuManager != null)
        {
            playerNumberP1 = mainMenuManager.GetComponent<MainMenu>().getPlayerNumber();
            gameMode = mainMenuManager.GetComponent<MainMenu>().getGameMode();
            mapN = mainMenuManager.GetComponent<MainMenu>().getMapNumber();

            if (mapN == 1)
            {
                if (music)
                {
                    audio.Stop("MusicMenu", 0, music);
                    audio.Play("MusicMap1", 0, music);
                }

                initialPositionP1 = new Vector2(-2.7f, 1.1f);
                initialPositionP2 = new Vector2(2.7f, 1.1f);
            }
            else if (mapN == 2)
            {
                if (music)
                {
                    audio.Stop("MusicMenu", 0, music);
                    audio.Play("MusicMap2", 0, music);
                }

                initialPositionP1 = new Vector3(-2, 1.6f, 0);
                initialPositionP2 = new Vector3(2, 1.6f, 0);
            }
        }
        else
        {
            //if when, for testing, no menu exists  
            gameMode = 1;
            initialPositionP1 = new Vector2(-2.7f, 1.5f);
            initialPositionP2 = new Vector2(2.7f, 1.5f);
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
            //if in multiplayer mode
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                //if you are the first in the room
                player1 = PhotonNetwork.Instantiate(character1, initialPositionP1, Quaternion.identity, 0);
                player1.tag = "Player 1";
                player1.layer = 8;
                player1.GetComponent<Player>().playerNum = 1;

                GameObject piedsJ1 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP1);
                piedsJ1.layer = player1.layer;
                piedsJ1.tag = player1.tag;
                player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                waitingScreen.SetActive(true);
                canCountDown = false;
                StartCoroutine("WaitForOtherPlayer");
            }
            else
            {
                //if you arrive in a room with a player present
                player2 = PhotonNetwork.Instantiate(character1, initialPositionP2, Quaternion.identity, 0);
                player2.tag = "Player 2";
                player2.layer = 9;
                player2.GetComponent<Player>().playerNum = 2;
                GameObject piedsJ2 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP1);
                piedsJ2.layer = player2.layer;
                piedsJ2.tag = player2.tag;

                StartCoroutine("WaitForMapToLoad");

            }
        }
        else
        {
            //if in singleplayer
            player1 = Instantiate(Resources.Load(character1), initialPositionP1, Quaternion.identity) as GameObject;
            player1.tag = "Player 1";
            player1.layer = 8;
            player1.GetComponent<Player>().jump = KeyCode.R;
            player1.GetComponent<Player>().A = KeyCode.T;
            player1.GetComponent<Player>().B = KeyCode.Y;
            player1.GetComponent<Player>().up = KeyCode.W;
            player1.GetComponent<Player>().left = KeyCode.A;
            player1.GetComponent<Player>().down = KeyCode.S;
            player1.GetComponent<Player>().right = KeyCode.D;
            GameObject piedsJ1 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP1);
            piedsJ1.layer = player1.layer;
            piedsJ1.tag = player1.tag;

            player2 = Instantiate(Resources.Load(character2), initialPositionP2, Quaternion.identity) as GameObject;
            player2.tag = "Player 2";
            player2.layer = 9;
            player2.GetComponent<Player>().jump = KeyCode.M;
            player2.GetComponent<Player>().A = KeyCode.Comma;
            player2.GetComponent<Player>().B = KeyCode.Period;
            player2.GetComponent<Player>().up = KeyCode.UpArrow;
            player2.GetComponent<Player>().left = KeyCode.LeftArrow;
            player2.GetComponent<Player>().down = KeyCode.DownArrow;
            player2.GetComponent<Player>().right = KeyCode.RightArrow;
            GameObject piedsJ2 = GameObject.FindGameObjectWithTag("Feet" + playerNumberP2);
            piedsJ2.layer = player2.layer;
            piedsJ2.tag = player2.tag;

            player2.GetComponent<Player>().aiON = true;
            player2.GetComponent<Player>().speed = 2.5f;
            setHeads(playerNumberP1, playerNumberP2);
        }
    }

    void Update()
    {

        if (canCountDown) updateTimer();

        if (isStarted)
        {
            //player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            //player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

            updateLifeDisplay();
            //Cooldowns
            //Cooldown1
            if (coolingDown1 == false)
            {
                cooldown_image1.gameObject.SetActive(false);
                cooldown_image1.fillAmount = 1;
            }
            else if (coolingDown1 == true)
            {
                cooldown_image1.gameObject.SetActive(true);
                coolDownTime = 1.0f;
                cooldown_image1.fillAmount -= 1.0f / coolDownTime * Time.deltaTime;
            }
            //Cooldown2
            if (coolingDown2 == false)
            {
                cooldown_image2.gameObject.SetActive(false);
                cooldown_image2.fillAmount = 1;
            }
            else if (coolingDown2 == true)
            {
                cooldown_image2.gameObject.SetActive(true);
                coolDownTime = 4.0f;
                cooldown_image2.fillAmount -= 1.0f / coolDownTime * Time.deltaTime;
            }
            //Cooldown3
            if (coolingDown3 == false)
            {
                cooldown_image3.gameObject.SetActive(false);
                cooldown_image3.fillAmount = 1;
            }
            else if (coolingDown3 == true)
            {
                cooldown_image3.gameObject.SetActive(true);
                coolDownTime = 6.0f;
                cooldown_image3.fillAmount -= 1.0f / coolDownTime * Time.deltaTime;
            }
        }

    }

    /*public void isCoolingDown(int x)
    {
        switch (x)
        {
            case 2:
                coolingDown2 = true;
                break;
        }
    }*/

    public void PlayerDeath(int playerNum)
    {
        if (playerNum == 1)
        {
            livesP1--;
            if (livesP1 == 0)
            {
                GameOver(2);
            }
            player1.transform.position = initialPositionP1;
        }
        else if (playerNum == 2)
        {
            livesP2--;
            if (livesP2 == 0)
            {
                GameOver(1);
            }
            player2.transform.position = initialPositionP2;
        }
    }

    private void TriggerStart()
    {
        Debug.Log("GOGOGO");

        player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        setHeads(player1.GetComponent<Player>().playerType, player2.GetComponent<Player>().playerType);
        isStarted = true;
    }


    private void updateTimer()
    {
        timeLeftSec -= Time.deltaTime;
        timeLeftMin = Mathf.Floor(timeLeftSec / 60);
        if (timeLeftSec < 151.52 && timeLeftSec > 151.5) this.TriggerStart();
        if (timeLeftSec > 150.5)
        {
            if ((timeLeftSec - 151).ToString("f0") == "0")
            {
                countdown.text = "GO!";
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

        if (timeLeftSec <= 0)
        {
            if (isStarted)
            {
                GameOver(0);
                timeLeftSec = 0;
            }


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

    public void UpdatePercentages(int playernum)
    {

        if (playernum == 1) textPercentageP1.SetText((20 * Math.Ceiling(player1.GetComponent<Player>().percentage)).ToString() + "%");
        else if (playernum == 2) textPercentageP2.SetText((20 * Math.Ceiling(player2.GetComponent<Player>().percentage)).ToString() + "%");
    }


    public void GameOver(int winner)
    {
        isStarted = false;
        canCountDown = false;
        Debug.Log("Game is over: Winner = player " + winner);
        player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        canvas.SetActive(true);
        int result;
        if (winner != 1 && winner != 2)
        {
            if (livesP1 > livesP2)
            {
                winner = 1;
            }
            else if (livesP1 < livesP2)
            {
                winner = 2;
            }
            else
            {
                if (player1.GetComponent<Player>().percentage > player2.GetComponent<Player>().percentage)
                {
                    winner = 2;
                }
                else winner = 1;
            }
        }
        if (winner == 1)
        {
            Destroy(player2);
            imageWin.SetActive(true);
            result = 1;
        }
        else
        {
            Destroy(player1);
            imageLost.SetActive(true);
            result = -1;
        }

        initElo.SetText(elo.ToString());
        float temp = Mathf.Floor(20 * (result - 1 / (1 + Mathf.Pow(10, (-1 * (elo - eloEnnemi) / 40)))));

        deltaElo.SetText(temp.ToString());
        sumElo.SetText((elo + temp).ToString());
        Debug.Log("gold = " + gold);
        if (10 * temp < -200)
        {
            temp = -19;
        }
        gold += 200 + 10 * temp;
        Debug.Log("gold = " + gold);
        cash.SetText(gold.ToString());
    }

    public void BackToMenu()
    {
        PlayerPrefs.SetFloat("gold", gold);
        PhotonNetwork.LoadLevel("Menu");
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

    #region Multiplayer
    IEnumerator WaitForOtherPlayer()
    {
        yield return new WaitForSeconds(1f);
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            //wait one more second
            if (!isConnected)
            {
                Debug.Log("waiting 1 more second");
                isConnected = true;
                StartCoroutine("WaitForOtherPlayer");
            }
            else
            {
                player2 = GameObject.FindGameObjectWithTag("UnassignedPlayer");
                if (player2 == null) Debug.Log("UNABLE to find connecting payer");
                player2.tag = "Player 2";
                player2.layer = 9;
                player2.GetComponent<Player>().playerNum = 2;
                int player2Type = player2.GetComponent<Player>().playerType;
                GameObject piedsJ2 = GameObject.FindGameObjectWithTag("Feet" + player2Type);
                piedsJ2.layer = player2.layer;
                piedsJ2.tag = player2.tag;

                waitingScreen.SetActive(false);
                canCountDown = true;
            }

        }
        else
        {
            Debug.Log("Waiting");
            StartCoroutine("WaitForOtherPlayer");
        }
    }

    IEnumerator WaitForMapToLoad()
    {
        yield return new WaitForSeconds(1f);
        player1 = GameObject.FindGameObjectWithTag("UnassignedPlayer");
        player1.tag = "Player 1";
        player1.layer = 8;
        player1.GetComponent<Player>().playerNum = 1;
        int player1Type = player1.GetComponent<Player>().playerType;
        GameObject piedsJ1 = GameObject.FindGameObjectWithTag("Feet" + player1Type);
        piedsJ1.layer = player1.layer;
        piedsJ1.tag = player1.tag;
    }
    #endregion

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

    private void Awake()
    {
        gold = PlayerPrefs.GetFloat("gold", 0f);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("gold", gold);
    }
}
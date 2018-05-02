using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class MainMenu : MonoBehaviour
{

    public Toggle toggleMap1;
    public Toggle toggleMap2;

    public Toggle togglePlayer1;
    public GameObject p1Background;
    public Toggle togglePlayer2;
    public GameObject p2Background;
    public Toggle togglePlayer3;
    public GameObject p3Background;
    public Toggle togglePlayer4;
    public GameObject p4Background;


    public int mapNumber;
    public int playerNumber = 0;

    private int gameMode = 1;

    GameObject player1;

    public void Awake()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainMenuManager"));
        //player1 = Instantiate(Resources.Load("Ninja"), new Vector2(-2.7f, 0.9f), Quaternion.identity) as GameObject;
        //player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainMenuManager"));

    }

    public void playAI()
    {
        SelectPlayer();
        gameMode = 1;

        if (toggleMap1.isOn)
        {
            toggleMap2.isOn = false;
            mapNumber = 1;

            PhotonNetwork.LoadLevel("MAP1");

        }
        else if (toggleMap2.isOn)
        {
            toggleMap1.isOn = false;
            mapNumber = 2;

            PhotonNetwork.LoadLevel("MAP2");

        }

    }

    public void playOnline()
    {
        SelectPlayer();
        gameMode = 2;

        if (playerNumber != 0)
        {
            if (toggleMap1.isOn)
            {
                //Lava
                mapNumber = 1;
            }
            else if (toggleMap2.isOn)
            {
                //Boat
                mapNumber = 2;
            }

            PhotonNetwork.LoadLevel("Launcher");

        }
    }

    public void unSelect1()
    {
        if (toggleMap2.isOn)
        {
            toggleMap1.isOn = false;
        }
    }
    public void unSelect2()
    {
        if (toggleMap1.isOn)
        {
            toggleMap2.isOn = false;
        }
    }

    public void SelectP1()
    {
        if (togglePlayer1.isOn)
        {
            togglePlayer2.isOn = false;
            togglePlayer3.isOn = false;
            togglePlayer4.isOn = false;
        }
        UpdatePlayerBackground();
    }
    public void SelectP2()
    {
        if (togglePlayer2.isOn)
        {
            togglePlayer1.isOn = false;
            togglePlayer3.isOn = false;
            togglePlayer4.isOn = false;
        }
        UpdatePlayerBackground();
    }
    public void SelectP3()
    {
        if (togglePlayer3.isOn)
        {
            togglePlayer1.isOn = false;
            togglePlayer2.isOn = false;
            togglePlayer4.isOn = false;
        }
        UpdatePlayerBackground();
    }
    public void SelectP4()
    {
        if (togglePlayer4.isOn)
        {
            togglePlayer1.isOn = false;
            togglePlayer2.isOn = false;
            togglePlayer3.isOn = false;
        }
        UpdatePlayerBackground();
    }

    private void UpdatePlayerBackground()
    {
        p1Background.SetActive(togglePlayer1.isOn);
        p2Background.SetActive(togglePlayer2.isOn);
        p3Background.SetActive(togglePlayer3.isOn);
        p4Background.SetActive(togglePlayer4.isOn);
    }

    public void SelectPlayer()
    {
        if (togglePlayer1.isOn)
        {
            playerNumber = 1;
        }
        else if (togglePlayer2.isOn)
        {
            playerNumber = 2;
        }
        else if (togglePlayer3.isOn)
        {
            playerNumber = 3;
        }
        else if (togglePlayer4.isOn)
        {
            playerNumber = 4;
        }
    }

    public int getMapNumber()
    {
        return mapNumber;
    }
    public int getPlayerNumber()
    {
        return playerNumber;
    }

    public int getGameMode()
    {
        return gameMode;
    }
}

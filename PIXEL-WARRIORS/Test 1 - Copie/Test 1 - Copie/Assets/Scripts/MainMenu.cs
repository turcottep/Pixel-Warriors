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

    private bool aiMode = false;

    public void Awake()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainMenuManager"));
    }

    public void playAI()
    {
        SelectPlayer();
        aiMode = true;

        if (toggleMap1.isOn)
        {
            toggleMap2.isOn = false;
#if UNITY_EDITOR
            EditorSceneManager.LoadScene("MAP1");
#endif
        }
        else if (toggleMap2.isOn)
        {
            toggleMap1.isOn = false;
#if UNITY_EDITOR
            EditorSceneManager.LoadScene("MAP2");
#endif
        }

    }

    public void playOnline()
    {
        SelectPlayer();

        if (playerNumber != 0)
        {
            if (toggleMap1.isOn)
            {
                mapNumber = 1;
            }
            else if (toggleMap2.isOn)
            {
                mapNumber = 2;
            }
#if UNITY_EDITOR
            EditorSceneManager.LoadScene("Launcher");
#endif
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

    public bool getAIMode()
    {
        return aiMode;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class MainMenu : MonoBehaviour
{
    public GameObject mapsMenu;
    public GameObject characterMenu;
    public GameObject errorMap;

    public Toggle toggleMap1;
    public Toggle toggleMap2;

    public bool boughtP2;
    public bool boughtP3;
    public bool boughtP4;

    public GameObject lockP2;
    public GameObject lockP3;
    public GameObject lockP4;
    public GameObject coinsDisplay;

    public float gold;

    private int priceP2 = 5000;
    private int priceP3 = 7500;
    private int priceP4 = 10000;

    public GameObject errorNotEnoughMoney;

    public Toggle togglePlayer1;
    public GameObject p1Background;
    public Toggle togglePlayer2;
    public GameObject p2Background;
    public Toggle togglePlayer3;
    public GameObject p3Background;
    public Toggle togglePlayer4;
    public GameObject p4Background;

    public Toggle soundCheckBox;
    public Toggle musicCheckBox;
    public TMP_Dropdown dropdownDifficultyAI;

    public GameObject errorCharacter;


    public int mapNumber;
    public int playerNumber = 0;

    private int gameMode = 1;
    private int aiDificulty = 0;

    private bool once;

    GameObject player1;

    public void Awake()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainMenuManager"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("SettingsMenu"));

        gold = PlayerPrefs.GetFloat("gold", 0f);
        UpdateCoinsDisplay();
        boughtP2 = PlayerPrefs.GetInt("boughtP2", 0) == 1;
        boughtP3 = PlayerPrefs.GetInt("boughtP3", 0) == 1;
        boughtP4 = PlayerPrefs.GetInt("boughtP4", 0) == 1;

        //player1 = Instantiate(Resources.Load("Ninja"), new Vector2(-2.7f, 0.9f), Quaternion.identity) as GameObject;
        //player1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainMenuManager"));

    }

    public void GoToCharactersMenu()
    {
        if (toggleMap1.isOn || toggleMap2.isOn)
        {
            mapsMenu.SetActive(false);
            characterMenu.SetActive(true);
            lockP2.SetActive(!boughtP2);
            lockP3.SetActive(!boughtP3);
            lockP4.SetActive(!boughtP4);

        }
        else
        {
            errorMap.SetActive(true);
        }

    }

    public void playAI()
    {

        if (SelectPlayer())
        {
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
        else errorCharacter.SetActive(true);
    }

    public void SelectAIDfficulty()
    {
        aiDificulty = dropdownDifficultyAI.value;
        Debug.Log("CHOOSING: " + aiDificulty);
    }

    public int getAIDifficulty()
    {
        return aiDificulty;
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

    // SHOP 
    public bool BuyP2()
    {
        Debug.Log("buying p2");
        if (!boughtP2 && gold >= priceP2)
        {
            gold -= priceP2;
            boughtP2 = true;
            PlayerPrefs.SetInt("boughtP2", 1);
            PlayerPrefs.SetFloat("gold", gold);
            lockP2.SetActive(false);
            UpdateCoinsDisplay();
            return true;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
            return false;
        }
    }

    public bool BuyP3()
    {
        Debug.Log("buying p3");
        if (!boughtP3 && gold >= priceP3)
        {
            gold -= priceP3;
            boughtP3 = true;
            PlayerPrefs.SetInt("boughtP3", 1);
            PlayerPrefs.SetFloat("gold", gold);
            lockP3.SetActive(false);
            UpdateCoinsDisplay();
            return true;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
            return false;
        }
    }

    public bool BuyP4()
    {
        Debug.Log("buying p4");
        if (!boughtP4 && gold >= priceP4)
        {
            gold -= priceP4;
            boughtP4 = true;
            PlayerPrefs.SetInt("boughtP4", 1);
            PlayerPrefs.SetFloat("gold", gold);
            lockP4.SetActive(false);
            UpdateCoinsDisplay();
            return true;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
            return false;
        }
    }

    public void unSelect1()
    {
        if (toggleMap2.isOn)
        {
            toggleMap1.isOn = false;
        }
        errorMap.SetActive(false);
    }
    public void unSelect2()
    {
        if (toggleMap1.isOn)
        {
            toggleMap2.isOn = false;
        }
        errorMap.SetActive(false);
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
            if (!boughtP2)
            {
                if (!BuyP2())
                {
                    togglePlayer2.isOn = false;
                    return;
                }
            }
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
            if (!boughtP3)
            {
                if (!BuyP3())
                {
                    togglePlayer3.isOn = false;
                    return;
                }
            }
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
            if (!boughtP4)
            {
                if (!BuyP4())
                {
                    togglePlayer4.isOn = false;
                    return;
                }
            }
            togglePlayer1.isOn = false;
            togglePlayer2.isOn = false;
            togglePlayer3.isOn = false;
        }
        UpdatePlayerBackground();
    }

    private void UpdatePlayerBackground()
    {
        errorCharacter.SetActive(false);
        p1Background.SetActive(togglePlayer1.isOn);
        p2Background.SetActive(togglePlayer2.isOn);
        p3Background.SetActive(togglePlayer3.isOn);
        p4Background.SetActive(togglePlayer4.isOn);
    }

    public bool SelectPlayer()
    {
        if (togglePlayer1.isOn)
        {
            playerNumber = 1;
            return true;
        }
        else if (togglePlayer2.isOn)
        {
            playerNumber = 2;
            return true;
        }
        else if (togglePlayer3.isOn)
        {
            playerNumber = 3;
            return true;
        }
        else if (togglePlayer4.isOn)
        {
            playerNumber = 4;
            return true;
        }
        else return false;
    }

    IEnumerator NotEnoughMoney()
    {
        errorNotEnoughMoney.SetActive(true);
        yield return new WaitForSeconds(1f);
        errorNotEnoughMoney.SetActive(false);
    }

    public void UpdateCoinsDisplay()
    {
        coinsDisplay.GetComponent<TextMeshProUGUI>().SetText(gold.ToString());
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

    public bool getMusic()
    {

        return musicCheckBox.isOn;
    }

    public bool getSound()
    {

        return soundCheckBox.isOn;
    }

}

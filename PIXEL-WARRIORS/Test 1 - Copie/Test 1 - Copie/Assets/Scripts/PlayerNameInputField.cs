using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Collections;

/// <summary>
/// Player name input field. Let the user input his name, will appear above the player in the game.
/// </summary>
[RequireComponent(typeof(TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    #region Private Variables


    // Store the PlayerPref Key to avoid typos
    static string playerNamePrefKey = "PlayerName";
    TMP_InputField _inputField;

    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {


        string defaultName = "";
        _inputField = this.GetComponent<TMP_InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                Debug.Log("Default name: " + defaultName);
                _inputField.text = defaultName;
            }
        }


        PhotonNetwork.playerName = defaultName;
    }


    #endregion


    #region Public Methods


    /// <summary>
    /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    /// </summary>
    /// <param name="value">The name of the Player</param>
    public void SetPlayerName(string value)
    {
        int elo = PlayerPrefs.GetInt("elo", 1000);
        // #Important
        Debug.Log("name = " + _inputField.text);
        PhotonNetwork.playerName = _inputField.text + " "; // force a trailing space string in case value is an empty string, else playerName would not be updated.
        PhotonNetwork.player.SetScore(elo);
        PlayerPrefs.SetString(playerNamePrefKey, _inputField.text);
    }

            
    #endregion
}



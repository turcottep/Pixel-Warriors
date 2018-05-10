using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxChange : MonoBehaviour {

    private new AudioManager audio;
    public Toggle checkBox;

    private void Start()
    {
        checkBox = GetComponent<Toggle>();
        checkBox.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(checkBox);
        });
    }

    void ToggleValueChanged (Toggle change)
    {
        if (change.isOn)
        {
            Debug.Log("REEEE");
            audio.Play("MusicMenu", 0, true);
        }
        if (!change.isOn)
        {
            Debug.Log("RE");
            audio.Stop("MusicMenu", 0, true);
        }
    }

    /*private void Start()
    {
        if (gameObject.name == "Music_Toggle")
        {
            CheckBox = gameObject.GetComponent<Toggle>();
        }
        if (gameObject.name == "Sound_Toggle")
        {
            CheckBox = gameObject.GetComponent<Toggle>();
        }

        CheckBox.onValueChanged.AddListener((value) =>
        {
            Listener(value);
        });
    }

    public void Listener(bool value)
    {
        if (value)
        {
            if (gameObject.name == "Music_Toggle")
            {
                audio.Play("MusicMenu", 0, true);
            }
            if (gameObject.name == "Sound_Toggle")
            {
                audio.soundOn = true;
            }
        }
        else
        {
            if (gameObject.name == "Music_Toggle")
            {
                audio.Stop("MusicMenu", 0, true);
            }
            if (gameObject.name == "Sound_Toggle")
            {
                audio.soundOn = false;
            }
        }
    }*/
}

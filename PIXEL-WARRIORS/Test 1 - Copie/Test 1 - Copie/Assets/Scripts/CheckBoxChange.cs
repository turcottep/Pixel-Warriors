using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxChange : MonoBehaviour {

    private new AudioManager audio;
    private Manager manager;
    //public Toggle checkBox;

    private void Start()
    {
        manager = gameObject.GetComponent<Manager>();
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audio.Play("MusicMenu", 0, true);
    }

    public void ToggleMusic(bool val)
    {
        if (val)
        {
            audio.musicOn = true;
            Debug.Log("play");
            audio.Play("MusicMenu", 0, true);
        }
        if (!val)
        {
            audio.musicOn = false;
            Debug.Log("stop");
            audio.Stop("MusicMenu", 0, true);
        }
    }

    public void ToggleSound(bool val)
    {
        if (val)
        {
            audio.soundOn = true;
            Debug.Log("playSOUND");
        }
        if (!val)
        {
            audio.soundOn = false;
            Debug.Log("stopSOUND");
        }
    }
}

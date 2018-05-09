using UnityEngine.Audio;
using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;
    public bool soundOn = true;
    public bool musicOn = true;

	void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}

    public void Play(String name, float delay, bool unmute)
    {
        if (unmute)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound : " + name + " not found!");
                return;
            }

            if (delay != 0) StartCoroutine(PlaySound(s, delay));
            else s.source.Play();
        }
    }

    IEnumerator PlaySound(Sound s, float d)
    {
        yield return new WaitForSeconds(d);
        s.source.Play();

    }

    public void Stop(String name, float delay, bool unmute)
    {
        if (unmute)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (delay != 0) StartCoroutine(StopSound(s, delay));
            else s.source.Stop();
        }
    }

    IEnumerator StopSound(Sound s, float d)
    {
        yield return new WaitForSeconds(d);
        s.source.Stop();
        
    }
}

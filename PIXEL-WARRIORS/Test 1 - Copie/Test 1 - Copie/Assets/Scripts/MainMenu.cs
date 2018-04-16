using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Toggle toggleMap1;
    public Toggle toggleMap2;

    private int mapNumber;

    public void Awake()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Manager"));
    }

    public void playAI()
    {
        if (toggleMap1.isOn)
        {
            toggleMap2.isOn = false;
            EditorSceneManager.LoadScene("MAP1");
        }
        else if (toggleMap2.isOn)
        {
            toggleMap1.isOn = false;
            EditorSceneManager.LoadScene("MAP2");
        }

    }

    public void playOnline()
    {
        if (toggleMap1.isOn)
        {
            mapNumber = 1;
        }
        else if (toggleMap2.isOn)
        {
            mapNumber = 2;
        }
        EditorSceneManager.LoadScene("Launcher");
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

    public int getMapNumber()
    {
        return mapNumber;
    }

    
}

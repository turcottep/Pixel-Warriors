using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Toggle toggleMap1;
    public Toggle toggleMap2;

    public void playButton()
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
}

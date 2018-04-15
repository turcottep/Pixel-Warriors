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
            EditorSceneManager.LoadScene("MAP1");
        }
        else if (toggleMap2.isOn)
        {
            EditorSceneManager.LoadScene("MAP2");
        }
    }
}

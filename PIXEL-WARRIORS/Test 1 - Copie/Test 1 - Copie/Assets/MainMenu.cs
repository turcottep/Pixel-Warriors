using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void playButton()
    {
        EditorSceneManager.LoadScene("Lava_Map");
    }
}

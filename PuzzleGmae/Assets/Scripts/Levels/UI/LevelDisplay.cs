using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] Text nameDisplay;
    [SerializeField] SceneLoader sceneLoader;
    TextAsset level;

    public void Initalize(TextAsset lvl)
    {
        nameDisplay.text = lvl.name;
        level = lvl;
    }

    public void PlayLevel()
    {
        PlayerPrefs.SetString("level", level.text);
        sceneLoader.LoadScene();
    }

}

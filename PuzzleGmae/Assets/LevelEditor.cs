using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{
    [SerializeField] Map map;
    LevelLayout layout;

    public int LevelWidth = 5;
    public int LevelHeight = 5;

    private void Start()
    {
        layout = new LevelLayout(LevelWidth, LevelHeight);

        UpdateSize();
    }

    public void UpdateSize()
    {
        map.BuildLevelLayout(layout);
    }

    public void ChangeWidth(float x)
    {
        LevelWidth = (int)x;
        layout.UpdateLevelWidth(LevelWidth);
        UpdateSize();
    }

    public void ChangeHeight(float y)
    {
        LevelHeight = (int)y;
        layout.UpdateLevelHeight(LevelHeight);
        UpdateSize();
    }

}

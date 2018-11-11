using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{
    [SerializeField] Map map;


    public int LevelWidth = 5;
    public int LevelHeight = 5;

    private void Start()
    {
        UpdateSize();
    }

    public void UpdateSize()
    {
        map.BuildBackground(LevelWidth, LevelHeight);
    }

    public void ChangeWidth(float x)
    {
        LevelWidth = (int)x;
        UpdateSize();
    }

    public void ChangeHeight(float y)
    {
        LevelHeight = (int)y;
        UpdateSize();
    }

}

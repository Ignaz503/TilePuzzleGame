using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{
    public event Action<BaseValueAndColorGenerator> OnNewValueAndColorGen;

    [SerializeField] Map map;
    LevelLayout layout;
    BaseValueAndColorGenerator gen;

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
        TryRecolorMoveableTiles();
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

    public void SetValueAndColorGen(string data)
    {
        gen = ValueAndColorGeneratorFactory.BuildValueAndColorGenerator(data);
        OnNewValueAndColorGen?.Invoke(gen);
    }

    public void AddBlockingTile(Vector2Int gridPos)
    {
        layout.AddBlockingTile(new BlockingTileSpawnInfo() { GridPosition = gridPos, Health = 50f });
    }

    public void UpdateHealthOfBlockingTile(Vector2Int pos, float newHealth)
    {
        layout.UpdateBlockingTileValueAT(pos, newHealth);
    }

    public void RemoveBlockingTile(Vector2Int gridPos)
    {
        layout.RemoveBlockingTileAtPosition(gridPos);
    }

    public void AddMoveableTile(Vector2Int gridPos)
    {
        layout.AddMoveableTile(new MoveableTileSpawnInfo() { GridPosition = gridPos, Value = 0});
    }

    public void UpdateMoveableTile(Vector2Int gridPos, int newVal)
    {
        layout.UpdateMoveableTileValueAt(gridPos, newVal);
    }

    public void RemoveMoveableTile(Vector2Int gridPos)
    {
        layout.RemoveMoveableTileAtPosition(gridPos);
    }

    void TryRecolorMoveableTiles()
    {
        if(gen != null)
        {
            foreach(MoveableTileSpawnInfo info in layout.MoveableTiles)
            {
                map.GetTileAt(info.GridPosition.ToVec3XY(0), Tile.GetLayerForType(Tile.Type.Movable)).ChangeColor(gen.GetColorForValue(info.Value));
            }
        }
    }

}

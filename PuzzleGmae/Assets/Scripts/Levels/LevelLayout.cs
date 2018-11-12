using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class LevelLayout
{
    public int Width { get; protected set; }
    public int Height { get; protected set; }

    public List<BlockingTileSpawnInfo> BlockingTiles { get; protected set; }
    public List<MoveableTileSpawnInfo> MoveableTiles { get; protected set; }

    [JsonConstructor]
    public LevelLayout(int width, int height, List<BlockingTileSpawnInfo> blockingTiles, List<MoveableTileSpawnInfo> moveableTiles)
    {
        Width = width;
        Height = height;
        this.BlockingTiles = blockingTiles;
        this.MoveableTiles = moveableTiles;
    }

    public LevelLayout(int width, int height)
    {
        Width = width;
        Height = height;
        BlockingTiles = new List<BlockingTileSpawnInfo>();
        MoveableTiles = new List<MoveableTileSpawnInfo>();
    }

    void AddBlockingTile(BlockingTileSpawnInfo info)
    {
        BlockingTiles.Add(info);
    }

    void AddMoveableTile(MoveableTileSpawnInfo info)
    {
        MoveableTiles.Add(info);
    }

    bool PositionAlreadyOccupied(Vector2Int pos)
    {
        foreach(BlockingTileSpawnInfo info in BlockingTiles)
        {
            if (info.GridPosition == pos)
                return true;
        }

        foreach(MoveableTileSpawnInfo info in MoveableTiles)
        {
            if (info.GridPosition == pos)
                return true;
        }
        return false;
    }

    public void RemoveBlockingTileAtPosition(Vector2Int pos)
    {
        BlockingTiles.RemoveAll(info => info.GridPosition == pos);
    }

    public void RemoveMoveableTileAtPosition(Vector2Int pos)
    {
        MoveableTiles.RemoveAll(info => info.GridPosition == pos);
    }

    public void RemoveTileAt(Vector2Int pos)
    {
        RemoveMoveableTileAtPosition(pos);
        RemoveBlockingTileAtPosition(pos);
    }

    bool UpdateMoveableTileValueAt(Vector2Int pos, int newVal)
    {
        foreach(MoveableTileSpawnInfo info in MoveableTiles)
        {
            if (pos == info.GridPosition)
            {
                info.Value = newVal;
                return false;
            }
        }
        return false;
    }

    bool UpdateBlockingTileValueAT(Vector2Int pos, float newHealth)
    {
        foreach(BlockingTileSpawnInfo info in BlockingTiles)
        {
            if(info.GridPosition == pos)
            {
                info.Health = newHealth;
                return true;
            }
        }
        return false;
    }

    public void UpdateLevelHeight(int newHeight)
    {
        Height = newHeight;

        //remove all tiles with position y greater of equal to height
        BlockingTiles.RemoveAll((t) => t.GridPosition.y >= newHeight);
        MoveableTiles.RemoveAll((t) => t.GridPosition.y >= newHeight);
    }
    
    public void UpdateLevelWidth(int newWidth)
    {
        Width = newWidth;

        BlockingTiles.RemoveAll(t => t.GridPosition.x >= newWidth);
        MoveableTiles.RemoveAll(t => t.GridPosition.x >= newWidth);
    }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static LevelLayout Deserialize(string jsonString)
    {
        return JsonConvert.DeserializeObject<LevelLayout>(jsonString);
    }
}

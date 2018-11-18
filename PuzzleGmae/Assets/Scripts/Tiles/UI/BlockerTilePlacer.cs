using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerTilePlacer : TilePlacer
{

    public void UpdateHealth(Vector2Int pos, float newHealth)
    {
        editor.UpdateHealthOfBlockingTile(pos, newHealth);
    }

    public override void CreateTile(Vector3Int pos)
    {
        //place tile and add to level layout
        //or only palce tile and build layout later? hmm
        //for now add to layout

        map.CreateBlockerTileAt(pos.ToVec2IntXY());

        //should probably do that on save
        //get all blocker tiles from map and add to layout
        //with health
        editor.AddBlockingTile(pos.ToVec2IntXY());
    }

    public override bool CheckIfPlaceable(Vector3Int pos)
    {
        return !map.HasTile(pos, Tile.GetLayerForType(Tile.Type.Blocking));
    }

    public override bool CheckIfPositionContainsInspectableTile(Vector3Int gridPos)
    {
        return map.HasTile(gridPos, Tile.GetLayerForType(Tile.Type.Blocking));
    }

    public override Tile GetTileToInspect(Vector3Int gridPos)
    {
        return map.GetTileAt(gridPos, Tile.GetLayerForType(Tile.Type.Blocking));
    }

    public override void UpdateValue(Vector2Int gridPos, float value)
    {
        UpdateHealth(gridPos, value);
    }

    public override void Remove(Tile t)
    {
        Debug.Log("Remove tile");
        editor.RemoveBlockingTile(t.GridPosition.ToVec2IntXY());
        base.Remove(t);
    }
}




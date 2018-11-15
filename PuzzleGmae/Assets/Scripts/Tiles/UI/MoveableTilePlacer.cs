using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableTilePlacer : TilePlacer
{
    public override bool CheckIfPlaceable(Vector3Int pos)
    {
        return !map.HasTile(pos, Tile.GetLayerForType(Tile.Type.Movable)) && !map.HasTile(pos, Tile.GetLayerForType(Tile.Type.Blocking));
    }

    public override bool CheckIfPositionContainsInspectableTile(Vector3Int gridPos)
    {
        return map.HasTile(gridPos, Tile.GetLayerForType(Tile.Type.Movable));
    }

    public override void CreateTile(Vector3Int pos)
    {
        map.CreateMoveableTileAt(pos.ToVec2IntXY());
        editor.AddMoveableTile(pos.ToVec2IntXY());
    }

    public override Tile GetTileToInspect(Vector3Int gridPos)
    {
        return map.GetTileAt(gridPos, Tile.GetLayerForType(Tile.Type.Movable));
    }

    public override void UpdateValue(Vector2Int gridPos, float value)
    {
        //TODO: change color of  tile when the pallet was chosen and value
        editor.UpdateMoveableTile(gridPos, (int)value);
    }

    public override void Remove(Tile t)
    {
        base.Remove(t);
        editor.RemoveMoveableTile(t.MapPosition);
    }
}

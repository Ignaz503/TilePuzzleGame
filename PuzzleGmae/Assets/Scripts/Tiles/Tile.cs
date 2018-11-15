using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public enum Type
    {
        Background = 0,
        Blocking = 1,
        Movable = 2
    }

    protected event Action<Tile> OnDeath;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    public Color TileColor { get { return spriteRenderer.color; } }
    protected Map map;
    public Map Map { get { return map; } }
    [SerializeField] protected Type type;
    public Type TileType { get { return type; } }
    public int Layer { get { return (int)TileType; } }

    [HideInInspector]public Vector3Int LayeredGridPosition;
    [HideInInspector] public Vector3Int GridPosition;
    [HideInInspector] public Vector2Int MapPosition { get { return new Vector2Int(GridPosition.x, GridPosition.y); } }

    public virtual void Initialize(Vector3Int position, Map map)
    {
        this.map = map;
        LayeredGridPosition = position;
        GridPosition = position;
        LayeredGridPosition.z = -(int)type;
        spriteRenderer.sortingOrder = Mathf.Abs((int)type);
    }

    public virtual void KillTile()
    {
        //does not remove it from map tiles
        Destroy(gameObject);
        OnDeath?.Invoke(this);
        Map.RemoveTile(LayeredGridPosition);
    }

    public void PlaceInWorld()
    {
        //placed infront of everything else
        Vector3 pos = map.GridPositionToWorld(LayeredGridPosition);
        transform.position = pos;
    }

    public void ChangeColor(Color c)
    {
        spriteRenderer.color = c;
    }

    public static Vector3Int GetLayerGridPosition(Vector3Int nonLayerdGridPos, Type t)
    {
        return new Vector3Int(nonLayerdGridPos.x, nonLayerdGridPos.y, -(int)t);
    }

    public static int GetLayerForType(Type t)
    {
        return (int)t;
    }

    protected void InvokeOnDeath()
    {
        OnDeath?.Invoke(this);
    }

    public void RegisterToOnDeath(Action<Tile> callBack)
    {
        OnDeath += callBack;
    }

    public void UnregisterFromOnDeath(Action<Tile> callback)
    {
        OnDeath -= callback;
    }
}

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

public class Map : MonoBehaviour
{
    protected event Action<Tile> OnTileCreated;

    [SerializeField] GameObject backGroundTilePrefab;
    [SerializeField] GameObject moveableTilePrefab;
    [SerializeField] GameObject blockingTilePrefab;
    [SerializeField][Range(0f,1f)] float camOrthographicSizeMultiplier = .6f;

    int numberOfLayers;

    [SerializeField] Vector3 tileSize;
    [SerializeField] Vector3 tileAnchor;

    [SerializeField] Bounds bounds;
    public Bounds FieldBounds { get { return bounds; } }

    [SerializeField] Camera cam;

    Dictionary<Vector3Int, Tile> Tiles;

    public Level Level { get; protected set; }

    private void Awake()
    {
        Tiles = new Dictionary<Vector3Int, Tile>();

        //find out number of layers
        numberOfLayers = (int)Enum.GetValues(typeof(Tile.Type)).Cast<Tile.Type>().Max() + 1;
    }

    public MoveableTile CreateMoveableTileAt(Vector2Int pos)
    {
        //TODO check if in bounds probably;
        GameObject obj = Instantiate(moveableTilePrefab);
        MoveableTile t = obj.GetComponent<MoveableTile>();
        t.Initialize(new Vector3Int(pos.x, pos.y, 0), this);
        t.name = t.GetType().ToString() +": " + t.LayeredGridPosition.ToString() ;
        t.PlaceInWorld();
        t.transform.SetParent(transform);

        Tiles.Add(t.LayeredGridPosition, t);

        OnTileCreated?.Invoke(t);
        return t;
    }

    public Tile GetTileAt(Vector3Int gridPosition)
    {
        if (Tiles.ContainsKey(gridPosition))
        {
            return Tiles[gridPosition];
        }
        return null;
    }

    public Tile GetTileFromWorldPos(Vector3 worldPos)
    {
        int x = (int)((worldPos.x / tileSize.x));
        int y = (int)((worldPos.y / tileSize.y));

        return GetTileFromAnyLayer(new Vector3Int(x, y, 0));
    }

    public Tile GetTileFromAnyLayer(Vector3Int gridPosWithoutLayer)
    {
        for (int layer = numberOfLayers -1 ; layer >= 0; layer--)
        {

            Tile t = GetTileAt(gridPosWithoutLayer, layer);
            if (t != null)//test every layer before return null or return first found
                return t;
        }
        return null;
    }

    public Tile GetTileAt(Vector3Int gridPosWithoutLayer, int layer)
    {
        gridPosWithoutLayer.z = layer < 0 ? layer : -layer;
        return GetTileAt(gridPosWithoutLayer);
    }

    public Tile GetTileFromLayer(Vector3 worldPos, int layer)
    {
        Vector3Int pos = WorldToGridWithoutLayer(worldPos);
        pos.z = layer < 0 ? layer : -layer;
        return GetTileAt(pos);
    }

    public Vector3Int WorldToGridWithoutLayer(Vector3 worldPos)
    {
        int x = (int)((worldPos.x / tileSize.x));
        int y = (int)((worldPos.y / tileSize.y));

        return new Vector3Int(x, y, 0);
    }

    void BuildLevel()
    {
        BuildBackground();

        BuildMoveableTiles();

        BuildBlockingTiles();
    }    

    void BuildBackground()
    {
        for (int x = 0; x < bounds.extents.x; x++)
        {
            for (int y = 0; y < bounds.extents.y; y++)
            {
                GameObject obj = Instantiate(backGroundTilePrefab);
                Tile t = obj.GetComponent<Tile>();
                t.Initialize(new Vector3Int(x, y, 0), this);
                t.name = t.GetType().ToString() + ": " + t.LayeredGridPosition.ToString();
                t.PlaceInWorld();
                t.transform.SetParent(transform);
                //OnTileCreated?.Invoke(t);
                Tiles.Add(t.LayeredGridPosition, t);
            }
        }
    }

    void BuildMoveableTiles()
    {
        BuildMoveableTiles(Level.GetMoveableTileLayout());
    }

    void BuildMoveableTiles(List<MoveableTileSpawnInfo> layout)
    {
        //level moveable tiles
        foreach (MoveableTileSpawnInfo info in layout)
        {
            MoveableTile t = CreateMoveableTileAt(info.GridPosition);
            t.SetValue(info.Value);
        }
    }

    void BuildBlockingTiles()
    {
        BuildBlockingTiles(Level.GetBlockerTileLayout());
    }

    void BuildBlockingTiles(List<BlockingTileSpawnInfo> layout)
    {
        //blocker tiles
        foreach (BlockingTileSpawnInfo info in layout)
        {

            BlockingTile t = CreateBlockerTileAt(info.GridPosition);
            t.SetHealth(info.Health);
        }
    }

    public BlockingTile CreateBlockerTileAt(Vector2Int pos)
    {
        //TODO check if in bounds probably;
        GameObject obj = Instantiate(blockingTilePrefab);
        BlockingTile t = obj.GetComponent<BlockingTile>();
        t.Initialize(new Vector3Int(pos.x, pos.y, 0), this);
        t.name = t.GetType().ToString() + ": " + t.LayeredGridPosition.ToString();
        t.PlaceInWorld();
        t.transform.SetParent(transform);

        Tiles.Add(t.LayeredGridPosition, t);
        OnTileCreated?.Invoke(t);
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 ext = bounds.extents;
        ext.Scale(tileSize * 2) ;
        //ext += tilemap.cellSize * 2;
        Gizmos.DrawWireCube(bounds.center, ext);
    }

    public Vector3 GridPositionToWorld(Vector3Int pos)
    {
        Vector3 position = pos;
        position.Scale(tileSize);
        return position + tileAnchor;
    }

    public bool HasTile(Vector3Int layeredGridPosition)
    {
        return Tiles.ContainsKey(layeredGridPosition);
    }

    public bool HasTile(Vector3Int gridPos, int layer)
    {
        gridPos.z = layer < 0 ? layer : -layer;
        return HasTile(gridPos);
    }

    public bool HasTileOnAnyLayer(Vector3Int gridPos)
    {
        for(int i = numberOfLayers - 1; i >= 0; i--)
        {
            bool hasTile = HasTile(gridPos, i);
            if (hasTile)
                return hasTile;
        }
        return false;
    }

    public bool HasTileOnAnyLayer(Vector3 worldPos)
    {
        Vector3Int gridPos = WorldToGridWithoutLayer(worldPos);
        return HasTileOnAnyLayer(gridPos);
    }

    public bool HasTile(Vector3 worldPos, int layer)
    {
        Vector3 gridPos = WorldToGridWithoutLayer(worldPos);
        return HasTile(gridPos, layer);
    }

    public bool RemoveTile(Vector3Int layerdGridPosition)
    {
        bool suc = Tiles.Remove(layerdGridPosition);
        return suc;
    }

    public bool Add(Vector3Int pos, Tile t)
    {
        if (!Tiles.ContainsKey(pos))
        {
            Tiles.Add(pos, t);
            return true;
        }
        return false;
    }

    public int GetTilesCountOnLayer(int layer)
    {
        int count = 0;
        foreach (Tile t in Tiles.Values)
        {
            if (t.Layer == layer)
                count++;
        }
        return count;
    }

    public void Initialize(Level lvl)
    {
        Level = lvl;

        bounds.extents = new Vector3(lvl.Width, lvl.Height, 0);
        bounds.center = new Vector3(lvl.Width / 2.0f, lvl.Height / 2.0f, 0);

        BuildLevel();

        SetUpCamera(lvl.Width, lvl.Height);

        bounds.extents = bounds.extents * .5f;
    }

    public void BuildBackground(int width, int height)
    {
        ClearTiles();

        bounds.extents = new Vector3(width, height, 0);
        bounds.center = new Vector3(width / 2.0f, height/2.0f, 0);

        BuildBackground();

        SetUpCamera(width, height);

        bounds.extents = bounds.extents * .5f;
    }

    void SetUpCamera(float width, float height)
    {
        cam.transform.position = bounds.center + (Vector3.back * 10);

        //float avg = (height * camOrthographicHeightMultiplier + width * camOrthographicWidthMultiplier) / 2;
        //cam.orthographicSize = avg;
        cam.orthographicSize = (width > height ? width : height) * camOrthographicSizeMultiplier;
    }

    void ClearTiles()
    {
        Dictionary<Vector3Int, Tile> oldTiles = Tiles;
        Tiles = new Dictionary<Vector3Int, Tile>();
        foreach(Tile t in oldTiles.Values)
        {
            t.KillTile();
        }
    }

    public void Restart(Level level)
    {
        ClearTiles();
        Initialize(level);
    }

    public void BuildLevelLayout(LevelLayout layout)
    {
        BuildBackground(layout.Width, layout.Height);

        BuildBlockingTiles(layout.BlockingTiles);
        BuildMoveableTiles(layout.MoveableTiles);
    }

    public bool CheckPositionMoveable(Vector3Int unlayerGridPosition)
    {
        Vector3Int layeredPosMoveAbleTiles = unlayerGridPosition;
        layeredPosMoveAbleTiles.z = -Tile.GetLayerForType(Tile.Type.Movable);
        Vector3Int layeredPosBlockingTiles = unlayerGridPosition;
        layeredPosBlockingTiles.z = -Tile.GetLayerForType(Tile.Type.Blocking);
        return !HasTile(layeredPosMoveAbleTiles) && !HasTile(layeredPosBlockingTiles);
    }

    public List<Tile> GetTilesInLayer(int layer)
    {
        List<Tile> tilesInLayer = new List<Tile>();
        
        foreach(Tile t in Tiles.Values)
        {
            if (t.Layer == layer)
                tilesInLayer.Add(t);
        }

        return tilesInLayer;
    }

    public void RegisterToOnTileCreated(Action<Tile> callback)
    {
        OnTileCreated += callback;
    }

    public void UnregisterFromOnTileCreated(Action<Tile> callback)
    {
        OnTileCreated -= callback;
    }

    public Color GetColorForValue(int value)
    {
        if (Level != null)
            return Level.GetColorForValue(value);
        return Color.white;
    }

    public int ClampValueIntoAccaptableRange(int val)
    {
        if (Level != null)
            return Level.ClampValueIntoAccaptableRange(val);
        return val;
    }

    public void ActivateMergeEffect(MoveableTile moved, MoveableTile mergedInto)
    {
        Level.ActivateMergeEffect(moved, mergedInto, this);
    }

    public void ToggleValueDisplayForTiles()
    {
        foreach(Tile t in GetTilesInLayer(Tile.GetLayerForType(Tile.Type.Movable)))
        {
            MoveableTile tM = t as MoveableTile;

            tM.ToggleValueDisplay();
        }
    }

    public int GetNewValue(int value, Direction movedDirection)
    {
        if (Level != null)
            return Level.GetNewValue(value, movedDirection);
        return 0;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TilePlacer : MonoBehaviour
{
    [SerializeField] protected LevelEditor editor;
    public LevelEditor Editor { get { return editor; } }
    [SerializeField] protected Camera cam;
    [SerializeField] protected Map map;
    [SerializeField] protected TileInspector inspector;
    [SerializeField] protected KeyCode placeButton = KeyCode.Mouse0;
    [SerializeField] protected KeyCode inspectButton = KeyCode.Mouse1;

    bool isInInspectMode = false;

    private void Update()
    {
        HandleTilePlacement();
        HandleSelection();
    }

    void HandleTilePlacement()
    {
        if (isInInspectMode)
            return;
        Vector3 mWorlPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mWorlPos.z = 0;
        if (Input.GetKeyDown(placeButton) && map.FieldBounds.Contains(mWorlPos))
        {
            Vector3Int gridPos = GetGridPosition();
            if (CheckIfPlaceable(gridPos))
            {
                CreateTile(gridPos);
            }
        }
    }

    void HandleSelection()
    {
        if (Input.GetKeyDown(inspectButton))
        {
            Vector3Int gPos = GetGridPosition();
            if (CheckIfPositionContainsInspectableTile(gPos))
            {
                // enable inspector and set value editing
                inspector.EnableInspector(GetTileToInspect(gPos));
                isInInspectMode = true;
            }
        }
    }

    Vector3Int GetGridPosition()
    {
        return map.WorldToGridWithoutLayer(cam.ScreenToWorldPoint(Input.mousePosition));

    }

    public void InformOfEndInspect()
    {
        isInInspectMode = false;
    }

    public abstract void CreateTile(Vector3Int pos);
    public abstract bool CheckIfPlaceable(Vector3Int pos);
    public abstract bool CheckIfPositionContainsInspectableTile(Vector3Int gridPos);
    public abstract Tile GetTileToInspect(Vector3Int gridPos);
    public abstract void UpdateValue(Vector2Int gridPos, float value);

    public virtual void Remove(Tile t)
    {
        t.KillTile();

        isInInspectMode = false;
    }
}
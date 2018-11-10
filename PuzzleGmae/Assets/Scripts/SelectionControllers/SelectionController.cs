using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionController : MonoBehaviour
{
    [SerializeField]protected Map map;
    [SerializeField] protected Camera playerCam;

    protected Tile selectedTile;

    protected event Action<Tile> OnTileSelected;
    protected event Action<Tile> OnTileDeselected;
    
    public void RegsiterToOnTileSelected(Action<Tile> callback)
    {
        OnTileSelected += callback;
    }

    public void UnregisterFromOnTileSelected(Action<Tile> callback)
    {
        OnTileSelected -= callback;
    }

    public void RegisterToOnTileDeselected(Action<Tile> callback)
    {
        OnTileDeselected += callback;
    }

    public void UnregisterFromOnTileDeselected(Action<Tile> callback)
    {
        OnTileDeselected -= callback;
    }

    protected void InvokeOnTileSelected()
    {
        OnTileSelected.Invoke(selectedTile);
    }

    protected void InvokeOnTileDeselected()
    {
        OnTileDeselected?.Invoke(selectedTile);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveableTileSelectionController : SelectionController
{
    [SerializeField] KeyCode selectionKey = KeyCode.Mouse0;
    [SerializeField] EventSystem EventSystem;

    public Tile SelectedTile
    {
        get { return selectedTile; }
        protected set
        {
            if (selectedTile != null)
            {
                (selectedTile as MoveableTile).UnregisterFromOnTileEndMove(DeselectTile);
                InvokeOnTileDeselected();
            }
            selectedTile = value;
            if (selectedTile != null)
                InvokeOnTileSelected();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(selectionKey) && !EventSystem.IsPointerOverGameObject())
        {
            //get tile
            Vector3 pos = playerCam.ScreenToWorldPoint(Input.mousePosition);
            MoveableTile t = map.GetTileFromLayer(map.WorldToGridWithoutLayer(pos),Tile.GetLayerForType(Tile.Type.Movable)) as MoveableTile;

            //make sure to deselect on end move
            if(t != null)
            {
                t.RegisterToOnTileEndMove(DeselectTile);
            }

            SelectedTile = t;// set even if null
        }
    }

    void DeselectTile(Tile t)
    {
        if(t == selectedTile)
        {
            SelectedTile = null;
        }
    }

    void MoveSelectedTile(Direction dir)
    {
        if (SelectedTile is MoveableTile)
        {
            MoveableTile t = selectedTile as MoveableTile;
            t.Move(dir);
            //deselect after single move
            SelectedTile = null;
        }
    }

    public void MoveSelectedTileUp()
    {
        MoveSelectedTile(Direction.Up);
    }

    public void MoveSelectedTileDown()
    {
        MoveSelectedTile(Direction.Down);
    }

    public void MoveSelectedTileLeft()
    {
        MoveSelectedTile(Direction.Left);
    }

    public void MoveSelectedTileRight()
    {
        MoveSelectedTile(Direction.Right);
    }

}

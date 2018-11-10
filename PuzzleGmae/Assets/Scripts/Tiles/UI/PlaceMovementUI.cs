using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceMovementUI : MonoBehaviour
{
    [SerializeField] SelectionController selectionController;

    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    private void Awake()
    {
        selectionController.RegsiterToOnTileSelected(TileSelected);
        selectionController.RegisterToOnTileDeselected(TileUnselected);

        gameObject.SetActive(false);
    }

    bool CheckIfButtonInBounds(Button btn, Bounds bs)
    {
        Vector3 pos = btn.transform.position;
        return bs.Contains(btn.transform.position);
    }

    void SetUpButtons(MoveableTile t, Bounds bounds)
    {
        upButton.gameObject.SetActive(CheckIfButtonInBounds(upButton, bounds) && t.CheckIfMoveable(Direction.Up));
        downButton.gameObject.SetActive(CheckIfButtonInBounds(downButton, bounds) && t.CheckIfMoveable(Direction.Down));
        leftButton.gameObject.SetActive(CheckIfButtonInBounds(leftButton, bounds) && t.CheckIfMoveable(Direction.Left));
        rightButton.gameObject.SetActive(CheckIfButtonInBounds(rightButton, bounds) && t.CheckIfMoveable(Direction.Right));
    }

    void SetButtonToDefault()
    {
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    void TileSelected(Tile tile)
    {
        if (tile is MoveableTile)
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(tile.LayeredGridPosition.x, tile.LayeredGridPosition.y, 0f);//TODO probably -,5 on x and y
            SetUpButtons(tile as MoveableTile, tile.Map.FieldBounds);
        }
    }

    void TileUnselected(Tile tile)
    {
        gameObject.SetActive(false);
        SetButtonToDefault();
    }
}

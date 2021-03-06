﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveableTile : Tile
{
    protected event Action<MoveableTile> OnTileStartMove;
    protected event Action<MoveableTile> OnTileEndMove;
    protected event Action<MoveableTile, MoveableTile> OnMerged;

    [SerializeField] TextMeshProUGUI valueDisplay;
    [SerializeField] Animator Animator;
    [SerializeField] AudioSource slideAudioSrc;
    [SerializeField] AudioSource mergeAudioSrc;
    [SerializeField] float speed = 1f;

    int value;
    public int Value {
        get
        {
            return value;
        }
        protected set
        {
            this.value = value;
            ChangeColor(map.GetColorForValue(this.value));
            DisplayValue();
        }
    }
        
    public void SetValue(int value)
    {
        Value = map.ClampValueIntoAccaptableRange(value);
    }

    public void Move(Direction dir)
    {
        //check if moveable
        if (!CheckIfMoveable(dir))
            return;

        OnTileStartMove?.Invoke(this);
        slideAudioSrc.Play();
        //remove from tiles
        Map.RemoveTile(LayeredGridPosition);

        StartCoroutine(MoveCoroutine(dir));
        ////if new pos has tile -> die
        //if (Map.HasTile(LayeredGridPosition + dir))
        //{
        //    MoveableTile mergedInto = map.GetTileAt(LayeredGridPosition + dir) as MoveableTile;
        //    //OnMerge 
        //    OnMerged?.Invoke(this, mergedInto);
        //    //movement ended
        //    OnTileEndMove?.Invoke(this);
        //    //"merged" with existing tile
        //    ActivateMergeEffectSelfMoving(mergedInto);
        //    //Destroy(gameObject);
        //    //InvokeOnDeath();
        //    KillTile();

        //    return;
        //}

        ////else change value
        //Value = map.Level.GetNewValue(value, dir);
        ////set position and add to tiles
        //LayeredGridPosition += dir;
        //GridPosition += dir;
        //PlaceInWorld();
        ////rejoin tiles in map
        //Map.Add(LayeredGridPosition, this);
        //OnTileEndMove?.Invoke(this);
    }

    public Color GetNextColor(Direction dir)
    {
        if (!Map.HasTile(LayeredGridPosition + dir))
        {
            return map.GetColorForValue(map.GetNewValue(value, dir));
        }
        else
            return Map.GetTileAt(LayeredGridPosition + dir).TileColor;
    }

    void EndMove(Direction dir)
    {
        //else change value
        Value = map.GetNewValue(value, dir);
        //set position and add to tiles
        LayeredGridPosition += dir;
        GridPosition += dir;
        PlaceInWorld();
        //rejoin tiles in map
        Map.Add(LayeredGridPosition, this);
        OnTileEndMove?.Invoke(this);
    }

    void Merge(Direction dir)
    {
        MoveableTile mergedInto = map.GetTileAt(LayeredGridPosition + dir) as MoveableTile;
        //OnMerge 
        mergedInto.PlayMergeAnimation();
        mergedInto.mergeAudioSrc.Play();
        OnMerged?.Invoke(this, mergedInto);
        //movement ended
        OnTileEndMove?.Invoke(this);
        //"merged" with existing tile
        ActivateMergeEffectSelfMoving(mergedInto);
        //Destroy(gameObject);
        //InvokeOnDeath();
        KillTile();
    }

    public bool CheckIfMoveable(Direction dir)
    {
        if (map.CheckPositionMoveable(GridPosition + dir))
            return true;
        else if (Map.HasTile(LayeredGridPosition + dir))
        {
            return Map.Level.CheckIfTilesCanMerge(this, Map.GetTileAt(LayeredGridPosition + dir) as MoveableTile, dir);

            //Debug.Log(Map.GetTileAt(LayeredGridPosition + dir));
            //return (Map.GetTileAt(LayeredGridPosition + dir) as MoveableTile).Value == Map.Level.GetNewValue(value, dir);
        }
        return false;
    }

    public override void Initialize(Vector3Int position, Map map)
    {
        base.Initialize(position, map);
        Value = 0;
    }

    void DisplayValue()
    {
        valueDisplay.text = Value.ToString();
    }

    public void RegisterToOnTileStartMove(Action<MoveableTile> callback)
    {
        OnTileStartMove += callback;
    }

    public void UnregisterFromOnTileStartMove(Action<MoveableTile> callback)
    {
        OnTileStartMove -= callback;
    }

    public void RegisterToOnTileEndMove(Action<MoveableTile> callback)
    {
        OnTileEndMove += callback;
    }

    public void UnregisterFromOnTileEndMove(Action<MoveableTile> callback)
    {
        OnTileEndMove -= callback;
    }

    public void RegisterToOnMerge(Action<MoveableTile,MoveableTile> callback)
    {
        OnMerged += callback;
    }

    public void UnregisterFromOnMerge(Action<MoveableTile,MoveableTile> callback)
    {
        OnMerged -= callback;
    }

    public void ActivateMergeEffectSelfMoving(MoveableTile mergedInto)
    {
        Map.ActivateMergeEffect(this, mergedInto);
    }

    public void SetActiveValueDisplay(bool value)
    {
        valueDisplay.gameObject.SetActive(value);
    }

    public void ToggleValueDisplay()
    {
        valueDisplay.gameObject.SetActive(!valueDisplay.gameObject.activeSelf);
    }

    IEnumerator MoveCoroutine(Direction dir)
    {
        Vector3 endPos = Map.GridPositionToWorld(GridPosition + dir);
        Vector3 worldPos = Map.GridPositionToWorld(GridPosition);
        Color current = TileColor;
        Color next;
        if (!Map.HasTile(LayeredGridPosition + dir))
        {
           next = Map.GetColorForValue(map.Level.GetNewValue(value, dir));
        }
        else
        {
            next = map.GetTileAt(LayeredGridPosition + dir).TileColor;
        }
        float t = 0f;

        while(endPos != transform.position)
        {
            t += Time.deltaTime*speed;
            transform.position = Vector3.Lerp(worldPos, endPos, t);
            ChangeColor(Color.Lerp(current, next, t));
            yield return null;
        }

        if (Map.HasTile(LayeredGridPosition + dir))
        {
            Merge(dir);
        }
        else
        {
            EndMove(dir);
        }
    }

    void PlayMergeAnimation()
    {
        Debug.Log("Playing animation");
        Animator.Play("MergeAnimation");
    }
}


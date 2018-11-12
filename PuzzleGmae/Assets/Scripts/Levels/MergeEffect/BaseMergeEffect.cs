using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMergeEffect
{
    public BaseMergeEffect(string data)
    {}

    public abstract void OnTilesMerged(MoveableTile movedTile, MoveableTile mergedInto, Map map);
    public abstract string Serialize(); 
}

public static class MergeEffectFactory
{
    public static BaseMergeEffect BuildMergeEffect(Type t, string data)
    {
        return Activator.CreateInstance(t, new object[] { data }) as BaseMergeEffect;
    }

    public static BaseMergeEffect BuildMergeEffect(string data)
    {
        string[] split = data.Split('\n');
        return BuildMergeEffect(Type.GetType(split[0].Trim()), data);
    }
}

public class SpawnBlockerMergeEffect : BaseMergeEffect
{
    public SpawnBlockerMergeEffect() : base("")
    {}

    public SpawnBlockerMergeEffect(string data) : base(data)
    {}

    public override void OnTilesMerged(MoveableTile movedTile, MoveableTile mergedInto, Map map)
    {
        //moved tile not in tiles

        mergedInto.KillTile();
        map.CreateBlockerTileAt(mergedInto.MapPosition);
    }

    public override string Serialize()
    {
        return GetType().ToString();
    }
}

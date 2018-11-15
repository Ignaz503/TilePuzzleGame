using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TestLevel : Level
{
    public TestLevel(int width, int height) : base(
        new LevelLayout(width, height,
        new List<BlockingTileSpawnInfo>()
        {
            new BlockingTileSpawnInfo(){ GridPosition = Vector2Int.one, Health = 50f},
            new BlockingTileSpawnInfo(){ GridPosition =new Vector2Int(1,2), Health = 50f},
            new BlockingTileSpawnInfo(){ GridPosition =new Vector2Int(2,2), Health = 50f},
            new BlockingTileSpawnInfo(){ GridPosition =new Vector2Int(3,4), Health = 50f},
            new BlockingTileSpawnInfo(){ GridPosition = new Vector2Int(3,3), Health = 50f}
        },
        new List<MoveableTileSpawnInfo>()
        {
            new MoveableTileSpawnInfo(){ GridPosition = Vector2Int.zero, Value = 2 },
            new MoveableTileSpawnInfo(){GridPosition = Vector2Int.up,Value =2},
            new MoveableTileSpawnInfo(){GridPosition = Vector2Int.right,Value =1},
            new MoveableTileSpawnInfo(){GridPosition = new Vector2Int(3,2),Value =2},
            new MoveableTileSpawnInfo(){GridPosition = new Vector2Int(4,4),Value =3},
            new MoveableTileSpawnInfo(){GridPosition = new Vector2Int(2,4),Value =0}
        }),
        new DefaultValueAndColorGenerator(new ColorPickerValueAsIndex(new Color[] { Color.red, Color.green, Color.blue, Color.yellow }),new DirectionMapping(-2,1,2,-1)),
        ConditionFactory.BuildCondition(typeof(NumberOfMoveableTilesLeftWinCondition), $"{typeof(NumberOfMoveableTilesLeftWinCondition)}\n1"),
        new NumberOfMovesLoseCondition(5),
        new DifferenceOfNMergeRule(1),
        new SpawnBlockerMergeEffect())
    {}
}


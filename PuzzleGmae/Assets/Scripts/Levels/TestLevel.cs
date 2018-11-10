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
        new DefaultValueAndColorGenerator(new ColorPickerValueAsIndex(new Color[] { Color.red, Color.green, Color.blue, Color.yellow })),
        ConditionFactory.BuildCondition(typeof(NumberOfMoveableTilesLeftWindCondition), $"{typeof(NumberOfMoveableTilesLeftWindCondition)}\n1"),
        new NumberOfMovesLoseCondition(5),
        new SameValueMergeRule(""))
    {}

    //public override int ClampValueIntoAccaptableRange(int value)
    //{
    //    return ClampValue(value);
    //}

    //public override int GetNewValue(int value, Direction movedDirection)
    //{
    //    return ClampValue(value + GetValueChangeForDirection(movedDirection));
    //}

    //int ClampValue(int value)
    //{
    //    value = value < 0 ? valueAndColorGenerator.Count + value : value;
    //    value = value >= valueAndColorGenerator.Count ? value - valueAndColorGenerator.Count : value;
    //    return value;
    //}

    //int GetValueChangeForDirection(Direction direction)
    //{
    //    switch (direction)
    //    {
    //        case Direction.Up:
    //            return -1;
    //        case Direction.Down:
    //            return 2;
    //        case Direction.Right:
    //            return 1;
    //        case Direction.Left:
    //            return -2;
    //        default:
    //            return 0;
    //    }
    //}
}


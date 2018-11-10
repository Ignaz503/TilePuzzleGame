using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameState
{
    public Map Map { get; }
    public int MovesMade { get; }
    public int MergedTiles { get; }
    //and so on

    public GameState(Map map, int movesMade, int mergedTiles)
    {
        Map = map;
        MovesMade = movesMade;
        MergedTiles = mergedTiles;
    }
}

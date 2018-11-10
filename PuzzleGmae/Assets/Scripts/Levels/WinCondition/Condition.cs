using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public abstract class Condition
{
    /// <summary>
    /// used to force inherited class to implement ctor of this kind
    /// </summary>
    /// <param name="data">data inheritors can use to build</param>
    public Condition(string data)
    {    }

    public Condition()
    {}

    public abstract bool CheckIfConditionAchived(GameState stateOfGame);

    public abstract string Serialize();
}

[Serializable]
public class NumberOfMoveableTilesLeftWindCondition : Condition
{
    [SerializeField]int numTilesLeft;

    public NumberOfMoveableTilesLeftWindCondition(string data) : base(data)
    {
        string[] split = data.Split('\n');

        if (split.Length < 2)
            throw new System.Exception($"Can't build {GetType()} from this data: {data}");

        numTilesLeft = int.Parse(split[1]);
    }

    public NumberOfMoveableTilesLeftWindCondition(int numberTilesLeft)
    {
        numTilesLeft = numberTilesLeft;
    }

    public override bool CheckIfConditionAchived(GameState stateOfGame)
    {
        return stateOfGame.Map.GetTilesCountOnLayer(Tile.GetLayerForType(Tile.Type.Movable)) <= numTilesLeft;
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + numTilesLeft;
    }
}

public class NumberOfMovesLoseCondition : Condition
{
    int numberOfMoves;

    public NumberOfMovesLoseCondition()
    {}

    public NumberOfMovesLoseCondition(int num)
    {
        numberOfMoves = num;
    }

    public NumberOfMovesLoseCondition(string data) : base(data)
    {
        string[] split = data.Split('\n');

        if (split.Length < 2)
            throw new System.Exception($"Can't build {GetType()} from this data: {data}");

        numberOfMoves = int.Parse(split[1]);
    }

    public override bool CheckIfConditionAchived(GameState stateOfGame)
    {
        return stateOfGame.MovesMade >= numberOfMoves;
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + numberOfMoves.ToString();
    }
}
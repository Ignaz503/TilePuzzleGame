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
    {}

    public Condition()
    {}

    public abstract string GetInstanceDescription();

    public abstract bool CheckIfConditionAchived(GameState stateOfGame);

    public abstract string Serialize();
}

[Serializable][WinCondition][Description("One wins when there are only N number of moveable tiles left on the field")]
public class NumberOfMoveableTilesLeftWinCondition : Condition
{
    [SerializeField,ParserType(ParserTypeAttribute.ParseableTypes.INTEGER,"Number Of Tiles Left For Victory")]protected int numTilesLeft;

    public NumberOfMoveableTilesLeftWinCondition(string data) : base(data)
    {
        string[] split = data.Split('\n');

        if (split.Length < 2)
            throw new System.Exception($"Can't build {GetType()} from this data: {data}");

        numTilesLeft = int.Parse(split[1]);
    }

    public NumberOfMoveableTilesLeftWinCondition(int numberTilesLeft)
    {
        numTilesLeft = numberTilesLeft;
    }

    public override bool CheckIfConditionAchived(GameState stateOfGame)
    {
        return stateOfGame.Map.GetTilesCountOnLayer(Tile.GetLayerForType(Tile.Type.Movable)) <= numTilesLeft;
    }

    public override string GetInstanceDescription()
    {
        if (numTilesLeft > 1)
        {
            return $"One wins when there are only {numTilesLeft} tiles left on the field";
        }
        else
            return $"One wins when there is only 1 tile left on the field";
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + numTilesLeft;
    }
}

[LoseCondtion][Description("One loses after the number of steps is reached")]
public class NumberOfMovesLoseCondition : Condition
{
    [ParserType(ParserTypeAttribute.ParseableTypes.INTEGER,"Number Of Possible Moves")]int numberOfMoves;

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
        return stateOfGame.MovesMade > numberOfMoves;
    }

    public override string GetInstanceDescription()
    {
        return $"One loses after {numberOfMoves} moves";
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + numberOfMoves.ToString();
    }
}
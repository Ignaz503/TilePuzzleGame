using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Level
{
    protected static string dataSeperator = "|";

    public int Width { get { return levelLayout.Width; } }
    public int Height { get { return levelLayout.Height; } }

    protected LevelLayout levelLayout;

    protected BaseValueAndColorGenerator valueAndColorGenerator;

    protected Condition winCondition;

    protected Condition loseCondition;

    protected MergeRule mergeRule;

    protected BaseMergeEffect mergeEffect;

    protected Level(LevelLayout layout)
    {
        levelLayout = layout;
    }

    protected Level(LevelLayout layout, BaseValueAndColorGenerator valueAndColorGenerator, Condition winCondition, Condition loseCondition,MergeRule mergeRule, BaseMergeEffect mergeEffect) : this(layout)
    {
        this.valueAndColorGenerator = valueAndColorGenerator;
        this.winCondition = winCondition;
        this.loseCondition = loseCondition;
        this.mergeRule = mergeRule;
        this.mergeRule.Initialize(this.valueAndColorGenerator);
        this.mergeEffect = mergeEffect;
    }

    public int GetNewValue(int value, Direction movedDirection)
    {
        return valueAndColorGenerator.GetNewValue(value, movedDirection);
    }

    public Color GetColorForValue(int val)
    {
        return valueAndColorGenerator.GetColorForValue(val);
    }

    public List<MoveableTileSpawnInfo> GetMoveableTileLayout()
    {
        return levelLayout.MoveableTiles;
    }

    public List<BlockingTileSpawnInfo> GetBlockerTileLayout()
    {
        return levelLayout.BlockingTiles;
    }

    public bool CheckIfAchievedWinCondition(GameState stateOfGame)
    {
        return winCondition.CheckIfConditionAchived(stateOfGame);
    }

    public bool CheckIfLost(GameState stateOfGame)
    {
        return loseCondition.CheckIfConditionAchived(stateOfGame);
    }

    public int ClampValueIntoAccaptableRange(int value)
    {
        return valueAndColorGenerator.ClampValueIntoAccaptableRange(value);
    }

    public bool CheckIfTilesCanMerge(MoveableTile tileTryingToMove, MoveableTile tileMovedInto, Direction movingDirection)
    {
        return mergeRule.CanMerge(tileMovedInto.Value, tileTryingToMove.Value, movingDirection);
    }

    public void ActivateMergeEffect(MoveableTile moved, MoveableTile mergedInto,Map map)
    {
        mergeEffect.OnTilesMerged(moved, mergedInto, map);
    }

    public string Serialize()
    {
        string ser = "";

        //layout
        ser += levelLayout.Serialize();
        ser += dataSeperator;

        //value gen
        ser += valueAndColorGenerator.Serialize();
        ser += dataSeperator;

        //winCondition
        ser += winCondition.Serialize();
        ser += dataSeperator;

        //loseCondition
        ser += loseCondition.Serialize();
        ser += dataSeperator;

        //mergeRule
        ser += mergeRule.Serialize();
        ser += dataSeperator;

        //merge effect
        ser += mergeEffect.Serialize();

        return ser;
    }

    public static Level Deserialize(string str)
    {
        string[] split = str.Split(dataSeperator.ToCharArray());

        if (split.Length < 6)
            throw new Exception($"Can't build level from this data: {str}");

        LevelLayout layout = LevelLayout.Deserialize(split[0]);
        BaseValueAndColorGenerator gen = ValueAndColorGeneratorFactory.BuildValueAndColorGenerator(split[1]);

        Condition winCond = ConditionFactory.BuildCondition(split[2]);

        Condition loseCond = ConditionFactory.BuildCondition(split[3]);

        MergeRule mergeRule = MergeRuleFactory.BuildMergeRule(split[4]);

        BaseMergeEffect effect = MergeEffectFactory.BuildMergeEffect(split[5]);

        return new Level(layout, gen, winCond, loseCond,mergeRule, effect);
    }
}





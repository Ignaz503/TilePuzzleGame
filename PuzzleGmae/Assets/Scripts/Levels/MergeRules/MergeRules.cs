using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class BaseMergeRule
{
    protected BaseValueAndColorGenerator valueGenerator;

    public BaseMergeRule()
    {}

    public BaseMergeRule(string data)
    {}

    public abstract bool CanMerge(int valPotentialMergedInto, int valMovingTile, Direction potentialMoveDirection);

    public abstract string GetInstanceDescription();

    public abstract string Serialize();

    public void Initialize(BaseValueAndColorGenerator valueGen)
    {
        valueGenerator = valueGen;
    }

}

public static class MergeRuleFactory
{
    public static BaseMergeRule BuildMergeRule(Type t, string data)
    {
        return Activator.CreateInstance(t, new object[] { data }) as  BaseMergeRule;
    }

    public static BaseMergeRule BuildMergeRule(string data)
    {
        string[] split = data.Split('\n');
        return BuildMergeRule(Type.GetType(split[0].Trim()), data);
    }
}

[Description("Allows merging of two tiles when the value of the tile you are trying to move equals the value of the tile you are trying to move into, after the move is done\n valuePotentialMergedInto == valueGenerator.GetNewValue(valueTileTryingToMove, potentialMoveDirection)")]
public class DefaultMergeRule : BaseMergeRule
{
    public DefaultMergeRule(string data) : base(data)
    {
    }

    public override bool CanMerge(int valuePotentialMergedInto, int valueTileTryingToMove, Direction potentialMoveDirection)
    {
        return valuePotentialMergedInto == valueGenerator.GetNewValue(valueTileTryingToMove, potentialMoveDirection);
    }

    public override string GetInstanceDescription()
    {
        return GetType().GetCustomAttribute<DescriptionAttribute>().Description;
    }

    public override string Serialize()
    {
        return GetType().ToString();
    }
}

[Description("Allows merging of two adjacent tiles when they have the same value\n valPotentialMergedInto == valMovingTile")]
public class SameValueMergeRule : BaseMergeRule
{
    public SameValueMergeRule(string data) : base(data)
    {}

    public override bool CanMerge(int valPotentialMergedInto, int valMovingTile, Direction potentialMoveDirection)
    {
        return valPotentialMergedInto == valMovingTile;
    }

    public override string GetInstanceDescription()
    {
        return GetType().GetCustomAttribute<DescriptionAttribute>().Description;
    }

    public override string Serialize()
    {
        return GetType().ToString();
    }
}

[Description("Allows merging of Tiles when difference of the tile merged into minus the tile you are trying to move equals N\n valPotentialMergedInto - valMovingTile) == N")]
public class DifferenceOfNMergeRule : BaseMergeRule
{
    [ParserType(ParserTypeAttribute.ParseableTypes.INTEGER,"Difference")]int N;

    public DifferenceOfNMergeRule(int n)
    {
        N = n;
    }

    public DifferenceOfNMergeRule(string data) : base(data)
    {
        string[] split = data.Split('\n');

        if (split.Length < 2)
            throw new Exception($"Can't build {GetType()} from this data: {data}");

        N = int.Parse(split[1].Trim());
    }

    public override bool CanMerge(int valPotentialMergedInto, int valMovingTile, Direction potentialMoveDirection)
    {
        return (Mathf.Abs(valPotentialMergedInto - valMovingTile)) == N;
    }

    public override string GetInstanceDescription()
    {
        return $"Allows merging of Tiles when difference of the tile merged into minus the tile you are trying to move equals {N}\n valPotentialMergedInto - valMovingTile) == {N}";
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + N;
    }
}
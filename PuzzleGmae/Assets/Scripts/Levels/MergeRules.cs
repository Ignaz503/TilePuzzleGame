﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MergeRule
{
    protected BaseValueAndColorGenerator valueGenerator;

    public MergeRule()
    {

    }

    public MergeRule(string data)
    {}

    public abstract bool CanMerge(int valPotentialMergedInto, int valMovingTile, Direction potentialMoveDirection);

    public abstract string Serialize();

    public void Initialize(BaseValueAndColorGenerator valueGen)
    {
        valueGenerator = valueGen;
    }
}

public static class MergeRuleFactory
{
    public static MergeRule BuildMergeRule(Type t, string data)
    {
        return Activator.CreateInstance(t, new object[] { data }) as  MergeRule;
    }

    public static MergeRule BuildMergeRule(string data)
    {
        string[] split = data.Split('\n');
        return BuildMergeRule(Type.GetType(split[0].Trim()), data);
    }
}

public class DefaultMergeRule : MergeRule
{
    public DefaultMergeRule(string data) : base(data)
    {
    }

    public override bool CanMerge(int valuePotentialMergedInto, int valueTileTryingToMove, Direction potentialMoveDirection)
    {
        return valuePotentialMergedInto == valueGenerator.GetNewValue(valueTileTryingToMove, potentialMoveDirection);
    }

    public override string Serialize()
    {
        return GetType().ToString();
    }
}

public class SameValueMergeRule : MergeRule
{
    public SameValueMergeRule(string data) : base(data)
    {}

    public override bool CanMerge(int valPotentialMergedInto, int valMovingTile, Direction potentialMoveDirection)
    {
        return valPotentialMergedInto == valMovingTile;
    }

    public override string Serialize()
    {
        return GetType().ToString();
    }
}

public class DifferenceOfNMergeRule : MergeRule
{
    int N;

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
        return (valPotentialMergedInto - valMovingTile) == N;
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + N;
    }
}
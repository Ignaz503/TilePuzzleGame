using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionFactory
{
    public static Condition BuildCondition(Type t, string data)
    {
        //TODO: switch on type when c# version of unity allows 

        return Activator.CreateInstance(t, new object[] { data }) as Condition;

        //switch (t.ToString())
        //{
        //    case "AllTilesMergedWinCondition":
        //        return new AllTilesMergedWinCondition(data);
        //    default:
        //        throw new Exception($"Factory does not know how to create win condition of type {t.ToString()}");
        //}
    }

    public static Condition BuildCondition(string data)
    {
        //TODO: switch on type when c# version of unity allows 
        string[] split = data.Split('\n');

        return Activator.CreateInstance(Type.GetType(split[0].Trim()), new object[] { data }) as Condition;
    }
}

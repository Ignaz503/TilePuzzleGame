﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public abstract  class ConditionAttribute : Attribute
{
    public enum ConditionType
    {
        Lose,
        Win
    }
    
    public ConditionType Type { get; protected set; }

    public ConditionAttribute(ConditionType type)
    {
        Type = type;
    }
}

public class LoseCondition : ConditionAttribute
{
    public LoseCondition() : base(ConditionType.Lose)
    {
    }
}

public class WinCondition : ConditionAttribute
{
    public WinCondition() : base(ConditionType.Win)
    {
    }
}
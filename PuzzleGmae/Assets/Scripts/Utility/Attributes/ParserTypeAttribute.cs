using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ParserTypeAttribute : Attribute
{
    public enum ParseableTypes
    {
        FLOAT,
        INTEGER,
        STRING
    }

    public ParseableTypes Type { get; protected set; }
    public string DisplayName { get; protected set; }

    public ParserTypeAttribute(ParseableTypes type, string displayName)
    {
        Type = type;
        DisplayName = displayName;
    }
}

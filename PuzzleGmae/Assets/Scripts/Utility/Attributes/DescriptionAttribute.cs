using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class DescriptionAttribute : Attribute
{
    public string Description { get; protected set; }

    public DescriptionAttribute(string description)
    {
        Description = description;
    }
}

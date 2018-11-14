using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Newtonsoft.Json;

public abstract class BaseValueAndColorGenerator
{
    public ColorPicker ColorPicker { get; protected set; }

    protected DirectionMapping directionToValueMapping;

    protected BaseValueAndColorGenerator(ColorPicker colorPicker, DirectionMapping mapping)
    {
        this.ColorPicker = colorPicker;
        this.directionToValueMapping = mapping;
    }

    public BaseValueAndColorGenerator(string data)
    {}

    public abstract int GetNewValue(int value, Direction movedDirection);

    public abstract int ClampValueIntoAccaptableRange(int value);

    public Color GetColorForValue(int value)
    {
        return ColorPicker.GetColor(value);
    }

    public abstract string Serialize();
}

public class DefaultValueAndColorGenerator : BaseValueAndColorGenerator
{
    public DefaultValueAndColorGenerator(ColorPicker colorPicker, DirectionMapping mapping) : base(colorPicker, mapping)
    {}

    public DefaultValueAndColorGenerator(string data) : base(data)
    {
        string[] split = data.Split('\n');

        if (split.Length < 3)
            throw new Exception($"Can't build {GetType()} from this data: {data}");

        ColorPicker = ColorPickerFactory.BuildColorPicker(split[1]);
        directionToValueMapping = new DirectionMapping(split[2]);
    }

    public override int ClampValueIntoAccaptableRange(int value)
    {
        value = value < 0 ? ColorPicker.Count + value : value;
        value = value >= ColorPicker.Count ? value - ColorPicker.Count : value;
        return value;
    }

    public override int GetNewValue(int value, Direction movedDirection)
    {
        return ClampValueIntoAccaptableRange(value + directionToValueMapping.GetValue(movedDirection));
    }

    public override string Serialize()
    {
        return GetType().ToString() + "\n" + ColorPicker.Serialize() + "\n" + directionToValueMapping.Serialize();
    }
}

public static class ValueAndColorGeneratorFactory
{
    public static BaseValueAndColorGenerator BuildValueAndColorGenerator(Type t, string data)
    {
        return Activator.CreateInstance(t, new object[] { data }) as BaseValueAndColorGenerator;
    }

    public static BaseValueAndColorGenerator BuildValueAndColorGenerator(string data)
    {
        string[] split = data.Split('\n');

        Type t = Type.GetType(split[0].Trim());

        return Activator.CreateInstance(t, new object[] { data }) as BaseValueAndColorGenerator;
    }
}

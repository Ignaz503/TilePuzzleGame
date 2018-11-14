using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class ColorPicker
{

    [SerializeField]public Color[] Colors { get; protected set; }

    public ColorPicker(Color[] colors)
    {
        this.Colors = colors;
    }

    public ColorPicker(string data)
    {}

    public int Count { get { return Colors.Length; } }

    public abstract Color GetColor(int value);

    public abstract string Serialize();
}


public static class ColorPickerFactory
{
    public static ColorPicker BuildColorPicker(Type t, string data)
    {
        return Activator.CreateInstance(t, new object[] { data }) as ColorPicker;
    }

    public static ColorPicker BuildColorPicker(string data)
    {
        string[] split = data.Split('#');
        return BuildColorPicker(Type.GetType(split[0]), data);
    }
}
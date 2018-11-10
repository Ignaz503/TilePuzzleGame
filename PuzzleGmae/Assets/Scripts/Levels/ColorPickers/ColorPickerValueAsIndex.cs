using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorPickerValueAsIndex : ColorPicker
{
    public ColorPickerValueAsIndex(Color[] colors) : base(colors)
    { }

    public ColorPickerValueAsIndex(string data):base(data)
    {
        string[] split = data.Split('#');
        if (split.Length < 3)
            throw new Exception($"Can't create {GetType()} from this data: {data}");

        //0 is type
        //1 is count of colors
        colors = new Color[int.Parse(split[1])];

        //2 is colors
        string[] colorSplit = split[2].Split('!');

        if (colorSplit.Length < colors.Length)
            throw new Exception($"Not enough colors for colors array wiht length{colors.Length}, only found {colorSplit.Length}");

        for(int i = 0; i < colors.Length; i++)
        {
            string[] floats = colorSplit[i].Split(' ');
            if (floats.Length < 4)
                throw new Exception($"found invalid color definitio: {colorSplit[i]}");

            float a = float.Parse(floats[0]);
            float r = float.Parse(floats[1]);
            float g = float.Parse(floats[2]);
            float b = float.Parse(floats[3]);
            colors[i] = new Color(r, g, b, a);
        }
    }

    public override Color GetColor(int value)
    {
        return colors[value];
    }

    public override string Serialize()
    {
        string ser = "";
        ser+= GetType().ToString()+"#";
        ser += Count.ToString() + "#";
        foreach(Color c in colors)
        {
            ser += c.a + " ";
            ser += c.r + " ";
            ser += c.g + " ";
            ser += c.b + " ";
            ser += "!";
        }

        return ser;
    }
}

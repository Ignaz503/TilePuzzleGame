using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMapping
{
    Dictionary<Direction, int> directionMapping;

    public DirectionMapping()
    {
        directionMapping = new Dictionary<Direction, int>();
    }

    public DirectionMapping(string data): this()
    {
        string[] split = data.Split('!');

        if (split.Length < 4)//should probably be 5 cause last one is always empty
            throw new System.Exception($"Can't create valid direction mapping, missing values for directions, from this data: {data}");

        for(int i = 0;  i < 4; i++)
        {
            string[] mapping = split[i].Split(' ');
            if (mapping.Length < 2)
                throw new System.Exception($"Can't build correct mapping from this data: {split[i]}, can't read two numbers from it");

            uint dir = uint.Parse(mapping[0]);
            int val = int.Parse(mapping[1]);

            directionMapping.Add(dir, val);
        }
    }

    public DirectionMapping(int up, int down, int right, int left) :this()
    {
        directionMapping.Add(Direction.Up, up);
        directionMapping.Add(Direction.Down, down);
        directionMapping.Add(Direction.Left, left);
        directionMapping.Add(Direction.Right, right);
    }

    public int GetValue(Direction dir)
    {
        return directionMapping[dir];
    }

    public int GetValueDirectionRight()
    {
        return directionMapping[Direction.Right];
    }

    public int GetValueDirectionLeft()
    {
        return directionMapping[Direction.Left];
    }

    public int GetValueDirectionUp()
    {
        return directionMapping[Direction.Up];
    }

    public int GetValueDirectionDown()
    {
        return directionMapping[Direction.Down];
    }

    public void AddValue(Direction dir, int value, bool overrideIfExisiting = true)
    {
        if (!directionMapping.ContainsKey(dir))
        {
            directionMapping.Add(dir, value);
        }
        else if (overrideIfExisiting)
        {
            directionMapping[dir] = value;
        }
    }

    public void SetValueDirectionRight(int value, bool overrideIfExisting = true)
    {
        AddValue(Direction.Right, value, overrideIfExisting);
    }

    public void SetValueDirectionLeft(int value, bool overrideIfExisting = true)
    {
        AddValue(Direction.Left, value, overrideIfExisting);
    }

    public void SetValueDirectionUp(int value, bool overrideIfExisting = true)
    {
        AddValue(Direction.Up, value, overrideIfExisting);
    }

    public void SetValueDirectionDown(int value, bool overrideIfExisting = true)
    {
        AddValue(Direction.Down, value, overrideIfExisting);
    }

    public bool ValidateMapping()
    {
        if (directionMapping.Keys.Count < 4)
            return false;
        else return true;
    }

    public string Serialize()
    {
        string ser = "";

        foreach (Direction dir in directionMapping.Keys)
        {
            ser += ((uint)dir).ToString() + " ";
            ser += directionMapping[dir] + "!";
        }

        return ser;
    }
}

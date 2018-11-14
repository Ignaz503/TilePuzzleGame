using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2Int ToVec2IntXY(this Vector3Int vec)
    {
        return new Vector2Int(vec.x, vec.y);
    }

    public static Vector3Int ToVec3XY(this Vector2Int vec, int other)
    {
        return new Vector3Int(vec.x, vec.y, other);
    }
}

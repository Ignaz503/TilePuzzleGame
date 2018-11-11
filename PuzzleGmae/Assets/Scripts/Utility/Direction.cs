using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    private enum InnerDirection
    {
        Right,
        Up,
        Left,
        Down
    }

    #region Static Values
    public const uint Right = (uint)InnerDirection.Right;
    public const uint Left = (uint)InnerDirection.Left;
    public const uint Up = (uint)InnerDirection.Up;
    public const uint Down = (uint)InnerDirection.Down;
    // Aliases for relative directions
    public const uint Forward = (uint)InnerDirection.Up;
    public const uint Back = (uint)InnerDirection.Down;
    #endregion

    InnerDirection direction;

    #region ctors

    private Direction(uint dir)
    {
        this.direction = (InnerDirection)dir;
    }

    private Direction(InnerDirection direction)
    {
        this.direction = direction;
    }
    #endregion

    #region Conversion and Operators
    public override bool Equals(object obj)
    {
        if (obj is Direction)
            return direction == ((Direction)obj).direction;
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return ((uint)this).GetHashCode();
    }

    public override string ToString()
    {
        return direction.ToString();
    }

    public static bool operator ==(Direction lhs, Direction rhs)
    {
        return lhs.direction == rhs.direction;
    }

    public static bool operator !=(Direction lhs, Direction rhs)
    {
        return !(lhs == rhs);
    }

    public static implicit operator uint(Direction direction)
    {
        return (uint)direction.direction;
    }

    public static implicit operator Vector2(Direction direction)
    {
        switch (direction)
        {
            case Right:
                return Vector2.right;
            case Left:
                return Vector2.left;
            case Up:
                return Vector2.up;
            case Down:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    public static implicit operator Vector2Int(Direction direction)
    {
        switch (direction)
        {
            case Right:
                return Vector2Int.right;
            case Left:
                return Vector2Int.left;
            case Up:
                return Vector2Int.up;
            case Down:
                return Vector2Int.down;
            default:
                return Vector2Int.zero;
        }
    }

    public static implicit operator Vector3(Direction direction)
    {
        switch (direction)
        {
            case Right:
                return Vector3.right;
            case Left:
                return Vector3.left;
            case Up:
                return Vector3.up;
            case Down:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }

    public static implicit operator Vector3Int(Direction direction)
    {
        switch (direction)
        {
            case Right:
                return Vector3Int.right;
            case Left:
                return Vector3Int.left;
            case Up:
                return Vector3Int.up;
            case Down:
                return Vector3Int.down;
            default:
                return Vector3Int.zero;
        }
    }

    public static implicit operator Direction(uint val)
    {
        switch (val)
        {
            case Right:
                return new Direction(Right);
            case Left:
                return new Direction(Left);
            case Up:
                return new Direction(Up);
            case Down:
                return new Direction(Down);
            default:
                return new Direction(Right);
        }
    }

    public static explicit operator Direction(Vector2 vector)
    {
        if (vector == Vector2.right)
            return Right;
        else if (vector == Vector2.left)
            return Left;
        else if (vector == Vector2.up)
            return Up;
        else if (vector == Vector2.down)
            return Down;
        else
            return Right;
    }

    public static explicit operator Direction(Vector2Int vector)
    {
        if (vector == Vector2Int.right)
            return Right;
        else if (vector == Vector2Int.left)
            return Left;
        else if (vector == Vector2Int.up)
            return Up;
        else if (vector == Vector2Int.down)
            return Down;
        else
            return Right;
    }

    public static explicit operator Direction(Vector3 vector)
    {
        if (vector == Vector3.right)
            return Right;
        else if (vector == Vector3.left)
            return Left;
        else if (vector == Vector3.up)
            return Up;
        else if (vector == Vector3.down)
            return Down;
        else
            return Right;
    }

    public static explicit operator Direction(Vector3Int vector)
    {
        if (vector == Vector3Int.right)
            return Right;
        else if (vector == Vector3Int.left)
            return Left;
        else if (vector == Vector3Int.up)
            return Up;
        else if (vector == Vector3Int.down)
            return Down;
        else
            return Right;
    }
    #endregion

    public static Direction RotateRight(Direction direction)
    {
        return new Direction((InnerDirection)(((uint)InnerDirection.Down + direction) % ((uint)InnerDirection.Down + 1)));
    }

    public static Direction RotateLeft(Direction direction)
    {
        return new Direction((InnerDirection)((direction + 1) % ((uint)InnerDirection.Down + 1)));
    }

    public static Direction Invert(Direction direction)
    {
        return new Direction((InnerDirection)((direction + 2) % ((uint)InnerDirection.Down + 1)));
    }

    public static Direction Random()
    {
        return (uint)UnityEngine.Random.Range((float)Right,(float)(Down));
    }

}

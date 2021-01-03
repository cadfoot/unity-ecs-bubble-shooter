using UnityEngine;

public static class Hex
{
    //x - col
    //y - row

    public enum Neighbour
    {
        Right       = 0,
        BottomRight = 1,
        BottomLeft  = 2,
        Left        = 3,
        TopLeft     = 4,
        TopRight    = 5
    }
    
    public static Vector2Int[] NeighboursOffsets = new[]
    {
       new Vector2Int(+2, +0),    // right
       new Vector2Int(+1, +1),    // bottom-right
       new Vector2Int(-1, +1),    // bottom-left
       new Vector2Int(-2, +0),    // left
       new Vector2Int(-1, -1),    // top-left
       new Vector2Int(+1, -1),    // top-right
    };
    
    /*
    public static Vector2Int[] NeighboursOffsets = new[]
    {
        new Vector2Int(+2, +0),    // right
        new Vector2Int(+1, +1),    // bottom-right
        new Vector2Int(-1, +1),    // bottom-left
        new Vector2Int(-2, +0),    // left
        new Vector2Int(-1, -1),    // top-left
        new Vector2Int(+1, -1),    // top-right
    };
    */

    public static Vector2Int GetNeighbourCoord(Vector2Int coord, Neighbour neighbour)
    {
        return coord + NeighboursOffsets[(int) neighbour];
    }

    public static Vector2 ToWorldPosition(Vector2Int coord, float radius = .5f)
    {
        var x = radius * Mathf.Sqrt(3f) / 2f * coord.x;
        var y = radius * 3f / 2f * coord.y;

        return new Vector2(x, -y);
    }

    public static void GetColBounds(int row, int width, out int start, out int end)
    {
        start = row % 2 * 1;
        end = width * 2 + start - 1;
    }
}

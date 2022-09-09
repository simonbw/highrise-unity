using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuilder
{
    public GridBuilder g;
    public CellBuilder(GridBuilder g)
    {
        this.g = g;
    }
}

public enum Direction {
    RIGHT,
    DOWN,
    LEFT,
    UP,
    RIGHTUP,
    RIGHTDOWN,
    LEFTUP,
    LEFTDOWN,
}

public class RenameMe {
    private static readonly Dictionary<Direction, Vector2> _directionToV = new Dictionary<Direction, Vector2>
    {
        {Direction.RIGHT, new Vector2(1f, 0f)},
        {Direction.DOWN, new Vector2(0f, 1f)},
        {Direction.LEFT, new Vector2(-1f, 0f)},
        {Direction.UP, new Vector2(0f, -1f)},
        {Direction.RIGHTUP, new Vector2(1f, -1f)},
        {Direction.RIGHTDOWN, new Vector2(1f, 1f)},
        {Direction.LEFTUP, new Vector2(-1f, -1f)},
        {Direction.LEFTDOWN, new Vector2(-1f, 1f)}
    };

    public static Dictionary<Direction, Vector2> DirectionToV
    {
        get
        {
            return _directionToV;
        }
    }
}

public class WallBuilder
{
    public GridBuilder g;
    Vector2 cellLevelCoords;
    Direction directionFromCell;
    Vector2 directionFromCellV;

    public bool exists = true;
    //indicates that this wall is not an important part of the level structure and can
        // be removed during maze generation
    public bool removable = true;

    public WallBuilder(GridBuilder g, Vector2 cellLevelCoords, Direction directionFromCell)
    {
        this.g = g;
        this.cellLevelCoords = cellLevelCoords;
        this.directionFromCell = directionFromCell;
        this.directionFromCellV = RenameMe.DirectionToV[directionFromCell];
    }

    public Vector2 getWorldCoords() { 
        return cellLevelCoords * 2 + directionFromCellV;
    }

    public Quaternion getRotation() { 
        return Quaternion.FromToRotation(
            RenameMe.DirectionToV[Direction.RIGHT],
            directionFromCellV
        );;
    }
}

public class GridBuilder
{
    public CellBuilder[,] cells;
    public WallBuilder[] walls;
}

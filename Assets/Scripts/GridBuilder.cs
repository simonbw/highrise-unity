using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellBuilder
{
    public GridBuilder g;

    public WallBuilder right;
    public WallBuilder up;
    public WallBuilder left;
    public WallBuilder down;

    public CellBuilder(GridBuilder g)
    {
        this.g = g;
    }

    public WallBuilder wall(Direction d) {
        switch (d) {
            case Direction.RIGHT:
                return right;
            case Direction.DOWN:
                return right;
            case Direction.LEFT:
                return left;
            case Direction.UP:
                return up;
        }
        return null;
    }
}

public enum Direction {
    NONE,
    RIGHT,
    DOWN,
    LEFT,
    UP,
    RIGHTUP,
    RIGHTDOWN,
    LEFTUP,
    LEFTDOWN,
}

public class DirectionToV {
    private static readonly Dictionary<Direction, Vector2Int> _directionToV = new Dictionary<Direction, Vector2Int>
    {
        {Direction.RIGHT, new Vector2Int(1, 0)},
        {Direction.DOWN, new Vector2Int(0, -1)},
        {Direction.LEFT, new Vector2Int(-1, 0)},
        {Direction.UP, new Vector2Int(0, 1)},
        {Direction.RIGHTUP, new Vector2Int(1, 1)},
        {Direction.RIGHTDOWN, new Vector2Int(1, -1)},
        {Direction.LEFTUP, new Vector2Int(-1, 1)},
        {Direction.LEFTDOWN, new Vector2Int(-1, -1)}
    };

    public static Dictionary<Direction, Vector2Int> v
    {
        get
        {
            return _directionToV;
        }
    }

    public static Direction d(Vector2Int v) {
        foreach (KeyValuePair<Direction, Vector2Int> entry in _directionToV) {
            // Pointer equals???
            if (v.Equals(entry.Value)) {
                return entry.Key;
            }
        }
        return Direction.NONE;
    }
}

public class WallBuilder
{
    public GridBuilder g;
    Vector2 cellLevelCoords;
    Direction directionFromCell;
    Vector2 directionFromCellV;

    public bool exists = true;
    //indicates that this wall is  an important part of the level structure and should not
        // be removed during maze generation
    public bool structural = false;

    public WallBuilder(GridBuilder g, Vector2 cellLevelCoords, Direction directionFromCell)
    {
        this.g = g;
        this.cellLevelCoords = cellLevelCoords;
        this.directionFromCell = directionFromCell;
        this.directionFromCellV = DirectionToV.v[directionFromCell];
    }

    public Vector2 getWorldCoords() { 
        return cellLevelCoords * 2 + directionFromCellV + new Vector2(1, 1);
    }

    public Quaternion getRotation() { 
        return Quaternion.FromToRotation(
            (Vector2) DirectionToV.v[Direction.RIGHT],
            directionFromCellV
        );
    }
}

public class GridBuilder
{
    public Vector2Int dimensions;
    public CellBuilder[,] cells;
    public WallBuilder[] walls;

    public GridBuilder(Vector2Int dimensions) {
        this.dimensions = dimensions;

        cells = new CellBuilder[dimensions.x, dimensions.y];
        int nWalls = 2 * dimensions.x * dimensions.y + dimensions.x + dimensions.y;
        walls = new WallBuilder[nWalls];

        int wallI = 0;
        foreach (Vector2Int cP in traverseDimensions(dimensions))
        {
            CellBuilder c = new CellBuilder(this);
            cells[cP.x, cP.y] = c;

            WallBuilder right = new WallBuilder(this, cP, Direction.RIGHT);
            if (cP.x == dimensions.x - 1) {
                right.structural = true;
            }
            c.right = right;
            walls[wallI++] = right;
            WallBuilder up = new WallBuilder(this, cP, Direction.UP);
            if (cP.y == dimensions.y - 1) {
                up.structural = true;
            }
            c.up = up;
            walls[wallI++] = up;

            if (cP.x == 0) {
                WallBuilder left = new WallBuilder(this, cP, Direction.LEFT);
                left.structural = true;
                c.left = left;
                walls[wallI++] = left;
            } else {
                c.left = cells[cP.x - 1, cP.y].right;
            }
            if (cP.y == 0) {
                WallBuilder down = new WallBuilder(this, cP, Direction.DOWN);
                down.structural = true;
                c.down = down;
                walls[wallI++] = down;
            } else {
                c.down = cells[cP.x, cP.y - 1].up;
            }
        }
        // Expect exactly nWalls to have been created
        Debug.Assert(wallI == nWalls);
    }

    public WallBuilder wallPositionToBuilder(Vector2 position) {
        Vector2Int cellP = Vector2Int.FloorToInt(position);
        Vector2Int directionV = Vector2Int.RoundToInt((position - cellP).normalized);
        if (cellP.x == -1) {
            cellP.x++;
            directionV = -directionV;
        } else if (cellP.y == -1) {
            cellP.y++;
            directionV = -directionV;
        }

        CellBuilder c = cells[cellP.x, cellP.y];
        Direction d = DirectionToV.d(directionV);
        return c.wall(d);
    }

    public IEnumerable<Vector2Int> traverseDimensions(Vector2Int dimensions) {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                Vector2Int v = new Vector2Int(x, y);
                yield return v;
            }
        }
    }

    public IEnumerable<CellBuilder> traverseCells(Vector2Int lowerLeftCell, Vector2Int dimensions) {
        return traverseDimensions(dimensions)
            .Select(roomCoords => lowerLeftCell + roomCoords)
            .Select(levelCoords => cells[levelCoords.x, levelCoords.y]);
    }

    public IEnumerable<WallBuilder> traverseExteriorWalls(Vector2Int lowerLeftCell, Vector2Int dimensions) {
        Vector2Int v = lowerLeftCell;
        WallBuilder w;
        for (int x = 0; x < dimensions.x; x++)
        {
            w = cells[v.x + x, v.y + 0].down;
            yield return w;
            w = cells[v.x + x, v.y + dimensions.y - 1].up;
            yield return w;
        }
        for (int y = 0; y < dimensions.y; y++)
        {
            w = cells[v.x + 0,                v.y + y].left;
            yield return w;
            w = cells[v.x + dimensions.x - 1, v.y + y].right;
            yield return w;
        }
    }

    public IEnumerable<WallBuilder> traverseInteriorWalls(Vector2Int lowerLeftCell, Vector2Int dimensions) {
        foreach (Vector2Int roomCoords in traverseDimensions(dimensions)) {
            Vector2Int levelCoords = roomCoords + lowerLeftCell;
            CellBuilder c = cells[levelCoords.x, levelCoords.y];
            bool topRow = roomCoords.y == dimensions.y - 1;
            bool rightColumn = roomCoords.x == dimensions.x - 1;
            if (!rightColumn) {
                WallBuilder r = c.right;
                yield return r;
            }
            if (!topRow) {
                WallBuilder u = c.up;
                yield return u;
            }
        }
    }
}

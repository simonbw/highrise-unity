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
            RenameMe.DirectionToV[Direction.UP],
            directionFromCellV
        );;
    }
}

public class GridBuilder
{
    public CellBuilder[,] cells;
    public WallBuilder[] walls;
}

public class LevelGenerator : MonoBehaviour
{
    public GameObject wallPrefab;

    void Start()
    {
        int width = 10;
        GridBuilder g = new GridBuilder();
        g.cells = new CellBuilder[width, width];
        int nWalls = 2 * width * width + 2 * width;
        g.walls = new WallBuilder[nWalls];

        int wallI = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (x == 0) {
                    g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.LEFT);
                    wallI++;
                }
                if (y == 0) {
                    g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.UP);
                    wallI++;
                }
                g.cells[x, y] = new CellBuilder(g);
                g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.RIGHT);
                wallI++;
                g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.DOWN);
                wallI++;
            }
        }
        // Expect exactly nWalls to have been created
        Debug.Assert(wallI == nWalls);

        foreach (WallBuilder w in g.walls) {
            Instantiate(wallPrefab, w.getWorldCoords() + new Vector2(5f, 5f), w.getRotation());
        }

    }

    void Update()
    {
        
    }
}

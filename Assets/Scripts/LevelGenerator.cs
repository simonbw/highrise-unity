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

public class WallBuilder
{
    public GridBuilder g;
    public WallBuilder(GridBuilder g)
    {
        this.g = g;
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
                    g.walls[wallI] = new WallBuilder(g);
                    wallI++;
                }
                if (y == 0) {
                    g.walls[wallI] = new WallBuilder(g);
                    wallI++;
                }
                g.cells[x, y] = new CellBuilder(g);
                g.walls[wallI] = new WallBuilder(g);
                wallI++;
                g.walls[wallI] = new WallBuilder(g);
                wallI++;
            }
        }
        // Expect exactly nWalls to have been created
        Debug.Assert(wallI == nWalls);


        Instantiate(wallPrefab);

    }

    void Update()
    {
        
    }
}

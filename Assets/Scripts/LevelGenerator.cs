using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    GridBuilder g;

    void initGrid()
    {
        int width = 10;
        this.g = new GridBuilder();
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
                    g.walls[wallI].removable = false;
                    wallI++;
                }
                if (y == 0) {
                    g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.UP);
                    g.walls[wallI].removable = false;
                    wallI++;
                }
                g.cells[x, y] = new CellBuilder(g);
                g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.RIGHT);
                if (x == width - 1) {
                    g.walls[wallI].removable = false;
                }
                wallI++;
                g.walls[wallI] = new WallBuilder(g, new Vector2(x, y), Direction.DOWN);
                if (y == width - 1) {
                    g.walls[wallI].removable = false;
                }
                wallI++;
            }
        }
        // Expect exactly nWalls to have been created
        Debug.Assert(wallI == nWalls);
    }

    void generateLevel() {
        initGrid();

        

    }

    void instantiateEntities()
    {
        foreach (WallBuilder w in g.walls) {
            if (!w.exists) {
                continue;
            }
            Instantiate(wallPrefab, w.getWorldCoords() + new Vector2(5f, 5f), w.getRotation());
        }
    }

    void Start()
    {
        generateLevel();
        instantiateEntities();
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct RoomConfiguration {

}

public class LevelTemplate : MonoBehaviour
{
    public GameObject[] roomTemplates;
    public GameObject wall;
    GridBuilder g;

    void initGrid()
    {
        int width = 10;
        this.g = new GridBuilder(new Vector2Int(width, width));
    }

    void generateLevel() {
        initGrid();

        // TODO
        Vector2Int [] roomPositions = {new Vector2Int(0, 0)};

        foreach (GameObject rRaw in roomTemplates) {
            foreach (Vector2Int roomPos in roomPositions) {
                RoomTemplate r = rRaw.GetComponent(typeof(RoomTemplate)) as RoomTemplate;
                if (r == null) {
                    Debug.LogWarning("expected only RoomTemplate components");
                    continue;
                }

                Vector2Int dimensions = (Vector2Int) r.getDimensions();
                List<WallBuilder> doorBuilders = r.doors
                    .Select(d => d.transform.position)
                    .Select(roomCoords => ((Vector2) roomPos) + ((Vector2) roomCoords))
                    .Select(levelCoords => g.wallPositionToBuilder(levelCoords))
                    .ToList();
                if (doorBuilders.Any(d => d == null || d.structural))
                {
                    Debug.Log("Ineligible: Door would destroy structural wall");
                    // Ineligible: Door would destroy structural wall
                    continue;
                }

                // TODO make this a yieldy iEnumerable in the room script
                for (int roomX = 0; roomX < dimensions.x; roomX++)
                {
                    for (int roomY = 0; roomY < dimensions.y; roomY++)
                    {
                        Vector2Int roomCoords = new Vector2Int(roomX, roomY);
                        Vector2Int levelCoords = roomCoords + roomPos;
                        CellBuilder c = g.cells[levelCoords.x, levelCoords.y];
                        bool topRow = roomY == dimensions.y - 1;
                        bool rightColumn = roomX == dimensions.x - 1;
                        if (c.right.structural && !rightColumn || c.up.structural && !topRow)
                        {
                            Debug.Log("Ineligible: Room internals would destroy structural wall");
                            // Ineligible: Room internals would destroy structural wall
                            continue;
                        }
                    }
                }

                Debug.Log("Found eligible room placement");
                for (int roomX = 0; roomX < dimensions.x; roomX++)
                {
                    for (int roomY = 0; roomY < dimensions.y; roomY++)
                    {
                        Vector2Int roomCoords = new Vector2Int(roomX, roomY);
                        Vector2Int levelCoords = roomCoords + roomPos;
                        CellBuilder c = g.cells[levelCoords.x, levelCoords.y];
                        bool bottomRow = roomY == 0;
                        bool leftColumn = roomX == 0;
                        bool topRow = roomY == dimensions.y - 1;
                        bool rightColumn = roomX == dimensions.x - 1;
                        if (rightColumn)
                        {
                            c.right.structural = true;
                        } else {
                            c.right.exists = false;
                        }
                        if (topRow)
                        {
                            c.up.structural = true;
                        } else {
                            c.up.exists = false;
                        }
                        if (bottomRow)
                        {
                            c.down.structural = true;
                        }
                        if (leftColumn)
                        {
                            c.left.structural = true;
                        }
                    }
                }

                Instantiate(rRaw, ((Vector2) roomPos), Quaternion.identity, transform);

                break;
            }
        }
    }

    void instantiateEntities()
    {
        foreach (WallBuilder w in g.walls) {
            if (!w.exists) {
                continue;
            }
            Instantiate(wall, w.getWorldCoords(), w.getRotation(), transform);
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

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
        int width = 6;
        this.g = new GridBuilder(new Vector2Int(width, width));
    }

    void generateLevel() {
        initGrid();

        // TODO
        Vector2Int [] roomPositions = {new Vector2Int(0, 0)};

        foreach (GameObject rRaw in roomTemplates) {
            Debug.Log("Attempting to place room..." + roomTemplates.Length);
            foreach (Vector2Int roomPos in roomPositions) {
                Debug.LogWarning("Trying p " + roomPos);

                RoomTemplate r = rRaw.GetComponent(typeof(RoomTemplate)) as RoomTemplate;
                if (r == null) {
                    Debug.LogWarning("expected only RoomTemplate components");
                    continue;
                }

                Vector2Int dimensions = (Vector2Int) r.getDimensions();
                if (r.doors
                    .Select(d => d.transform.position)
                    .Select(roomCoords => ((Vector2) roomPos) + ((Vector2) roomCoords))
                    .Select(levelCoords => g.wallPositionToBuilder(levelCoords))
                    .Any(d => d == null || d.structural))
                {
                    Debug.Log("Ineligible: Door would destroy structural wall");
                    // Ineligible: Door would destroy structural wall
                    continue;
                }

                if (g.traverseInteriorWalls(roomPos, dimensions).Any(w => w.structural)) {
                    Debug.Log("Ineligible: Room internals would destroy structural wall");
                    // Ineligible: Room internals would destroy structural wall
                    continue;
                }

                Debug.Log("Found eligible room placement");
                foreach (WallBuilder w in g.traverseExteriorWalls(roomPos, dimensions))
                {
                    w.structural = true;
                }
                foreach (WallBuilder w in g.traverseInteriorWalls(roomPos, dimensions))
                {
                    w.structural = true;
                    w.exists = false;
                }

                Instantiate(rRaw, ((Vector2) roomPos), Quaternion.identity, transform);

                break;
            }
        }
    }

    void instantiateEntities()
    {
        foreach (WallBuilder w in g.walls) {
            if (w == null || !w.exists) {
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

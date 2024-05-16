using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class NavMesh : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase[] walkableTiles;
    public Vector3Int targetTile;
    public float nodeSize = 1f;

    private void Start()
    {
        FindPathToTarget(transform.position, targetTile);
    }

    void FindPathToTarget(Vector3 startPosition, Vector3Int targetPosition)
    {
        Vector3Int startTile = WorldToTilemapPosition(startPosition);
        List<Vector3Int> path = AStarPathfinding(startTile, targetPosition);

        if (path != null)
        {
            // Do something with the path (e.g., move NPC along the path)
            foreach (Vector3Int tile in path)
            {
                Vector3 worldPos = TilemapToWorldPosition(tile);
                Debug.Log("Next Tile: " + tile + ", World Position: " + worldPos);
            }
        }
        else
        {
            Debug.Log("No path found.");
        }
    }

    List<Vector3Int> AStarPathfinding(Vector3Int startTile, Vector3Int targetTile)
    {
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        HashSet<Vector3Int> openSet = new HashSet<Vector3Int>();
        openSet.Add(startTile);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
        gScore[startTile] = 0;

        Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();
        fScore[startTile] = HeuristicCostEstimate(startTile, targetTile);

        while (openSet.Count > 0)
        {
            Vector3Int currentTile = GetLowestFScoreTile(openSet, fScore);
            if (currentTile == targetTile)
            {
                return ReconstructPath(cameFrom, currentTile);
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            foreach (Vector3Int neighbor in GetNeighbors(currentTile))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[currentTile] + DistanceBetween(currentTile, neighbor);

                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = currentTile;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, targetTile);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    float HeuristicCostEstimate(Vector3Int tileA, Vector3Int tileB)
    {
        return Mathf.Abs(tileA.x - tileB.x) + Mathf.Abs(tileA.y - tileB.y);
    }

    Vector3Int GetLowestFScoreTile(HashSet<Vector3Int> openSet, Dictionary<Vector3Int, float> fScore)
    {
        float lowestFScore = Mathf.Infinity;
        Vector3Int lowestTile = Vector3Int.zero;

        foreach (Vector3Int tile in openSet)
        {
            if (fScore.ContainsKey(tile) && fScore[tile] < lowestFScore)
            {
                lowestFScore = fScore[tile];
                lowestTile = tile;
            }
        }

        return lowestTile;
    }

    List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int currentTile)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(currentTile);

        while (cameFrom.ContainsKey(currentTile))
        {
            currentTile = cameFrom[currentTile];
            path.Add(currentTile);
        }

        path.Reverse();
        return path;
    }

    float DistanceBetween(Vector3Int tileA, Vector3Int tileB)
    {
        return Vector3Int.Distance(tileA, tileB);
    }

    List<Vector3Int> GetNeighbors(Vector3Int tile)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, -1, 0)
        };

        foreach (Vector3Int dir in directions)
        {
            Vector3Int neighbor = tile + dir;
            if (IsWalkable(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    bool IsWalkable(Vector3Int tile)
    {
        TileBase tileBase = tilemap.GetTile(tile);
        foreach (TileBase walkableTile in walkableTiles)
        {
            if (tileBase == walkableTile)
            {
                return true;
            }
        }
        return false;
    }

    Vector3Int WorldToTilemapPosition(Vector3 worldPosition)
    {
        Vector3Int tilemapPosition = tilemap.WorldToCell(worldPosition);
        return tilemapPosition;
    }

    Vector3 TilemapToWorldPosition(Vector3Int tilemapPosition)
    {
        Vector3 worldPosition = tilemap.GetCellCenterWorld(tilemapPosition);
        return worldPosition;
    }
}

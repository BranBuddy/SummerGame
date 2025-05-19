using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fleet : GridObject
{
    //Speed of the slowest ship in the fleet
    int speed;
    public int Speed { get { return speed; } }

    List<Ship> ships = new List<Ship>();
    public List<Ship> Ships { get { return ships; } }

    // Key: Tile: the tile to find information for
    // Value: (Tile, int)
    //      Tile: the previous tile in the path
    //      int: the distance of the tile from the source
    Dictionary<Tile, (Tile, int)> pathfindingTiles = new Dictionary<Tile, (Tile, int)>();

    //Stores all tiles that should be highlighted
    //Either by showing reachable tiles
    //or by showing a pathfinding trail
    List<Tile> tilesToHighlight = new List<Tile>();

    List<Tile> pathTrail = new List<Tile>();

    // (Tile: Tile being propagated, 
    //  int: range this tile can propagate to)
    Queue<(Tile, int)> rangeQueue = new Queue<(Tile, int)>();

    public Fleet()
    {
        objName = "Fleet";
        AddShip(new Ship("Ship", 2));
    }

    public override void OnSelected()
    {
        tilesToHighlight.Clear();
        FindInRange();

        foreach (Tile t in tilesToHighlight)
            t.Highlight();

    }

    public override void OnDeselected()
    {
        foreach (Tile t in tilesToHighlight)
            t.Unhighlight();
    }

    public void AddShip(Ship s)
    {
        ships.Add(s);
        RecalculateSpeed();
    }

    void RecalculateSpeed()
    {
        int min = int.MaxValue;
        foreach (Ship s in ships)
        {
            if (s.Speed < min)
                min = s.Speed;
        }

        speed = min;
        Debug.Log("new ship speed: " + speed);
    }

    void FindInRange()
    {
        //seed queue with fleet's parent tile
        Tile parent = gameObject.transform.parent.gameObject.GetComponent<Tile>();
        rangeQueue.Enqueue((parent, speed));

        while (rangeQueue.Count > 0)
        {
            (Tile, int) nextTile = rangeQueue.Dequeue();
            if (nextTile.Item2 <= 0) continue;

            foreach (Tile t in nextTile.Item1.GetAdjacentTiles())
            {
                if (!tilesToHighlight.Contains(t) && t != parent && !t.IsOccupied())
                {
                    tilesToHighlight.Add(t);
                    rangeQueue.Enqueue((t, nextTile.Item2 - 1));
                }
            }
        }

        Debug.Log("tiles nearby: " + tilesToHighlight.Count);
    }

    public override void Pathfind(Tile dest)
    {
        if (dest == null) return;
        
        rangeQueue.Clear();

        Tile parent = gameObject.transform.parent.gameObject.GetComponent<Tile>();
        rangeQueue.Enqueue((parent, 0));

        while (rangeQueue.Count > 0)
        {
            (Tile, int) nextTile = rangeQueue.Dequeue();

            foreach (Tile t in nextTile.Item1.GetAdjacentTiles())
            {
                if (!t.IsOccupied() && (!pathfindingTiles.ContainsKey(t) || pathfindingTiles[t].Item2 > nextTile.Item2 + 1))
                {
                    pathfindingTiles[t] = (nextTile.Item1, nextTile.Item2 + 1);
                    rangeQueue.Enqueue((t, nextTile.Item2 + 1));
                }
            }
        }

        if (pathfindingTiles.ContainsKey(dest))
        {
            foreach (Tile t in tilesToHighlight)
                t.Unhighlight();

            tilesToHighlight.Clear();

            Tile walk = dest;
            while (pathfindingTiles.ContainsKey(walk) && pathfindingTiles[walk].Item1 != null)
            {
                walk.Highlight();
                tilesToHighlight.Add(walk);
                walk = pathfindingTiles[walk].Item1;
            }
        }
    }
}

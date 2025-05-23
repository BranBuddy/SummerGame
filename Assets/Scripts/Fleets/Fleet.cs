using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Object on the grid that contains one or more ships
public class Fleet : GridObject
{
    //Speed of the slowest ship in the fleet
    int speed;
    public int Speed { get { return speed; } }

    //Contains all ships in the fleet
    List<Ship> ships = new List<Ship>();
    public List<Ship> Ships { get { return ships; } }

    // Key: Tile: the tile to find information for
    // Value: (Tile, int)
    //      Tile: the previous tile in the path
    //      int: the distance of the tile from the source
    // When finding the path to a tile this dictionary
    // is traversed like a linked list backwards to the source
    Dictionary<Tile, (Tile, int)> pathfindingTiles = new Dictionary<Tile, (Tile, int)>();

    //Stores all tiles that should be highlighted
    //Either by showing reachable tiles
    //or by showing a pathfinding trail
    List<Tile> tilesToHighlight = new List<Tile>();

    // Used with pathfinding and reachable tile algorithms
    Queue<(Tile, int)> rangeQueue = new Queue<(Tile, int)>();

    //Destination tile as marked by the pathfinding algorithm
    //Usually set to whatever tile is currently right clicked on,
    //But will be null if the target tile has no valid path,
    //or if you are attempting to path to the void
    Tile destination;
    public override Tile Destination { get { return destination; } }

    public Fleet()
    {
        objName = "Fleet";
        // While testing, give fleet one ship of speed 3
        // So that we can move the fleet
        // Fleets with 0 ships will not exist in end product
        AddShip(new Ship("Ship", 3));
    }

    //Ran when the parent tile is selected
    public override void OnSelected()
    {
        //Clear current highlighted tiles
        tilesToHighlight.Clear();
        //save all tiles that are within range
        FindInRange();

        //Highlight tiles within range
        foreach (Tile t in tilesToHighlight)
            t.Highlight(false); //false means we are displaying as blue, not red

    }

    //Ran when the parent tile is deselected
    public override void OnDeselected()
    {
        //Unhighlight whatever tiles we are currently highlighting
        foreach (Tile t in tilesToHighlight)
            t.Unhighlight();
    }

    //Used to add a ship to the fleet
    public void AddShip(Ship s)
    {
        ships.Add(s); //Add ship to list
        RecalculateSpeed(); //Find new slowest speed of fleet
    }

    //Sets proper fleet speed
    void RecalculateSpeed()
    {
        //Find slowest ship in our fleet
        int min = int.MaxValue;
        foreach (Ship s in ships)
        {
            if (s.Speed < min)
                min = s.Speed;
        }

        //set slowest ship speed to our fleet speed
        speed = min;
    }

    //Finds all reachable tiles within our fleet's range
    void FindInRange()
    {
        //seed queue with fleet's parent tile
        //(Tile, int): (current tile, remaining speed)
        Tile parent = GetParentTile();
        rangeQueue.Enqueue((parent, speed));

        while (rangeQueue.Count > 0)
        {
            //Pull tile/range pairing from end of queue
            (Tile, int) nextTile = rangeQueue.Dequeue();
            if (nextTile.Item2 <= 0) continue; //do not propagate if tile is at end of range

            foreach (Tile t in nextTile.Item1.GetAdjacentTiles()) //propagate to each neighboring tile
            {
                //only propagate if we have not checked the tile yet,
                //the tile is not the parent and the tile is open
                if (!tilesToHighlight.Contains(t) && t != parent && !t.IsOccupied())
                {
                    //add to list of tiles to highlight,
                    //and add tile to queue to be repropagated
                    tilesToHighlight.Add(t);
                    rangeQueue.Enqueue((t, nextTile.Item2 - 1));
                }
            }
        }
    }

    //Finds a path to the destination tile using Djikstra's
    //returns true if we have found a path
    //false if there is no path or if we did not give a valid tile
    public override bool Pathfind(Tile dest)
    {
        if (dest == null)
        {
            //if we pathfind to a null tile
            //reset destination and exit
            destination = null;
            return false;
        }

        rangeQueue.Clear(); //prep queue since other functions use it

        //Seed pathfinder with parent tile
        //(Tile, int): (current tile, distance from source)
        Tile parent = gameObject.transform.parent.gameObject.GetComponent<Tile>();
        rangeQueue.Enqueue((parent, 0));

        while (rangeQueue.Count > 0)
        {
            //Pull tile/range pairing from end of queue
            (Tile, int) nextTile = rangeQueue.Dequeue();

            foreach (Tile t in nextTile.Item1.GetAdjacentTiles()) //check each adjacent tile
            {
                //only propagate if the tile is open and either:
                //the tile has not been found yet or
                //the tile has been found but its saved distance is greater than our new optimal distance
                if (!t.IsOccupied() && (!pathfindingTiles.ContainsKey(t) || pathfindingTiles[t].Item2 > nextTile.Item2 + 1))
                {
                    //save tile to dictionary with its new distance, and its previous tile
                    pathfindingTiles[t] = (nextTile.Item1, nextTile.Item2 + 1);
                    rangeQueue.Enqueue((t, nextTile.Item2 + 1)); //repropagate tile
                }
            }
        }

        //hide and clear highlighted tiles
        foreach (Tile t in tilesToHighlight)
            t.Unhighlight();

        tilesToHighlight.Clear();

        //pathfindingTiles[dest] will have an entry
        //only if there is a valid path to the destination
        if (pathfindingTiles.ContainsKey(dest))
        {
            //walk the list from the destination tile back to its source
            Tile walk = dest;
            while (pathfindingTiles.ContainsKey(walk) && pathfindingTiles[walk].Item1 != null) //walk if the tile is reachable
            {
                //if the tile is reachable but out of our range, highlight it red
                //otherwise blue
                walk.Highlight(pathfindingTiles[dest].Item2 > speed);
                tilesToHighlight.Add(walk);
                //set walk to the next tile backwards
                walk = pathfindingTiles[walk].Item1;
            }

            //set destination var since we found a path
            destination = dest;
            return true;
        }
        else
        {
            //if we have not found a path to the tile
            //set destination to null
            destination = null;
            return false;
        }
    }

    //Move a fleet from one tile to another
    //returns the destination tile if the move was successful,
    //null if it was not
    public override Tile Move(Tile dest)
    {
        //only move if the destination is valid and within range
        if (dest != null && pathfindingTiles[dest].Item2 <= speed)
        {
            //set our parent tile to the destination,
            //and move the tile in space to the new parent
            SetParentTile(dest);
            gameObject.transform.position = GetParentTile().gameObject.transform.position + Vector3.up;

            //now that we are in a new location,
            //our old pathfinding info is invalid
            ClearDest();
            return dest;
        }

        ClearDest();
        return null;
    }

    //clears destination and highlights
    //usually used when moving fleet to a new tile
    void ClearDest()
    {
        destination = null;
        pathfindingTiles.Clear();
        foreach (Tile t in tilesToHighlight)
            t.Unhighlight();
        tilesToHighlight.Clear();
    }
}

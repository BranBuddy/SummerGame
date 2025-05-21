using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//represents a tile on the grid
public class Tile : MonoBehaviour
{
    //GameObject stored in the tile (ex. Fleet)
    public GridObject contains;
    public GridObject Contains { get { return contains; } }

    [SerializeField]
    //Object to display when tile is selected
    private GameObject selectedInd;

    //position of the fleet in (x,y) coordinates
    public Vector2Int pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Testing code, spawns fleets in ~25% of tiles
        if (UnityEngine.Random.value < .25f)
        {
            GameObject ghost = Instantiate(TileManager.instance.fleetGhost);
            //Fleets are attached to a tile by parenting it,
            //as well as setting the contains value
            ghost.transform.position = gameObject.transform.position;
            ghost.transform.parent = gameObject.transform;
            contains = ghost.GetComponent<Fleet>();
        }
    }

    //returns true if the tile is occupied and thus is an obstacle for pathfinding
    public bool IsOccupied() { return contains != null; }

    //returns coordinates of tiles adjancent to this one
    public Vector2Int[] GetAdjacentPoints()
    {
        //convert tile coordinates to cubic coordinates
        //our adjacency finder uses cubic bc it is easier to work w/
        CCube cCoord = CCube.ToCube(pos);
        Vector2Int[] points = new Vector2Int[6];

        //directions to check are saved in our CCube class
        //we add each direction to our current position
        //then convert it back into x/y coordinates
        int i = 0;
        foreach (CCube dir in CCube.Directions)
            points[i++] = (cCoord + dir).FromCube();

        return points;
    }

    public Tile[] GetAdjacentTiles()
    {
        //list of points in x/y coords
        Vector2Int[] points = GetAdjacentPoints();
        List<Tile> tiles = new List<Tile>();

        foreach (Vector2Int p in points)
        {
            //check bounds of the point
            if (p.x < 0 || p.x >= TileManager.instance.Width) continue;
            if (p.y < 0 || p.y >= TileManager.instance.Height) continue;

            //get tiles from tilemanager list
            tiles.Add(TileManager.instance.Tiles[p.x][p.y]);
        }

        return tiles.ToArray();
    }

    public void Select()
    {
        //run selection function on whatever object we select
        if (contains) contains.OnSelected();

        //enable selection display on the tile
        selectedInd.GetComponent<MeshRenderer>().material = SelectionManager.instance.SelectMat;
        selectedInd.SetActive(true);
    }

    public void Highlight(bool error)
    {
        //if error is true, render the indicator red, otherwise blue
        //usually used when we are pathfinding to a destination out of range

        selectedInd.GetComponent<MeshRenderer>().material = error ? SelectionManager.instance.NavErrorMat : SelectionManager.instance.NavHighlightMat;
        selectedInd.SetActive(true);
    }

    public void Unhighlight()
    {
        //disable indicator
        selectedInd.SetActive(false);
    }

    public void Deselect()
    {
        //run deselection function on our current object and disable the indicator
        if (contains) contains.OnDeselected();
        selectedInd.SetActive(false);
    }

    public void SetContains(GridObject obj)
    {
        contains = obj;
    }
}

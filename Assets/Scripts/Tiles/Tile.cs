using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //GameObject stored in the tile (ex. Fleet)
    public GridObject contains;
    public GridObject Contains {get {return contains;}}

    [SerializeField]
    //Object to display when tile is selected
    private GameObject selectedInd;

    public Vector2Int pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(UnityEngine.Random.value < .25f)
        {
            GameObject ghost = Instantiate(TileManager.instance.fleetGhost);
            ghost.transform.position = gameObject.transform.position;
            ghost.transform.parent = gameObject.transform;
            contains = ghost.GetComponent<Fleet>();
        }
    }

    //returns true if the tile is occupied and thus is an obstacle for pathfinding
    public bool IsOccupied() {return contains != null;}

    //returns coordinates of tiles adjancent to this one
    public Vector2Int[] GetAdjacentPoints()
    {
        CCube cCoord = CCube.ToCube(pos);
        Vector2Int[] points = new Vector2Int[6];

        int i = 0;
        foreach(CCube dir in CCube.Directions)
            points[i++] = (cCoord + dir).FromCube();
        
        return points;
    }

    public Tile[] GetAdjacentTiles()
    {
        Vector2Int[] points = GetAdjacentPoints();
        List<Tile> tiles = new List<Tile>();

        foreach (Vector2Int p in points)
        {
            Debug.Log(p);
            if (p.x < 0 || p.x >= TileManager.instance.Width) continue;
            if (p.y < 0 || p.y >= TileManager.instance.Height) continue;
            
            tiles.Add(TileManager.instance.Tiles[p.x][p.y]);
        }

        return tiles.ToArray();
    }

    public void Select()
    {
        if (contains) contains.OnSelected();
        selectedInd.GetComponent<MeshRenderer>().material = SelectionManager.instance.SelectMat;
        selectedInd.SetActive(true);
    }

    public void Highlight()
    {
        selectedInd.GetComponent<MeshRenderer>().material = SelectionManager.instance.NavHighlightMat;
        selectedInd.SetActive(true);
    }

    public void Unhighlight()
    {
        selectedInd.SetActive(false);
    }

    public void Deselect()
    {
        if (contains) contains.OnDeselected();
        selectedInd.SetActive(false);
    }
}

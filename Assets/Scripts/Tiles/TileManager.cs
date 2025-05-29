/*
    TileManager.cs
    
    Last edited by:
    Luke Cullen

    5/26/25
*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//Handles all tiles on the grid
public class TileManager : MonoBehaviour
{
    public static TileManager instance; //ingame instance of the class

    [SerializeField]
    //Stores tile prefab for reproduction
    private GameObject tileObject;

    //Temp. fleet object for testing
    public GameObject fleetGhost;

    //stores game manager prefab
    private GameObject gameManager;


    //Stores all tiles on the map
    private Tile[][] tiles;
    public Tile[][] Tiles { get { return tiles; } }

    //x/y Dimensions of the tile grid
    [SerializeField]
    private int width = 5;
    [SerializeField]
    private int height = 5;

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    void Awake()
    {
        //Singleton design pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        if (GameObject.Find("GameManager") != null) 
        {
            gameManager = GameObject.Find("GameManager");
            width = gameManager.GetComponent<SceneChange>().GetMapSize();
            height = gameManager.GetComponent<SceneChange>().GetMapSize();
        }
        //Generates a grid when the manager is created
        GenerateGrid();
    }

    //Generates a rectangular grid of tiles
    void GenerateGrid()
    {
        //initialize 2D tile array for easy tile access
        tiles = new Tile[width][];
        for (int i = 0; i < width; i++)
            tiles[i] = new Tile[height];

        //ingame size of the grid object, needed so that we can tile it
        float size = tileObject.transform.localScale.x;

        //iterate through each position on the grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newTile = Instantiate(tileObject);

                //finds offset of hex tiles from each other
                //uses odd-r, pointy top configuration
                //Reference: https://www.redblobgames.com/grids/hexagons/

                float xSize = Mathf.Sqrt(3) * size / 2;
                float ySize = size * 3f / 4f;
                Vector3 offset = new Vector3((xSize * x) + (y % 2 * xSize / 2), 0, ySize * y);

                //moves new tile to its proper position
                newTile.transform.position = gameObject.transform.position + offset;

                //save new tile in our tile array, tell it what its x/y position is
                tiles[x][y] = newTile.GetComponent<Tile>();
                newTile.GetComponent<Tile>().pos = new Vector2Int(x, y);
            }
        }
    }

}

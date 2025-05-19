using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    public Camera cam;

    [SerializeField]
    //Stores tile object for reproduction
    private GameObject tileObject;

    //Temp. fleet object for testing
    public GameObject fleetGhost;

    //Temp. UI object for testing
    public TextMeshProUGUI tileText;

    //Stores all tiles on the map
    private Tile[][] tiles;
    public Tile[][] Tiles {get;}

    public Tile selectedTile;

    //Dimensions of the tile grid
    [SerializeField]
    private int width = 5;
    [SerializeField]
    private int height = 5;

    public int Width {get;}
    public int Height {get;}

    void Awake()
    {
        //Singleton design pattern
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    
    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3 pos = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 3))
            {
                if(selectedTile) selectedTile.Deselect();
                selectedTile = hit.collider.gameObject.GetComponent<Tile>();
                selectedTile.Select();
                tileText.text = (selectedTile.IsOccupied() ? selectedTile.Contains.Name : "Empty") + " | "+selectedTile.pos;
            }
        }
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        tiles = new Tile[width][];
        for(int i = 0; i < width; i++)
            tiles[i] = new Tile[height];

        float w = tileObject.transform.localScale.x;
        float h = tileObject.transform.localScale.y;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GameObject newTile = Instantiate(tileObject);
                float xSize = Mathf.Sqrt(3) * w / 2;
                float ySize = h * 3f/4f;
                Vector3 offset = new Vector3((xSize * x) + (y%2 * xSize/2) , 0, ySize * y);
                newTile.transform.position = gameObject.transform.position + offset;

                tiles[x][y] = newTile.GetComponent<Tile>();
                newTile.GetComponent<Tile>().pos = new Vector2Int(x, y);
            }
        }
    }

}

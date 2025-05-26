/*
    SelectionManager.cs
    
    Last edited by:
    Luke Cullen

    5/26/25
*/
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//Handles selections made on the grid
public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance; //ingame instance of the class
    Tile selectedTile; //the tile currently selected

    //the currently active camera
    //used to raycast to tiles
    [SerializeField]
    private Camera cam;

    //testing UI, displays info abt the current selected tile
    [SerializeField]
    private TextMeshProUGUI tileText;

    //Material to display when a tile is selected
    [SerializeField]
    private Material selectMat;
    public Material SelectMat { get { return selectMat; } }

    //Material to display when a tile is reachable and in range
    [SerializeField]
    private Material navHighlightMat;
    public Material NavHighlightMat { get { return navHighlightMat; } }

    //Material to display when a tile is reachable but out of range
    [SerializeField]
    private Material navErrorMat;
    public Material NavErrorMat { get { return navErrorMat; } }

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

    // Update is called once per frame
    void Update()
    {
        //Select a tile on left click
        if (Mouse.current.leftButton.wasPressedThisFrame)
            if (selectedTile != GetCurTile())
                SelectTile(GetCurTile()); //this code runs deselect if we pass a null tile (void)
            else //deselect our current tile if we click on it again
                DeselectTile();

        //While holding right click, show a path
        //from the tile we are currently selecting
        //to the tile our mouse is hovering
        if (selectedTile && selectedTile.contains && Mouse.current.rightButton.isPressed)
        {
            //check if we have already found a path to the current tile before running 
            //(running djikstra's every frame is probably a bad idea...)
            if (GetCurTile() != selectedTile.contains.Destination)
                //find path to hovered tile, if hovering on void this clears the fleets destination 
                selectedTile.contains.Pathfind(GetCurTile());
        }

        //When right click is released and we have a selected tile
        //attempt to move a selected tile's unit to the tile we right clicked on
        if (selectedTile && selectedTile.contains && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            //make sure we are not moving to the same tile
            if (GetCurTile() != selectedTile)
                //move our unit to the hovered tile, then select the new tile
                //if move is unsuccessful this also clears our selection
                SelectTile(selectedTile.contains.Move(selectedTile.contains.Destination));

        }
    }

    //returns the current tile hovered by the cursor
    Tile GetCurTile()
    {
        //cast a ray from our camera to our mouse position
        Vector3 pos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(pos);

        //return tile under the cursor (layer mask of 3 only includes tiles)
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 3))
            return hit.collider.gameObject.GetComponent<Tile>();

        //return null if the raycast finds nothing
        return null;
    }

    //Selects the given tile
    void SelectTile(Tile tile)
    {
        if (tile == null)
        {
            //Deselect current tile if we are not given a valid one
            DeselectTile();
            return;
        }

        if (selectedTile) selectedTile.Deselect(); //run deselection function on the old tile
        selectedTile = tile; //swap selected tile
        selectedTile.Select(); //run selection function on the new tile
        //update UI to display the new tile's information
        tileText.text = (selectedTile.IsOccupied() ? selectedTile.Contains.Name : "Empty") + " | " + selectedTile.pos;
    }

    //Unselects the currently selected tile
    void DeselectTile()
    {
        if (selectedTile) selectedTile.Deselect(); //run deselection function on the old tile
        tileText.text = "No Tile Selected"; //update UI
        selectedTile = null; //clear selection
    }
}

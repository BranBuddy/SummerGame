using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;
    Tile selectedTile;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private TextMeshProUGUI tileText;

    [SerializeField]
    private Material selectMat;
    public Material SelectMat { get { return selectMat; } }

    [SerializeField]
    private Material navHighlightMat;
    public Material NavHighlightMat { get { return navHighlightMat; } }

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
        if (Mouse.current.leftButton.wasPressedThisFrame)
            SelectTile(GetCurTile());

        if (selectedTile && selectedTile.contains && Mouse.current.rightButton.wasPressedThisFrame)
            selectedTile.contains.Pathfind(GetCurTile());
    }

    Tile GetCurTile()
    {
        Vector3 pos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << 3))
            return hit.collider.gameObject.GetComponent<Tile>();

        return null;
    }

    void SelectTile(Tile tile)
    {
        if (tile == null) return;

        if (selectedTile) selectedTile.Deselect();
        selectedTile = tile;
        selectedTile.Select();
        tileText.text = (selectedTile.IsOccupied() ? selectedTile.Contains.Name : "Empty") + " | " + selectedTile.pos;
    }
}

using UnityEngine;

//wrapper class for objects that are placed on the grid
public class GridObject : MonoBehaviour
{
    //Name that is displayed when the object is selected
    protected string objName = "Object";
    public string Name { get { return objName; } }
    public virtual Tile Destination { get { return null; } }

    //Gets the tile this object belongs to
    public Tile GetParentTile()
    {
        return gameObject.transform.parent.gameObject.GetComponent<Tile>();
    }

    //Swaps parenting and values from current parent to a new one
    public void SetParentTile(Tile tile)
    {
        GetParentTile().SetContains(null);
        gameObject.transform.parent = tile.gameObject.transform;
        tile.SetContains(this);
    }

    public virtual void OnSelected()
    {

    }

    public virtual void OnDeselected()
    {

    }

    public virtual bool Pathfind(Tile dest)
    {
        return false;
    }

    public virtual Tile Move(Tile dest)
    {
        return dest;
    }
}
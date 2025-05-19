using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected string objName = "Object";
    public string Name { get { return objName; } }

    public virtual void OnSelected()
    {

    }

    public virtual void OnDeselected()
    {

    }

    public virtual void Pathfind(Tile dest)
    {

    }
}
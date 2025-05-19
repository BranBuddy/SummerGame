using UnityEngine;

public class Ship
{
    string shipName;
    int speed;

    public string Name {get{return shipName;}}
    public int Speed {get{ return speed; }}

    public Ship(string name, int speed)
    {
        this.shipName = name;
        this.speed = speed;
    }
}

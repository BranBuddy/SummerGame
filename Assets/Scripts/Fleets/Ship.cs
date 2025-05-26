/*
    Ship.cs
    
    Last edited by:
    Luke Cullen

    5/26/25
*/
using UnityEngine;

//Basic ship entity, only ingame when in a fleet
public class Ship
{
    //Name of the ship to be displayed
    string shipName;

    //The distance this ship can travel per turn in tiles
    //Fleets travel at the speed of the slowest ship no matter what
    int speed;

    public string Name { get { return shipName; } }
    public int Speed { get { return speed; } }

    public Ship(string name, int speed)
    {
        this.shipName = name;
        this.speed = speed;
    }
}

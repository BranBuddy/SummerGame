/*
    CCube.cs
    
    Last edited by:
    Luke Cullen

    5/26/25
*/
using UnityEngine;
using Unity.Collections;

//Represents cubic hexagonal coordinates
public class CCube
{
    public int q, r, s;
    public CCube()
    {
        q = r = s = 0;
    }

    public CCube(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
    }

    public static CCube operator +(CCube a, CCube b)
        => new CCube(a.q + b.q, a.r + b.r, a.s + b.s);

    public static CCube operator -(CCube a, CCube b)
        => new CCube(a.q - b.q, a.r - b.r, a.s - b.s);

    //Convert from x/y coordinates to cubic coordinates
    public static CCube ToCube(Vector2 coord)
    {
        int x = (int)coord.x;
        int y = (int)coord.y;
        int q = x - (y - (y & 1)) / 2;
        return new CCube(q, y, -q - y);
    }

    //Convert from cubic coordinates to x/y coordinates
    public Vector2Int FromCube()
    {
        return new Vector2Int(q + (r - (r & 1)) / 2, r);
    }

    //List of cubic directions to check when finding adjacent tiles
    private static CCube[] directions = {
        new CCube(1, 0, -1), new CCube(1, -1, 0), new CCube(0, -1, 1),
        new CCube(-1, 0, 1), new CCube(-1, 1, 0), new CCube(0, 1, -1),
    };

    public static CCube[] Directions { get { return directions; } }
}
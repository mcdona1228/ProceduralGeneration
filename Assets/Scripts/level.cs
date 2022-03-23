using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class level
{
    public abstract void startAt(location Location);
    public abstract location makeConnection(location Location);
}

public class location
{
    public int x;
    public int y;

    public location(int xCoords, int yCoords)
    {
        x = xCoords;
        y = yCoords;
    }
}

public class Connect
{
    public bool insideMaze;
    public bool[] directions = new bool[4] { false, false, false, false };

    public override string ToString()
    {
        string s = "InsideMaze = " + insideMaze + "; Connections: ";
        foreach(bool b in directions)
        {
            s += (b + " ");
        }
        return s;
    }
}

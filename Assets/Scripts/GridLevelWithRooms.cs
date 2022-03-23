using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevelWithRooms : GridLevel
{
    Stack<room> unplacedRooms;
    float chance_of_room = 0.9f;
    int iteration = 0;
    bool includeUnreachables = false;

    public GridLevelWithRooms(int width, int height) : base(width, height)
    {
        unplacedRooms = new Stack<room>();
        int numRooms = 20;
        for (int i=0; i <numRooms; i++)
        {
            room Room = new room();
            Room.width = (int)Random.Range(3f, 5.99f);
            Room.height = (int)Random.Range(3f, 5.99f);
            unplacedRooms.Push(Room);
        }
        if (includeUnreachables)
        {
            int numCells = width * height;
            int numUnreachable = (int)(numCells * 0.05f);
            for (int i = 0; i < numUnreachable; i++)
            {
                int x = (int)Random.Range(0f, width - 1);
                int y = (int)Random.Range(0f, height - 1);
                cells[x, y].insideMaze = true;
            }
        }
    }

    bool placeRoom(room Room, int x, int y)
    {
        bool inBound = (x >= 0) && (x < (mazeWidth - Room.width)) && (y >= 0) && (y < (mazeHeight - Room.height));
        if (!inBound)
        {
            return false;
        }
        
        for (int rx = x; rx < x + Room.width; rx++)
        {
            for(int  ry = y; ry < y + Room.height; ry++)
            {
                if (cells[rx, ry].insideMaze)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void addRoom(room Room, location Location)
    {
        for (int x = Location.x; x < Location.x + Room.width; x++)
        {
            for (int y = Location.y; y < Location.y + Room.height; y++)
            {
                cells[x, y].insideMaze = true;
                if(x != Location.x + Room.width - 1)
                {
                    cells[x, y].directions[0] = true;
                    cells[x + 1, y].directions[3] = true;
                }
                if(y != Location.y + Room.height - 1)
                {
                    cells[x, y].directions[1] = true;
                    cells[x + 1, y].directions[2] = true;
                }
            }
        }
    }

    public override location makeConnection(location Location)
    {
        iteration++;
        if (unplacedRooms.Count > 0 && iteration > 5 && (Random.Range(0f, 1.0f) < chance_of_room))
        {
            int x = Location.x;
            int y = Location.y;

            room Room = unplacedRooms.Peek();
            Vector3 v = neighbors[(int)Random.Range(0f, 3.99f)];

            int dx = (int)v.x;
            int dy = (int)v.y;
            int dirn = (int)v.z;

            int nx = x + dx;
            int ny = y + dy;
            if (dx < 0)
            {
                nx -= (Room.width - 1);
            }
            if (dy < 0)
            {
                ny -= (Room.height - 1);
            }

            if(placeRoom(Room, nx, ny))
            {
                unplacedRooms.Pop();
                addRoom(Room, new location(nx, ny));

                cells[x, y].directions[dirn] = true;
                cells[x + dx, y + dy].directions[3 - dirn] = true;

                return null;
            }
        }
        return base.makeConnection(Location);
    }
}

public class room
{
    public int width;
    public int height;
}
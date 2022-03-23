using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : level
{
    protected Vector3[] neighbors = new Vector3[4]
    {
        new Vector3(1,0,0),
        new Vector3(0,1,1),
        new Vector3(0,-1,2),
        new Vector3(-1,0,3)
    };

    protected int mazeWidth;
    protected int mazeHeight;
    public Connect[,] cells;

    public GridLevel(int width, int height)
    {
        mazeWidth = width;
        mazeHeight = height;
        cells = new Connect[width, height];
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j=0; j < mazeHeight; j++)
            {
                cells[i, j] = new Connect();
            }
        }
    }

    public override void startAt(location Location)
    {
        cells[Location.x, Location.y].insideMaze = true;
    }

    bool placeCorridor(int x, int y, int dirn)
    {
        return (x >= 0 && x < mazeWidth) && (y >= 0 && y < mazeHeight) && !cells[x, y].insideMaze;
    }

    void shuffleNeighbors()
    {
        int n = neighbors.Length;
        while (n > 1)
        {
            n--;
            int u = (int)Random.Range(0, n);
            Vector3 v = neighbors[u];
            neighbors[u] = neighbors[n];
            neighbors[n] = v;
        }
    }

    public override location makeConnection(location Location)
    {
        //throw new System.NotImplementedException();
        shuffleNeighbors();

        int x = Location.x;
        int y = Location.y;
        foreach(Vector3 v in neighbors)
        {
            int dx = (int)v.x;
            int dy = (int)v.y;
            int dirn = (int)v.z;

            int nx = x + dx;
            int ny = y + dy;
            int fromDirn = 3 - dirn;
            if (placeCorridor(nx, ny, fromDirn))
            {
                cells[x, y].directions[dirn] = true;
                cells[nx, ny].insideMaze = true;
                cells[nx, ny].directions[fromDirn] = true;

                return new location(nx, ny);
            }
        }
        return null; 
    }
}

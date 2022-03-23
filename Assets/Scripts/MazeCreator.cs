using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    public int mWidth;
    public int mHeight;
    public location mStart = new location(0, 0);
    bool hasExit = true;

    GridLevelWithRooms levelOne;
    GameObject wallPrefab;
    GameObject blockerPrefab;

    void Start()
    {
        wallPrefab = Resources.Load<GameObject>("Wall");
        blockerPrefab = Resources.Load<GameObject>("Blocker");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            foreach(GameObject wall in walls)
            {
                Destroy(wall);
            }
            mWidth = (int)Random.Range(10f, 30f);
            mHeight = (int)Random.Range(10f, 30f);
            mStart = new location((int)Random.Range(0f, mWidth - 1), 0);

            levelOne = new GridLevelWithRooms(mWidth, mHeight);
            generateMaze(levelOne, mStart);
            MakeDoor(mStart);
            if (hasExit)
            {
                int exitX = mWidth - 1 - mStart.x;
                int exitY = mHeight - 1 - mStart.y;
                location exit = new location(exitX, exitY);
                MakeDoor(exit);
            }
            Build();
        }
        if(levelOne != null)
        {
            for(int x = 0; x< mWidth; x++)
            {
                for (int y = 0; y < mHeight; y++)
                {
                    Connect currentCell = levelOne.cells[x, y];
                    if (currentCell.insideMaze)
                    {
                        Vector3 cellPos = new Vector3(x, 0, y);
                        float lineLength = 1f;
                        if (currentCell.directions[0])
                        {
                            Vector3 neighborPos = new Vector3(x + lineLength, 0, y);
                        }
                        if (currentCell.directions[1])
                        {
                            Vector3 neighborPos = new Vector3(x, 0, y + lineLength);
                        }
                        if (currentCell.directions[2])
                        {
                            Vector3 neighborPos = new Vector3(x, 0, y - lineLength);
                        }
                        if (currentCell.directions[3])
                        {
                            Vector3 neighborPos = new Vector3(x - lineLength, 0, y);
                        }
                    }
                }
            }
        }
    }

    void Build()
    {
        for (int x = 0; x < mWidth; x++)
        {
            for (int y = 0; y < mHeight; y++)
            {
                Connect currentCell = levelOne.cells[x, y];
                if (levelOne.cells[x, y].insideMaze)
                {
                    Vector3 cellPos = new Vector3(x, 0, y);
                    float lineLength = 1f;
                    if (!currentCell.directions[0])
                    {
                        Vector3 wallPos = new Vector3(x + lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                    if (!currentCell.directions[1])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y + lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                    if (y == 0 && !currentCell.directions[2])
                    {
                        Vector3 wallPos = new Vector3(x, 0, y - lineLength / 2);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                    }
                    if (x == 0 && !currentCell.directions[3])
                    {
                        Vector3 wallPos = new Vector3(x - lineLength / 2, 0, y);
                        GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity) as GameObject;
                    }
                }
                if (!currentCell.directions[0] && !currentCell.directions[1] && !currentCell.directions[2] && !currentCell.directions[3])
                {
                    GameObject blocker = Instantiate(blockerPrefab, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
                }
            }
        }
    }
    void MakeDoor(location Location)
    {
        Connect cell = levelOne.cells[Location.x, Location.y];
        // which connection to set to true?
        // directions are listed in this order: +x, +y, -y, -x
        if (Location.x == 0)
        {
            cell.directions[3] = true;
        }
        else if (Location.x == mWidth - 1)
        {
            cell.directions[0] = true;
        }
        else if (Location.y == 0)
        {
            cell.directions[2] = true;
        }
        else if (Location.y == mHeight - 1)
        {
            cell.directions[1] = true;
        }
    }

    // From Millington pg. 706. He calls this function just "maze"
    void generateMaze(level Level, location start)
    {
        // a stack of locations we can branch from
        Stack<location> locations = new Stack<location>();
        locations.Push(start);
        Level.startAt(start);

        while (locations.Count > 0)
        {
            location current = locations.Peek();

            // try to connect to a neighboring location
            location next = Level.makeConnection(current);
            if (next != null)
            {
                // if successful, it will be our next iteration
                locations.Push(next);
            }
            else
            {
                locations.Pop();
            }
        }
    }
}

           
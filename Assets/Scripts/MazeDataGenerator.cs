
using System;
using System.Collections.Generic;
using UnityEngine;
public class MazeDataGenerator
{
    public float placementThreshold;    // chance of empty space 
    public MazeDataGenerator()
    {
        placementThreshold = .1f;                               // 1 
    }
    public int[,] FromDimensions(int sizeRows, int sizeCols)
    {
        int[,] maze = new int[sizeRows, sizeCols];

        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                // 1 — outer border walls
                if (i == 0 || j == 0 || i == rMax || j == cMax)
                {
                    maze[i, j] = 1;
                }
                // 2 — interior random wall placement
                else if (i % 2 == 0 && j % 2 == 0)
                {
                    if (UnityEngine.Random.value > placementThreshold)
                    {
                        // 3 — place a wall and one adjacent wall
                        maze[i, j] = 1;
                        int a = UnityEngine.Random.value < .5f ? 0 : (UnityEngine.Random.value < .5f ? -1 : 1);
                        int b = a != 0 ? 0 : (UnityEngine.Random.value < .5f ? -1 : 1);
                        maze[i + a, j + b] = 1;
                    }
                }
            }
        }

        return maze;
    }

}

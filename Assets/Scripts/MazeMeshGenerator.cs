using System.Collections.Generic;
using UnityEngine;

public class MazeMeshGenerator
{
    // generator parameters
    public float width;   // how wide are hallways
    public float height;  // how tall are hallways

    public MazeMeshGenerator()
    {
        width = 3.75f;
        height = 3.5f;
    }

    public Mesh FromData(int[,] data)
    {
        Mesh maze = new Mesh();

        // Lists for mesh data
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> floorTriangles = new List<int>();
        List<int> wallTriangles = new List<int>();

        maze.subMeshCount = 2; // two submeshes: floor/ceiling and walls

        int rMax = data.GetUpperBound(0);
        int cMax = data.GetUpperBound(1);
        float halfH = height * 0.5f;

        // Loop through every cell in the maze
        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                // Only create geometry for empty spaces (not walls)
                if (data[i, j] != 1)
                {
                    // Floor
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, 0, i * width),
                        Quaternion.LookRotation(Vector3.up),
                        new Vector3(width, width, 1)),
                        ref vertices, ref uvs, ref floorTriangles);

                    // Ceiling
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, height, i * width),
                        Quaternion.LookRotation(Vector3.down),
                        new Vector3(width, width, 1)),
                        ref vertices, ref uvs, ref floorTriangles);

                    // Walls only if adjacent cell is blocked or outside bounds
                    if (i - 1 < 0 || data[i - 1, j] == 1) // forward wall
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, halfH, (i - 0.5f) * width),
                            Quaternion.LookRotation(Vector3.forward),
                            new Vector3(width, height, 1)),
                            ref vertices, ref uvs, ref wallTriangles);

                    if (j + 1 > cMax || data[i, j + 1] == 1) // left wall
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j + 0.5f) * width, halfH, i * width),
                            Quaternion.LookRotation(Vector3.left),
                            new Vector3(width, height, 1)),
                            ref vertices, ref uvs, ref wallTriangles);

                    if (j - 1 < 0 || data[i, j - 1] == 1) // right wall
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j - 0.5f) * width, halfH, i * width),
                            Quaternion.LookRotation(Vector3.right),
                            new Vector3(width, height, 1)),
                            ref vertices, ref uvs, ref wallTriangles);

                    if (i + 1 > rMax || data[i + 1, j] == 1) // back wall
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, halfH, (i + 0.5f) * width),
                            Quaternion.LookRotation(Vector3.back),
                            new Vector3(width, height, 1)),
                            ref vertices, ref uvs, ref wallTriangles);
                }
            }
        }

        // Assign generated data to the mesh
        maze.vertices = vertices.ToArray();
        maze.uv = uvs.ToArray();
        maze.SetTriangles(floorTriangles.ToArray(), 0); // submesh 0 = floor/ceiling
        maze.SetTriangles(wallTriangles.ToArray(), 1);  // submesh 1 = walls

        maze.RecalculateNormals(); // prepare for proper lighting

        return maze;
    }
    // Helper method to create a single quad (used for floor, ceiling, or wall)
    private void AddQuad(Matrix4x4 matrix, ref List<Vector3> vertices, ref List<Vector2> uvs, ref List<int> triangles)
    {
        int index = vertices.Count;

        // Define quad corners in local space
        Vector3 vert1 = new Vector3(-0.5f, -0.5f, 0);
        Vector3 vert2 = new Vector3(-0.5f, 0.5f, 0);
        Vector3 vert3 = new Vector3(0.5f, 0.5f, 0);
        Vector3 vert4 = new Vector3(0.5f, -0.5f, 0);

        // Apply the transformation matrix
        vertices.Add(matrix.MultiplyPoint3x4(vert1));
        vertices.Add(matrix.MultiplyPoint3x4(vert2));
        vertices.Add(matrix.MultiplyPoint3x4(vert3));
        vertices.Add(matrix.MultiplyPoint3x4(vert4));

        // Add UVs
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(0, 0));

        // Add triangles
        triangles.Add(index + 2);
        triangles.Add(index + 1);
        triangles.Add(index);

        triangles.Add(index + 3);
        triangles.Add(index + 2);
        triangles.Add(index);
    }
}

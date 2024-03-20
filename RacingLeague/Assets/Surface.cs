using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    int widthInSquares = 4;
    int lengthInSquares = 8;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Color[] colors;

    private void Awake()
    {
        vertices = new Vector3[4 * widthInSquares * lengthInSquares];
        triangles = new int[6 * widthInSquares * lengthInSquares];
        uvs = new Vector2[4 * widthInSquares * lengthInSquares];
        colors = new Color[2* widthInSquares * lengthInSquares];
    }

    private void Start()
    {
        SetMeshData();
        SetTrackColor();
        CreateMesh();
    }

    void SetMeshData()
    {
        int countVerts = 0;
        int countTris = 0;

        for (int x = 0; x < widthInSquares; x++)
        {
            for (int z = 0; z < lengthInSquares; z++)
            {
                vertices[countVerts] = new Vector3(x, 0, z);
                vertices[countVerts + 1] = new Vector3(x, 0, z + 1);
                vertices[countVerts + 2] = new Vector3(x + 1, 0, z + 1);
                vertices[countVerts + 3] = new Vector3(x + 1, 0, z);

                triangles[countTris] = countVerts;
                triangles[countTris + 1] = countVerts + 1;
                triangles[countTris + 2] = countVerts + 3;
                triangles[countTris + 3] = countVerts + 3;
                triangles[countTris + 4] = countVerts + 1;
                triangles[countTris + 5] = countVerts + 2;

                float uvWidth = 1.0f / widthInSquares;
                float uvLength = 1.0f / lengthInSquares;

                uvs[countVerts] = new Vector2(x * uvWidth, z * uvLength);
                uvs[countVerts + 1] = new Vector2(x * uvWidth, (z + 1) * uvLength);
                uvs[countVerts + 2] = new Vector2((x + 1) * uvWidth, (z + 1) * uvLength);
                uvs[countVerts + 3] = new Vector2((x + 1) * uvWidth, z * uvLength);

                countVerts += 4;
                countTris += 6;
            }
        }

    }

    void SetTrackColor()
    {

        colors[0] = Color.white;
        colors[1] = Color.yellow;
        colors[2] = Color.green;
        colors[3] = Color.blue;
        colors[4] = Color.blue;
        colors[5] = Color.blue;
        colors[6] = Color.blue;
        colors[7] = Color.blue;
        colors[8] = Color.white;
        colors[9] = Color.blue;
        colors[10] = Color.blue;
        colors[11] = Color.black;

    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();

        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        //Color-----
        Texture2D texture = new Texture2D(widthInSquares, lengthInSquares, TextureFormat.RGBA32, false);

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colors);
        texture.Apply();

        Color32[] test = texture.GetPixels32();

        gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
    }

}

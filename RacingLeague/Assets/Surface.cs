using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] private int widthInSquares = 10;
    [SerializeField] private int lenghtInSquares = 6;

    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Color[] colors;

    private void Awake()
    {
        vertices = new Vector3[4 * widthInSquares * lenghtInSquares];
        triangles = new int[6 * widthInSquares * lenghtInSquares];
        uvs = new Vector2[4 * widthInSquares * lenghtInSquares];
        colors = new Color[4 * widthInSquares * lenghtInSquares];
    }

    private void Start()
    {
        SetMeshData();
        CreateMesh();
    }

    void SetMeshData()
    {
        int countVerts = 0;
        int countTris = 0;

        for (int x = 0; x < widthInSquares; x++)
        {
            for (int z = 0; z < lenghtInSquares; z++)
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

                uvs[countVerts] = new Vector2(x, z);
                uvs[countVerts + 1] = new Vector2(x, z + 1);
                uvs[countVerts + 2] = new Vector2(x + 1, z + 1);
                uvs[countVerts + 3] = new Vector2(x + 1, z);

                colors[countVerts] = Color.green;
                colors[countVerts + 1] = Color.green;
                colors[countVerts + 2] = Color.green;
                colors[countVerts + 3] = Color.green;

                countVerts += 4;
                countTris += 6;
            }
        }

    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateBounds();

        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;

        ////TEST
        //colors[4] = Color.gray;
        //colors[5] = Color.gray;
        //colors[6] = Color.gray;
        //colors[7] = Color.gray;

        //Color-----
        Texture2D texture = new Texture2D(widthInSquares, lenghtInSquares, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colors);
        texture.Apply();
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;
    }

}

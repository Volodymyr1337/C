using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MainMap : Photon.MonoBehaviour
{
    public int size_X = 10;
    public int size_Y = 10;

    public float tileSize = 1f;

    int[,] colorArray;

    private void Awake()
    {
        colorArray = new int[size_X, size_Y];
        for (int j = 0; j < size_Y; j++)
            for (int i = 0; i < size_X; i++)
            {
                colorArray[i, j] = -1;
            }

    }


    private void Start()
    {
        BuildMesh();
    }

    void BuildMesh()
    {
        int numTiles = size_X * size_Y;
        int numTris = numTiles * 2;        // кол-во треугольников

        int vsize_X = size_X + 1;           // кол-во точек по Х и Y
        int vsize_Y = size_Y + 1;
        int numVerts = vsize_X * vsize_Y;

        // Generate mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];


        int[] triangles = new int[numTris * 3];

        int x, y;
        for (y = 0; y < vsize_Y; y++)
            for (x = 0; x < vsize_X; x++)
            {
                vertices[y * vsize_X + x] = new Vector3(x * tileSize, y * tileSize, 0);
                normals[y * vsize_X + x] = Vector3.up;
                uv[y * vsize_X + x] = new Vector2((float)x / size_X, (float)y / size_Y);
            }

        for (y = 0; y < size_Y; y++)
            for (x = 0; x < size_X; x++)
            {
                int squareIndex = y * size_X + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = y * vsize_X + x + 0;
                triangles[triOffset + 1] = y * vsize_X + x + vsize_X + 0;
                triangles[triOffset + 2] = y * vsize_X + x + vsize_X + 1;

                triangles[triOffset + 3] = y * vsize_X + x + 0;
                triangles[triOffset + 4] = y * vsize_X + x + vsize_X + 1;
                triangles[triOffset + 5] = y * vsize_X + x + 1;
            }

        // цепляем нашу сетку на меш
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
        mesh_collider.sharedMesh = mesh;

        BuildTexture();
    }

    void BuildTexture()
    {
        Texture2D texture = new Texture2D(size_X, size_Y);
        for (int y = 0; y < size_Y; y++)
            for (int x = 0; x < size_X; x++)
            {
                Color col = Color.green;
                texture.SetPixel(x, y, col);

            }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();        // apply all previus changes

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture;
    }

    public void MainFieldUpdater(int x, int y, bool hit)
    {
        Texture2D texture = new Texture2D(size_X, size_Y);
        for (int j = 0; j < size_Y; j++)
            for (int i = 0; i < size_X; i++)
            {
                Color col = colorArray[i, j] == -1 ? Color.green : colorArray[i, j] == 1 ? Color.red : Color.blue;
                if (i == x && j == y)
                {
                    colorArray[i, j] = hit ? 1 : 0;
                    col = hit ? Color.red : Color.blue;
                }
                texture.SetPixel(i, j, col);
            }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();        // apply all previus changes

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture;
    }
}

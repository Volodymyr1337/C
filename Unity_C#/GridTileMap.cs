using System.Collections;
using UnityEngine;


// По умолчанию скрипты-компоненты выполняются только в режиме игры. 
// Добавив этот атрибут, каждый компонент будет также вызывать функции когда редактор не находится в режиме игры.
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]  // Автоматически этому объекту добавится компонент
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class GridTileMap : MonoBehaviour
{
    public int size_X = 50;
    public int size_Z = 50;

    public float tileSize = 1f;

    // ТЕКСТУРЫ ВЫСОКОГО  РАЗРЕШЕНИЯ НЕ ЗАГРУЖАТЬ!!! - ЗАВИСНЕТ НАХУЙ 128/(макс 256) в зависимости от размеров карты самое то
    public Texture2D tileTexture;   
    int tileResolution;

    private void Start()
    {
        BuildMesh();
    }

    public void BuildMesh()
    {
        int numTiles = size_X * size_Z;
        int numTris = numTiles * 2;        // кол-во треугольников

        int vsize_X = size_X + 1;           // кол-во точек по Х и Z. напрмер:
        int vsize_Z = size_Z + 1;           // 'для постройки одного полгона нужно 4 точки'
        int numVerts = vsize_X * vsize_Z;

        // Generate mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];


        int[] triangles = new int[numTris * 3];

        int x, z;
        for (z = 0; z < vsize_Z; z++)
            for (x = 0; x < vsize_X; x++)
            {
                vertices[z * vsize_X + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * vsize_X + x] = Vector3.up;
                uv[z * vsize_X + x] = new Vector2((float)x / vsize_X, (float)z / vsize_Z);
            }

        for (z = 0; z < size_Z; z++)
            for (x = 0; x < size_X; x++)
            {
                int squareIndex = z * size_X + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = z * vsize_X + x + 0;
                triangles[triOffset + 1] = z * vsize_X + x + vsize_X + 0;
                triangles[triOffset + 2] = z * vsize_X + x + vsize_X + 1;

                triangles[triOffset + 3] = z * vsize_X + x + 0;
                triangles[triOffset + 4] = z * vsize_X + x + vsize_X + 1;
                triangles[triOffset + 5] = z * vsize_X + x + 1;
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
        if (tileTexture.height > 256)
        {
            Debug.Log("Too high texture resolution!!!");
            return;
        }
        tileResolution = tileTexture.height;
        Debug.Log(tileTexture.height);
        int w = size_X * tileResolution;
        int h = size_Z * tileResolution;

        Texture2D texture = new Texture2D(w, h);
        for (int y = 0; y < size_Z; y++)
            for (int x = 0; x < size_X; x++)
            {
                //Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));    // просто задает цвет
                //texture.SetPixels(x, y, с);
                Color[] terrain = tileTexture.GetPixels(0, 0, tileResolution, tileResolution);                // записывает текстуру в массив цветных пикселей
                texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, terrain);
                
            }
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();        // apply all previus changes

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture;
    }
}

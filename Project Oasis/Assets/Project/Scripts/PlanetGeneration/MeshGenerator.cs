using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector2[] uv;
    private Color[] colors;

    public int xSize, ySize;

    [Range(1, 200)]
    public int Resolution = 10;
    private int m_iCurrentResolution = 0;

    private void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }
        Refresh();
    }

    public void Refresh()
    {
        if (Resolution != m_iCurrentResolution)
        {
            Debug.Log("Resolution Change: " + Resolution);
            CreateSquareGrid();
        }
    }

    private void CreateSquareGrid()
    {
        m_iCurrentResolution = Resolution;
        mesh.Clear(); //TODO: Investigate a better solution

        vertices = new Vector3[(m_iCurrentResolution + 1) * (m_iCurrentResolution + 1)];
        normals = new Vector3[vertices.Length];
        uv = new Vector2[vertices.Length];

        float stepSize = 1f / m_iCurrentResolution;
        for (int v = 0, z = 0; z <= m_iCurrentResolution; z++)
        {
            for (int x = 0; x <= m_iCurrentResolution; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize - 0.5f, 0f, z * stepSize - 0.5f);
                normals[v] = Vector3.up;
                uv[v] = new Vector2(x * stepSize, z * stepSize);
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;

        int[] triangles = new int[m_iCurrentResolution * m_iCurrentResolution * 6];
        for (int t = 0, v = 0, y = 0; y < m_iCurrentResolution; y++, v++)
        {
            for (int x = 0; x < m_iCurrentResolution; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + m_iCurrentResolution + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + m_iCurrentResolution + 1;
                triangles[t + 5] = v + m_iCurrentResolution + 2;
            }
        }
        mesh.triangles = triangles;

    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}

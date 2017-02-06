using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class IcosahedronGenerator : MonoBehaviour
{
    [Range(0, 5)]
    public int Recursions = 0;
    public int Radius = 1;

    protected int m_iCurrentRecursions = 0;

    protected List<Vector3> m_vVerticesList = new List<Vector3>();
    protected List<Vector3> m_vTrianglesList = new List<Vector3>();

    protected Vector3[] m_vTriangles = new Vector3[1];
    protected Vector3[] m_vVertices = new Vector3[3];
    protected Vector3[] m_vNormals = new Vector3[3];

    protected Mesh mesh;

    protected int[] triangles;
    protected int idx = 0;

    public virtual void Create()
    {
        if (Recursions != m_iCurrentRecursions)
        {
            m_iCurrentRecursions = Recursions;
            Debug.Log("Recursions Change: " + Recursions);
            Create(m_iCurrentRecursions);
        }
    }

    private void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }

        Create();
    }

    /*====================================================
    Create
        desc
    params: int - how many times we want to recurse
    return: none
    //*///================================================
    public virtual void Create(int recursionLevel)
    {
        //create mesh
        mesh = new Mesh();
        mesh.name = "Ico Mesh";
        GetComponent<MeshFilter>().mesh = mesh;

        // create 12 vertices of a icosahedron
        ResetIcosahedron();

        if (Recursions <= 0)
        {
            return;
        }

        // refine triangles
        for (int i = 0; i < Recursions; ++i)
        {
            m_vTrianglesList = new List<Vector3>();

            for (int j = 0; j < m_vTriangles.Length; ++j)
            {
                Vector3 tri = m_vTriangles[j];
                // replace triangle by 4 triangles
                int a = getMiddlePoint((int)tri.x, (int)tri.y);
                int b = getMiddlePoint((int)tri.y, (int)tri.z);
                int c = getMiddlePoint((int)tri.z, (int)tri.x);

                m_vTrianglesList.Add(new Vector3(tri.x, a, c));
                m_vTrianglesList.Add(new Vector3(tri.y, b, a));
                m_vTrianglesList.Add(new Vector3(tri.z, c, b));
                m_vTrianglesList.Add(new Vector3(a, b, c));
            }

            //Make lists arrays
            m_vVertices = new Vector3[m_vVerticesList.Count];
            for (int j = 0; j < m_vVerticesList.Count; ++j)
            {
                m_vVertices[j] = m_vVerticesList[j].normalized * Radius;
            }

            m_vTriangles = new Vector3[m_vTrianglesList.Count];
            for (int j = 0; j < m_vTrianglesList.Count; ++j)
            {
                m_vTriangles[j] = m_vTrianglesList[j];
            }
            VectorArrayToIntArray(m_vTriangles);
        }

        //make arrays 
        m_vNormals = new Vector3[m_vVertices.Length];
        for (int i = 0; i < m_vVertices.Length; ++i)
        {
            m_vNormals[i] = Vector3.up;
        }

        mesh.vertices = m_vVertices;
        mesh.normals = m_vNormals;
        mesh.triangles = triangles;
    }

    /*====================================================
    addVertex
        add vertex to mesh, fix position to be on unit sphere, return index
    params: int - how many times we want to recurse
    return: none
    //*///================================================
    private int addVertex(Vector3 p)
    {
        m_vVerticesList.Add(p);
        return idx++;
    }//*/

    /*====================================================
    getMiddlePoint
        return index of point in the middle of p1 and p2
    params: int - how many times we want to recurse
    return: none
    //*///================================================
    private int getMiddlePoint(int p1, int p2)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        Int64 smallerIndex = firstIsSmaller ? p1 : p2;
        Int64 greaterIndex = firstIsSmaller ? p2 : p1;

        // not in cache, calculate it
        Vector3 point1 = m_vVerticesList[p1];
        Vector3 point2 = m_vVerticesList[p2];
        Vector3 middle = new Vector3(
            (point1.x + point2.x) / 2.0f,
            (point1.y + point2.y) / 2.0f,
            (point1.z + point2.z) / 2.0f);

        // add vertex makes sure point is on unit sphere
        int i = addVertex(middle);

        // store it, return index
        return i;
    }//*/

    private void VectorArrayToIntArray(Vector3[] vVector)
    {
        triangles = new int[vVector.Length * 3];
        for (int i = 0; i < vVector.Length; ++i)
        {
            triangles[i * 3] = (int)vVector[i].x;
            triangles[1 + (i * 3)] = (int)vVector[i].y;
            triangles[2 + (i * 3)] = (int)vVector[i].z;
        }
    }

    /*====================================================
    ResetIcosahedron
        desc
    params: int - how many times we want to recurse
    return: none
    //*///================================================
    private void ResetIcosahedron()
    {
        float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;
        //Debug.Log("Icosahedron: ResetIcosahedron: t: " + t);

        m_vVertices = new Vector3[12];
        m_vVertices[0] = new Vector3(-1, t, 0);
        m_vVertices[1] = new Vector3(1, t, 0);
        m_vVertices[2] = new Vector3(-1, -t, 0);
        m_vVertices[3] = new Vector3(1, -t, 0);

        m_vVertices[4] = new Vector3(0, -1, t);
        m_vVertices[5] = new Vector3(0, 1, t);
        m_vVertices[6] = new Vector3(0, -1, -t);
        m_vVertices[7] = new Vector3(0, 1, -t);

        m_vVertices[8] = new Vector3(t, 0, -1);
        m_vVertices[9] = new Vector3(t, 0, 1);
        m_vVertices[10] = new Vector3(-t, 0, -1);
        m_vVertices[11] = new Vector3(-t, 0, 1);
        m_vVerticesList = new List<Vector3>(m_vVertices);

        //Triangles to Vector3 Vertices
        triangles = new int[]
        {
            // 5 faces around point 0
            0, 11, 5,
            0, 5, 1,
            0, 1, 7,
            0, 7, 10,
            0, 10, 11,

            // 5 adjacent faces
            1, 5, 9,
            5, 11, 4,
            11, 10, 2,
            10, 7, 6,
            7, 1, 8,

            // 5 faces around point 3
            3, 9, 4,
            3, 4, 2,
            3, 2, 6,
            3, 6, 8,
            3, 8, 9,

            // 5 adjacent faces
            4, 9, 5,
            2, 4, 11,
            6, 2, 10,
            8, 6, 7,
            9, 8, 1
    };
        m_vTriangles = new Vector3[20];
        m_vTriangles[0] = new Vector3(0, 11, 5);
        m_vTriangles[1] = new Vector3(0, 5, 1);
        m_vTriangles[2] = new Vector3(0, 1, 7);
        m_vTriangles[3] = new Vector3(0, 7, 10);
        m_vTriangles[4] = new Vector3(0, 10, 11);

        // 5 adjacent faces
        m_vTriangles[5] = new Vector3(1, 5, 9);
        m_vTriangles[6] = new Vector3(5, 11, 4);
        m_vTriangles[7] = new Vector3(11, 10, 2);
        m_vTriangles[8] = new Vector3(10, 7, 6);
        m_vTriangles[9] = new Vector3(7, 1, 8);

        // 5 faces around point 3
        m_vTriangles[10] = new Vector3(3, 9, 4);
        m_vTriangles[11] = new Vector3(3, 4, 2);
        m_vTriangles[12] = new Vector3(3, 2, 6);
        m_vTriangles[13] = new Vector3(3, 6, 8);
        m_vTriangles[14] = new Vector3(3, 8, 9);

        // 5 adjacent faces
        m_vTriangles[15] = new Vector3(4, 9, 5);
        m_vTriangles[16] = new Vector3(2, 4, 11);
        m_vTriangles[17] = new Vector3(6, 2, 10);
        m_vTriangles[18] = new Vector3(8, 6, 7);
        m_vTriangles[19] = new Vector3(9, 8, 1);
        m_vTrianglesList = new List<Vector3>(m_vTriangles);
        
        idx = m_vVerticesList.Count;

        m_vNormals = new Vector3[m_vVertices.Length];
        for (int i = 0; i < m_vVertices.Length; ++i)
        {
            m_vVertices[i] = m_vVertices[i].normalized * Radius;
            m_vNormals[i] = Vector3.up;
        }

        mesh.vertices = m_vVertices;
        mesh.normals = m_vNormals;
        mesh.triangles = triangles;
    }
}
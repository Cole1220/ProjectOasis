using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcosahedronChunkGenerator : MonoBehaviour
{
    [Range(0, 7)]
    public int Recursions = 0;
    public int Radius = 1;

    protected int m_iCurrentRecursions = -1;
    
    protected List<Vector3> m_vVerticesList = new List<Vector3>();
    protected List<Vector3> m_vTrianglesList = new List<Vector3>();

    protected Vector3[] m_vTotalTriangles = new Vector3[1];
    protected Vector3[] m_vTotalVertices = new Vector3[3];
    protected Vector3[] m_vTotalNormals = new Vector3[3];

    protected Vector3[][] m_vTriangles = new Vector3[20][];
    protected Vector3[][] m_vVertices = new Vector3[20][];
    protected Vector3[][] m_vNormals = new Vector3[20][];

    protected int[] m_itotalTriangles;
    protected int[][] triangles;
    protected int idx = 0;

    protected GameObject[] faces;
    protected Mesh[] mesh;

    public virtual void Create()
    {
        if (Recursions != m_iCurrentRecursions)
        {
            m_iCurrentRecursions = Recursions;
            //Debug.Log("Recursions Change: " + Recursions);
            Create(m_iCurrentRecursions);
        }
    }

    private void OnEnable()
    {
        if (faces == null)
        {
            Debug.Log("Faces Null");
            faces = new GameObject[20];
            GameObject currentGameObject;
            for(int i = 0; i < 20; ++i)
            {
                currentGameObject = new GameObject();
                currentGameObject.name = "Face_" + i;
                currentGameObject.AddComponent<MeshFilter>();
                currentGameObject.AddComponent<MeshRenderer>();

                currentGameObject.transform.parent = this.transform;
                faces[i] = currentGameObject;
            }
        }

        if (mesh == null)
        {
            Debug.Log("Mesh Null");
            mesh = new Mesh[20];
            for (int i = 0; i < 20; ++i)
            {
                mesh[i] = new Mesh();
                mesh[i].name = "Surface Mesh";
                faces[i].GetComponent<MeshFilter>().mesh = mesh[i];
            }
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
        // create 12 vertices of a icosahedron
        CreateInitialIcosahedron();

        if (Recursions <= 0)
        {
            return;
        }

        //recurse for just one face
        Vector3 tri = new Vector3();
        for (int rec = 0; rec < Recursions; ++rec)
        {
            for (int i = 0; i < 20; ++i)
            {
                m_vTrianglesList = new List<Vector3>();
                m_vVerticesList = new List<Vector3>(m_vVertices[i]);

                idx = m_vVerticesList.Count;

                for (int j = 0; j < m_vTriangles[i].Length; ++j)
                {
                    tri = m_vTriangles[i][j];
                    // replace triangle by 4 triangles
                    int a = getMiddlePoint((int)tri.x, (int)tri.y);
                    int b = getMiddlePoint((int)tri.y, (int)tri.z);
                    int c = getMiddlePoint((int)tri.z, (int)tri.x);

                    m_vTrianglesList.Add(new Vector3(tri.x, a, c));
                    m_vTrianglesList.Add(new Vector3(tri.y, b, a));
                    m_vTrianglesList.Add(new Vector3(tri.z, c, b));
                    m_vTrianglesList.Add(new Vector3(a, b, c));
                }

                m_vVertices[i] = new Vector3[m_vVerticesList.Count];
                m_vNormals[i] = new Vector3[m_vVerticesList.Count];
                for (int j = 0; j < m_vVerticesList.Count; ++j)
                {
                    m_vVertices[i][j] = m_vVerticesList[j].normalized * Radius;
                    m_vNormals[i][j] = Vector3.up;
                }

                m_vTriangles[i] = new Vector3[m_vTrianglesList.Count];
                for (int j = 0; j < m_vTrianglesList.Count; ++j)
                {
                    m_vTriangles[i][j] = m_vTrianglesList[j];
                    //Debug.Log("m_vTriangles[i][j]: " + m_vTriangles[i][j]);
                }
                VectorArrayToIntArray(i, m_vTriangles[i]);

                mesh[i].vertices = m_vVertices[i];
                mesh[i].normals = m_vNormals[i];
                mesh[i].triangles = triangles[i];
                faces[i].GetComponent<MeshFilter>().mesh = mesh[i];
            }
        }
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

    //*
    private void VectorArrayToIntArray(int triIndex, Vector3[] vVector)
    {
        triangles[triIndex] = new int[vVector.Length * 3];
        for (int i = 0; i < vVector.Length; ++i)
        {
            triangles[triIndex][i * 3] = (int)vVector[i].x;
            triangles[triIndex][1 + (i * 3)] = (int)vVector[i].y;
            triangles[triIndex][2 + (i * 3)] = (int)vVector[i].z;
        }
    }//*/

    /*====================================================
    CreateInitialIcosahedron
        desc
    params: none
    return: none
    //*///================================================
    private void CreateInitialIcosahedron()
    {
        //Reset Mesh
        for (int i = 0; i < 20; ++i)
        {
            mesh[i] = new Mesh();
            mesh[i].name = "Surface Mesh";
            faces[i].GetComponent<MeshFilter>().mesh = mesh[i];
        }

        //Get all of our data first
        float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;
        //Debug.Log("Icosahedron: ResetIcosahedron: t: " + t);

        //Data on vertices
        m_vTotalVertices = new Vector3[12];
        m_vTotalVertices[0] = new Vector3(-1, t, 0);
        m_vTotalVertices[1] = new Vector3(1, t, 0);
        m_vTotalVertices[2] = new Vector3(-1, -t, 0);
        m_vTotalVertices[3] = new Vector3(1, -t, 0);
        
        m_vTotalVertices[4] = new Vector3(0, -1, t);
        m_vTotalVertices[5] = new Vector3(0, 1, t);
        m_vTotalVertices[6] = new Vector3(0, -1, -t);
        m_vTotalVertices[7] = new Vector3(0, 1, -t);
        
        m_vTotalVertices[8] = new Vector3(t, 0, -1);
        m_vTotalVertices[9] = new Vector3(t, 0, 1);
        m_vTotalVertices[10] = new Vector3(-t, 0, -1);
        m_vTotalVertices[11] = new Vector3(-t, 0, 1);
        //m_vVerticesList = new List<Vector3>(m_vTotalVertices);

        //Data on normals
        m_vTotalNormals = new Vector3[m_vTotalVertices.Length];
        for (int i = 0; i < m_vTotalVertices.Length; ++i)
        {
            m_vTotalNormals[i] = Vector3.up;
        }

        //Data on triangles
        //Triangles to Vector3 Vertices
        m_itotalTriangles = new int[]
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


        int trianglePointCount = 3;
        int triangleCount = m_itotalTriangles.Length / 3;
        int currentTrianglePoint = 0;
        triangles = new int[20][];
        for (int i = 0; i < triangleCount; i++)
        {
            m_vVertices[i] = new Vector3[trianglePointCount];
            m_vNormals[i] = new Vector3[trianglePointCount];
            triangles[i] = new int[] { 0, 1, 2 };
            m_vTriangles[i] = new Vector3[1];
            m_vTriangles[i][0] = new Vector3(0, 1, 2);

            for (int j = 0; j < trianglePointCount; j++)
            {
                //Debug.Log("I: " + i + ", j: " + j);
                currentTrianglePoint = m_itotalTriangles[(i * trianglePointCount) + j];
                m_vVertices[i][j] = m_vTotalVertices[currentTrianglePoint].normalized * Radius;
                m_vNormals[i][j] = Vector3.up;
            }

            mesh[i].vertices = m_vVertices[i];
            mesh[i].normals = m_vNormals[i];
            mesh[i].triangles = triangles[i];

            faces[i].GetComponent<MeshFilter>().mesh = mesh[i];
        }
    }
}
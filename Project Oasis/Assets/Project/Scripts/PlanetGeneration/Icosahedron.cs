using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Icosahedron : MonoBehaviour
{
    private List<Vector3> m_vTriangles = new List<Vector3>();
    private List<Vector3> m_vVertices = new List<Vector3>();

    private Mesh mesh;

    //private MeshGeometry3D geometry;
    //private int index;
    //private Dictionary<Int64, int> middlePointIndexCache;

    /*/ add vertex to mesh, fix position to be on unit sphere, return index
    private int addVertex(Point3D p)
    {
        double length = Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
        geometry.Positions.Add(new Point3D(p.X / length, p.Y / length, p.Z / length));
        return index++;
    }//*/

    /*/ return index of point in the middle of p1 and p2
    private int getMiddlePoint(int p1, int p2)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        Int64 smallerIndex = firstIsSmaller ? p1 : p2;
        Int64 greaterIndex = firstIsSmaller ? p2 : p1;
        Int64 key = (smallerIndex << 32) + greaterIndex;

        // not in cache, calculate it
        Vector3 point1 = this.geometry.Positions[p1];
        Vector3 point2 = this.geometry.Positions[p2];
        Vector3 middle = new Point3D(
            (point1.X + point2.X) / 2.0f,
            (point1.Y + point2.Y) / 2.0f,
            (point1.Z + point2.Z) / 2.0f);

        // add vertex makes sure point is on unit sphere
        int i = addVertex(middle);

        // store it, return index
        this.middlePointIndexCache.Add(key, i);
        return i;
    }//*/

    void Awake()
    {
        Create(0);
    }

    public void Create(int recursionLevel)
    {
        Debug.Log("Entered Create");
        mesh = new Mesh();
        mesh.name = "Ico Mesh";
        GetComponent<MeshFilter>().mesh = mesh;

        // create 12 vertices of a icosahedron
        float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

        m_vVertices.Add(new Vector3(-1, t, 0));
        m_vVertices.Add(new Vector3(1, t, 0));
        m_vVertices.Add(new Vector3(-1, -t, 0));
        m_vVertices.Add(new Vector3(1, -t, 0));

        m_vVertices.Add(new Vector3(0, -1, t));
        m_vVertices.Add(new Vector3(0, 1, t));
        m_vVertices.Add(new Vector3(0, -1, -t));
        m_vVertices.Add(new Vector3(0, 1, -t));

        m_vVertices.Add(new Vector3(t, 0, -1));
        m_vVertices.Add(new Vector3(t, 0, 1));
        m_vVertices.Add(new Vector3(-t, 0, -1));
        m_vVertices.Add(new Vector3(-t, 0, 1));

        Vector3[] arrVertices = new Vector3[m_vVertices.Count];
        Vector3[] normals = new Vector3[m_vVertices.Count];
        for (int i = 0; i < m_vVertices.Count; ++i)
        {
            arrVertices[i] = m_vVertices[i];
            normals[i] = Vector3.up;
        }

        mesh.vertices = arrVertices;
        mesh.normals = normals;

        m_vTriangles.Add(new Vector3(0, 11, 5));
        m_vTriangles.Add(new Vector3(0, 5, 1));
        m_vTriangles.Add(new Vector3(0, 1, 7));
        m_vTriangles.Add(new Vector3(0, 7, 10));
        m_vTriangles.Add(new Vector3(0, 10, 11));

        // 5 adjacent faces 
        m_vTriangles.Add(new Vector3(1, 5, 9));
        m_vTriangles.Add(new Vector3(5, 11, 4));
        m_vTriangles.Add(new Vector3(11, 10, 2));
        m_vTriangles.Add(new Vector3(10, 7, 6));
        m_vTriangles.Add(new Vector3(7, 1, 8));

        // 5 faces around point 3
        m_vTriangles.Add(new Vector3(3, 9, 4));
        m_vTriangles.Add(new Vector3(3, 4, 2));
        m_vTriangles.Add(new Vector3(3, 2, 6));
        m_vTriangles.Add(new Vector3(3, 6, 8));
        m_vTriangles.Add(new Vector3(3, 8, 9));

        // 5 adjacent faces 
        m_vTriangles.Add(new Vector3(4, 9, 5));
        m_vTriangles.Add(new Vector3(2, 4, 11));
        m_vTriangles.Add(new Vector3(6, 2, 10));
        m_vTriangles.Add(new Vector3(8, 6, 7));
        m_vTriangles.Add(new Vector3(9, 8, 1));

        int[] triangles = 
        {
            // 5 faces around point 0
            0,11,5,
            0,5,1,
            0,1,7,
            0,7,10,
            0,10,11,

            // 5 adjacent faces 
            1,5,9,
            5,11,4,
            11,10,2,
            10,7,6,
            7,1,8,

            // 5 faces around point 3
            3,9,4,
            3,4,2,
            3,2,6,
            3,6,8,
            3,8,9,

            // 5 adjacent faces 
            4,9,5,
            2,4,11,
            6,2,10,
            8,6,7,
            9,8,1
        };

        /*
        for (int i = 0; i < recursionLevel; i++)
        {
            int[] trianglesRefined = new int[triangles.Length * 3];
            //foreach (Vector3 tri in m_vTriangles)
            for(int i = 0; i < m_vTriangles.Count; ++i)
            {
                // replace triangle by 4 triangles
                int a = getMiddlePoint(tri.x, tri.y);
                int b = getMiddlePoint(tri.y, tri.z);
                int c = getMiddlePoint(tri.z, tri.x);

                

                faces2.Add(new Vector3(tri.x, a, c));
                faces2.Add(new Vector3(tri.y, b, a));
                faces2.Add(new Vector3(tri.z, c, b));
                faces2.Add(new Vector3(a, b, c));
            }
            m_vTriangles = faces2;
        }
        //*/
        mesh.triangles = triangles;
    }
}

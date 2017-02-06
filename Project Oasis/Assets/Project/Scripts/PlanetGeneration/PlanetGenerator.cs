using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : IcosahedronGenerator
{
    protected Vector2[] uvs;

    public override void Create()
    {
        if (Recursions != m_iCurrentRecursions)
        {
            m_iCurrentRecursions = Recursions;
            Debug.Log("Recursions Change: " + Recursions);
            Create(m_iCurrentRecursions);
        }
    }

    /*====================================================
    Create
        desc
    params: int - how many times we want to recurse
    return: none
    //*///================================================
    public override void Create(int recursionLevel)
    {
        base.Create(recursionLevel);

        uvs = new Vector2[m_vVertices.Length];
        for (var i = 0; i < m_vVertices.Length; i++)
        {
            var unitVector = m_vVertices[i].normalized;
            Vector2 ISOuv = new Vector2(0, 0);
            ISOuv.x = (Mathf.Atan2(unitVector.x, unitVector.z) + Mathf.PI) / Mathf.PI / 2;
            ISOuv.y = (Mathf.Acos(unitVector.y) + Mathf.PI) / Mathf.PI - 1;
            uvs[i] = new Vector2(ISOuv.x, ISOuv.y);
        }
        mesh.uv = uvs;
    }
}

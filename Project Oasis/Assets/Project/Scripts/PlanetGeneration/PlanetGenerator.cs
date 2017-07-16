using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : IcosahedronChunkGenerator
{
    protected const String STRING_BODY = "Body";
    protected const String STRING_ATMO = "Atmosphere";

    protected Vector2[][] uvs = new Vector2[1][];
    protected SphereCollider m_scPlanetSurface;
    protected SphereCollider m_scAtmosphereSphere;

    protected GameObject Body;
    protected GameObject Atmo;

    public Vector3 planetSurface = new Vector3();

    public override void Init(GameObject parentObj)
    {
        Body = new GameObject();
        Body.name = STRING_BODY;
        //Add collider
        m_scPlanetSurface = Body.AddComponent<SphereCollider>();
        m_scPlanetSurface.radius = Radius; //TODO: Make this differ

        Atmo = new GameObject();
        Atmo.name = STRING_ATMO;
        //Add collider
        m_scAtmosphereSphere = Atmo.AddComponent<SphereCollider>();
        m_scAtmosphereSphere.radius = Radius * 2; //TODO: Make this differ
        m_scAtmosphereSphere.isTrigger = true;

        planetSurface = new Vector3(0,Radius, 0);
        Body.transform.parent = parentObj.transform;
        Atmo.transform.parent = parentObj.transform;

        //Create Planet
        base.Init(Body);        
    }

    public override void Redraw()
    {
        base.Redraw();
    }

    /*====================================================
    Create
        desc
    params: int - how many times we want to recurse
    return: none
    //*///================================================
    protected override void Redraw(int recursionLevel)
    {
        base.Redraw(recursionLevel);

        uvs = new Vector2[m_vVertices.Length][];
        for (int i = 0; i < m_vVertices.Length; ++i)
        {
            uvs[i] = new Vector2[m_vVertices[i].Length];
            for (int j = 0; j < m_vVertices[i].Length; ++j )
            {
                var unitVector = m_vVertices[i][j].normalized;
                Vector2 ISOuv = new Vector2(0, 0);
                ISOuv.x = (Mathf.Atan2(unitVector.x, unitVector.z) + Mathf.PI) / Mathf.PI / 2;
                ISOuv.y = (Mathf.Acos(unitVector.y) + Mathf.PI) / Mathf.PI - 1;
                uvs[i][j] = new Vector2(ISOuv.x, ISOuv.y);
            }
            mesh[i].uv = uvs[i];
        }
    }
}

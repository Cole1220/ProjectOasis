using UnityEngine;
using System.Collections;

public class GenerateTerrain : MonoBehaviour
{
    public Terrain terrain;
    private TerrainData terrainData;

    void Awake()
    {
        terrainData = terrain.terrainData;
    }

	// Use this for initialization
	void Start ()
    {
        EditTerrain();
	}
	
    private void EditTerrain()
    {
        int heightMapWidth = terrainData.heightmapWidth;
        int heightMapHeight = terrainData.heightmapHeight;

        float[,] heights = terrainData.GetHeights(0, 0, heightMapWidth, heightMapHeight);

        for (int y = 0; y < heightMapHeight; ++y)
        {
            for (int x = 0; x < heightMapWidth; ++x)
            {
                float cos = Mathf.Cos(x);
                float sin = -Mathf.Sin(y);
                heights[x, y] = (cos - sin) / 250;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}

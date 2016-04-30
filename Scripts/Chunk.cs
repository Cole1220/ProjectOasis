using UnityEngine;
using System.Collections;

public class Chunk
{
    int width, height, depth;
    //array 
        
    public Chunk()
    {
        width = 5;
        height = 5;
        depth = 5;

        //inst array

        BuildChunk();

    }

    private void BuildChunk()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                for(int k = 0; k < depth; ++k)
                {
                    //fill array with data
                }
            }
        }
    }

}

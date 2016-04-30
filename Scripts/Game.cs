using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    public static Game game;

    void Awake()
    {
        if(game == null)
        {
            DontDestroyOnLoad(this.gameObject);
            game = this;
        }
        else if(game != null)
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Game : MonoBehaviour
{
    public const string STRING_PLANET = "Planet";
    public const String STRING_ATMO = "Atmosphere";

    public static Game g_cGame;
    public static GameObject player; //TODO: reference script Player
    public static GameObject planet; //TODO: reference script Planet
    public static Camera camera;

    public static GameEventManager events;

    public GameObject playerPrefab;
    
	// Used for when the script is enabled
	void Awake ()
    {
		if(g_cGame == null)
        {
            DontDestroyOnLoad(this);
            g_cGame = this;
        }
        else if(g_cGame != this)
        {
            Destroy(gameObject);
        }

        //Set up Camera
        camera = Camera.main;

        events = gameObject.AddComponent<GameEventManager>();
	}

    // Use this for initialization
    void Start()
    {
        //Load up loading Overlay
        //Display loading Overlay

        //Set up Planet
        planet = new GameObject();
        planet.name = STRING_PLANET;
        planet.tag = STRING_PLANET;
        PlanetGenerator planetIcosahedron = planet.AddComponent<PlanetGenerator>();
        planetIcosahedron.Init(planet);

        //Set up Player Location
        //Set up Player Avatar
        //player = Instantiate(Resources.Load("Player")) as GameObject;
        player = Instantiate(playerPrefab) as GameObject;
        player.transform.position = planetIcosahedron.planetSurface;
        CharacterController playerController = player.AddComponent<CharacterController>();
        playerController.radius = 15;
        playerController.height = 40;
        playerController.center = new Vector3(0,20,0);
        Movement playerMovement = player.AddComponent<Movement>();
        PlayerController controller = player.AddComponent<PlayerController>();
        controller.Init();

        //Hide loading Overlay
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
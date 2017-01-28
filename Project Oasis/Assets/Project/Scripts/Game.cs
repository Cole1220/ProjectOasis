using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game g_cGame;
    public static Camera camera;

    public static GameEventManager events;
    
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

        events = gameObject.AddComponent<GameEventManager>();
	}

    // Use this for initialization
    void Start()
    {
        //Set up Camera
        camera = Camera.main;

        //Set up Planet


    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Dictionaries need to be saved through a scriptableObject
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/GameSave.dat", FileMode.Open);

        float hp = 10;
        float mp = 10;
        float exp = 10;

        //save local items to saved items
        SaveData data = new SaveData();
        data.hp = hp;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/GameSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/GameSave.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            //Set local items to saved items
        }
    }
}

[Serializable]
class SaveData
{
    public float hp;
    //Add class data save here and use it to manage saves 
}

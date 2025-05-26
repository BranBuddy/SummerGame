//KOlby
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject hexPrefab;
    private GameObject gameManager;
    public int width = 18;
    public int height = 18;
    public float hexWidth = 1.5f;
    public float hexHeight = 2.0f;

    //gets the game manager and then the size of the map from the game manager before generating the map
    void Start()
    {
        //if there is no GameManager use default w and h which is the medium map size
        if (GameObject.Find("GameManager") != null) 
        {
            gameManager = GameObject.Find("GameManager");
            width = gameManager.GetComponent<SceneChange>().GetMapSize();
            height = gameManager.GetComponent<SceneChange>().GetMapSize();
        }
        GenerateMap();
    }

    //flat-top hexagonal grid generator
    void GenerateMap()
    {
        float xOffset = hexWidth;
        float zOffset = hexHeight * 0.75f;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float xPos = x * xOffset;
                float zPos = z * zOffset;

                if (z % 2 == 1)
                {
                    xPos += xOffset / 2f;
                }

                Vector3 position = new Vector3(xPos, 0, zPos);
                Instantiate(hexPrefab, position, Quaternion.identity, this.transform);
            }
        }
    }
}

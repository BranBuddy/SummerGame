//KOlby
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    int mapSize;
    //Changes the scene
    public void ChangeScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }
    //sets the size of the map
    public void SetMapSize(int size) 
    {
        mapSize = size;
    }
    //gets the size of the map
    public int GetMapSize() 
    {
        return mapSize;
    }
    //makes sure game manager stays between scenes
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

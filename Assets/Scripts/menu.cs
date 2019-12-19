using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menu : MonoBehaviour
{
    public string SceneName;
  
    public void sceneLoader()
    {
        Debug.Log("sceneName to load: " + SceneName);
        SceneManager.LoadScene(SceneName);
    }


}

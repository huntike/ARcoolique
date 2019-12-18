using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menu : MonoBehaviour
{
    public string SceneName;
  
    public void sceneLoader()
    {
        SceneManager.LoadScene(SceneName);
    }


}

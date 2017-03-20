using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger_Script : MonoBehaviour {

    public string _sceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}

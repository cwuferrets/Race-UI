﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public void LoadByIndex(int sceneIndex)
    {
        Destroy(Camera.main.GetComponent<GameObject>());
        SceneManager.LoadScene(sceneIndex);
    }
}

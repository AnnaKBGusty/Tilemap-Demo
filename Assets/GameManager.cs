using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public delegate void LevelChange();
    public static event LevelChange onLevelChange;

    public void ChangeLevel()//called from ladder when player interacts
    {
        if(onLevelChange != null)
        {
            onLevelChange();
        }
    }


    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}

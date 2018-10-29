using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public static void PickLevel()
    {
        var level = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }

}

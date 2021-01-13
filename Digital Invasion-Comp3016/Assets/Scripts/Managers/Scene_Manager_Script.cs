using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager_Script : MonoBehaviour
{
    public GameObject Page1;
    public GameObject Page2;


    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void switch_Pages(int PageNo)
    {
        if (PageNo == 0)
        {
            Page1.SetActive(true);
            Page2.SetActive(false);
        }
        if (PageNo == 1)
        {
            Page1.SetActive(false);
            Page2.SetActive(true);
        }
    }
}

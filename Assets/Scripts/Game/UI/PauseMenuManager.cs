using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject menu;
    bool isOpen = false;

    public void OpenMenu()
    {
        if (!isOpen)
        {
            menu.SetActive(true);
            isOpen = true;
            Time.timeScale = 0;
        }
        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
            isOpen = false;
        }
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        isOpen = false;
    }

    public void ReturnToMainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

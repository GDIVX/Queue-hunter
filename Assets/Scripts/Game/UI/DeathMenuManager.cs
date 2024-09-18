using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour
{
    public GameObject deathMenu;

    public void OpenMenu()
    {
        deathMenu.SetActive(true);
        Time.timeScale = 0;
    }



    public void ReturnToMainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}

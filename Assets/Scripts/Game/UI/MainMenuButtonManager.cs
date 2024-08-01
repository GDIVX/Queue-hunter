using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    

    public void PlayGameButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {

    }

    public void QuitButton()
    {
#if !UNITY_EDITOR
        Application.Quit();
#endif
    }
}

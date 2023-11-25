using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject UpgradeMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UpgradeMenu.active)
            {
                CloseUpgradeMenu();
            }
            else if (!PauseMenu.active)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }
    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        UpgradeMenu.SetActive(false);
    }

    public void OpenUpgradeMenu()
    {
        UpgradeMenu.SetActive(true);
        Time.timeScale = 0;
        PauseMenu.SetActive(false);
    }

    public void CloseUpgradeMenu()
    {
        UpgradeMenu.SetActive(false);
        Time.timeScale = 1;
        PauseMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

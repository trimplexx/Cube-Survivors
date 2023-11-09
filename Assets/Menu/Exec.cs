using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exec : MonoBehaviour
{
    // Funkcja do przejścia do innej sceny
    public void PrzejdzDoSceny()
    {
        SceneManager.LoadScene("character");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("PlayArea");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}

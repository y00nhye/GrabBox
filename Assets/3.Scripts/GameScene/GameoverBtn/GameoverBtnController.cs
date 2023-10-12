using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverBtnController : MonoBehaviour
{
    public void RetryBtn()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.instance.isGameOver = false;
        GameManager.instance.timeRank = 0;
        GameManager.instance.objectCount = 3;

        SceneManager.LoadScene("GameScene");
    }

    public void Start()
    {
        RecordJson.instance.LoadRecord();

        FindObjectOfType<RecordController>().RecordWrite(RecordJson.instance.timeRecordData.timeRecord);
    }
}

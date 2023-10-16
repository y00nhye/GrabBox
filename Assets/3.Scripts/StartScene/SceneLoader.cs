using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] InputField userName;

    public void GameStart()
    {
        GameManager.Instance.isGameOver = false;
        GameManager.Instance.timeRank = 0;
        GameManager.Instance.objectCount = 3;

        if (userName.text == "")
        {
            userName.gameObject.transform.GetChild(1).GetComponent<Text>().color = Color.red;
        }
        else
        {
            //유저 id 기록
            GameManager.Instance.userName = userName.text;

            SceneManager.LoadScene("GameScene");

        }
    }

    public void Start()
    {
        //Json
        /*
        RecordJson.Instance.LoadRecord();

        FindObjectOfType<RecordController>().RecordWrite(RecordJson.Instance.timeRecordData.timeRecord);
        */

        FindObjectOfType<RecordController>().RecordWriteDB(DBController.Instance.DBSelect());

        if (GameManager.Instance.userName != null)
        {
            userName.text = GameManager.Instance.userName;
        }
    }
}

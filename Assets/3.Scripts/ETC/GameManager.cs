using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string userName = null;
    
    public int objectCount = 3;
    public int timeRank = 0;

    public bool isGameOver = false;

    private void Update()
    {
        if (objectCount == 0)
        {
            isGameOver = true;

            objectCount = 3;
        }

        if (isGameOver)
        {
            //타이머 정지, 게임오버 팝업 등장
            FindObjectOfType<Timer>().StopTimer();
            GameObject.Find("Gameover").transform.GetChild(0).gameObject.SetActive(true);

            isGameOver = false;

            //타임 기록 저장 (Json)
            /*
            RecordJson.Instance.SaveRecord();
            */

            //타임, 유저 이름 저장 (DB)
            DBController.Instance.DBInsert(userName, timeRank);
        }
    }
}

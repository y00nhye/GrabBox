using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

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

            //타임 기록 저장
            RecordJson.instance.SaveRecord();

            isGameOver = false;
        }
    }
}

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
            //Ÿ�̸� ����, ���ӿ��� �˾� ����
            FindObjectOfType<Timer>().StopTimer();
            GameObject.Find("Gameover").transform.GetChild(0).gameObject.SetActive(true);

            isGameOver = false;

            //Ÿ�� ��� ���� (Json)
            /*
            RecordJson.Instance.SaveRecord();
            */

            //Ÿ��, ���� �̸� ���� (DB)
            DBController.Instance.DBInsert(userName, timeRank);
        }
    }
}

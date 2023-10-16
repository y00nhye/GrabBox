using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timerTxt;

    public int currentTime = 0;

    private bool isStopTimer = false;

    private void Start()
    {
        currentTime = 0;
        
        StartCoroutine(TimerOn_co());
    }

    public void StopTimer()
    {
        isStopTimer = true;

        GameManager.Instance.timeRank = currentTime;
    }

    IEnumerator TimerOn_co()
    {
        while (!GameManager.Instance.isGameOver && !isStopTimer)
        {
            currentTime += 1;

            timerTxt.text = currentTime.ToString();

            yield return new WaitForSeconds(1f);
        }

        isStopTimer = false;
    }
}

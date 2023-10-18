using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordController : MonoBehaviour
{
    [Header("Record UI")]
    [SerializeField] GameObject recordPanel;
    [SerializeField] GameObject recordBtn;

    [SerializeField] Text[] recordTxt;

    public void RecordPop()
    {
        recordPanel.SetActive(true);
        recordBtn.SetActive(false);
    }

    public void RecordClose()
    {
        recordPanel.SetActive(false);
        recordBtn.SetActive(true);
    }

    //기록 나타내기 (Json)
    public void RecordWrite(List<int> record)
    {
        //기록 정렬
        for (int i = 0; i < record.Count; i++)
        {
            for (int j = i + 1; j < record.Count; j++)
            {
                if (record[i] > record[j])
                {
                    int num = record[i];
                    record[i] = record[j];
                    record[j] = num;
                }
            }
        }

        //기록 text에 옮기기
        if (record.Count < 9)
        {
            for (int i = 0; i < record.Count; i++)
            {
                recordTxt[i].text = record[i].ToString();
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                recordTxt[i].text = record[i].ToString();
            }
        }
    }

    //기록 나타내기 (DB)
    public void RecordWriteDB(Dictionary<string, string> data)
    {
        List<string> id = new List<string>();
        List<string> record = new List<string>();

        foreach (string value in data.Keys)
        {
            id.Add(value);
        }
        foreach (string value in data.Values)
        {
            record.Add(value);
        }

        //기록 text에 옮기기
        for (int i = 0; i < record.Count; i++)
        {
            recordTxt[i].text = $"[ {id[i]} ]   {record[i]}";
        }

        /*if (record.Count < 9)
        {
            for (int i = 0; i < record.Count; i++)
            {
                recordTxt[i].text = $"[ {id[i]} ]   {record[i]}";
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                recordTxt[i].text = $"[ {id[i]} ]   {record[i]}";
            }
        }*/
    }
}

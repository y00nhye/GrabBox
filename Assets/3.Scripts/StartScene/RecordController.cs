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

    //��� ��Ÿ����
    public void RecordWrite(List<int> record)
    {
        //��� ����
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

        //��� text�� �ű��
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
}
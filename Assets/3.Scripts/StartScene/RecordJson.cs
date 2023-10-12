using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TimeRecordData
{
    public List<int> timeRecord = new List<int>();
}

public class RecordJson : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    public static RecordJson instance = null;

    private void Awake()
    {
        if (instance == null)
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

    public TimeRecordData timeRecordData = new TimeRecordData();

    public void SaveRecord()
    {
        timeRecordData.timeRecord.Add(GameManager.instance.timeRank);

        string jsonData = JsonUtility.ToJson(timeRecordData, true);

        string path = Path.Combine(Application.dataPath, "timeRecordData.json");

        File.WriteAllText(path, jsonData);

        Debug.Log("save json");
    }

    public void LoadRecord()
    {
        string path = Path.Combine(Application.dataPath, "timeRecordData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            timeRecordData = JsonUtility.FromJson<TimeRecordData>(jsonData);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TimeRecordData
{
    public List<int> timeRecord = new List<int>();
}

public class RecordJson : Singleton<RecordJson>
{
    public TimeRecordData timeRecordData = new TimeRecordData();

    public void SaveRecord()
    {
        timeRecordData.timeRecord.Add(GameManager.Instance.timeRank);

        string jsonData = JsonUtility.ToJson(timeRecordData, true);

        string path = Path.Combine(Application.dataPath, "timeRecordData.json");

        File.WriteAllText(path, jsonData);
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

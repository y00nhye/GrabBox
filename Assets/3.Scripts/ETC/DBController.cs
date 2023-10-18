using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

public class DBConnectionData
{
    //DB 접속 경로 data
    public string ip_Address = "127.0.0.1";
    public string db_id = "root";
    public string db_pw = "1234";
    public string db_name = "DB";
}

public class DBController : Singleton<DBController>
{
    DBConnectionData dbconnerctionData = new DBConnectionData(); //json 파일 저장 정보

    public static MySqlConnection sqlConnection;

    string strConnection = null;

    private void JsonSaveDBdata()
    {
        string jsonData = JsonUtility.ToJson(dbconnerctionData, true);
        string path = Path.Combine(Application.dataPath, "dbconnerctionData.json");

        File.WriteAllText(path, jsonData);
    }
    private void JsonLoadDBdata()
    {
        string path = Path.Combine(Application.dataPath, "dbconnerctionData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            dbconnerctionData = JsonUtility.FromJson<DBConnectionData>(jsonData);
        }
    }

    private void Start()
    {
        //json data 가 없다면 json 파일 생성
        if (Path.Combine(Application.dataPath, "dbconnerctionData.json") == null)
        {
            JsonSaveDBdata();
        }

        JsonLoadDBdata(); //Json File 불러오기

        strConnection = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8;"
            , dbconnerctionData.ip_Address, dbconnerctionData.db_id, dbconnerctionData.db_pw, dbconnerctionData.db_name);

        try
        {
            sqlConnection = new MySqlConnection(strConnection);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    //연결상태 확인
    public void ConnectCheck()
    {
        if (sqlConnection.State == ConnectionState.Closed)
        {
            for (int i = 0; i < 3; i++)  //세번 접속 시도
            {
                sqlConnection.Open();

                Debug.Log("try connect to db..");

                if (sqlConnection.State == ConnectionState.Open)
                {
                    break;
                }
            }
        }
        else
        {
            return;
        }
    }

    //data 삽입 및 업데이트
    public void DBInsert(string id, int record)
    {
        string select = "select * from user";

        string insert = $"insert into user(ID, Record) value (@id, @record)";

        Dictionary<string, string> idList = OnSelectRequest(select);

        foreach (string key in idList.Keys)
        {
            if (key == id)
            {
                if (int.Parse(idList[key]) > GameManager.Instance.timeRank)
                {
                    insert = $"update user set Record = @record where ID = @id";

                    break;
                }
                else
                {
                    return;
                }
            }
        }

        OnInsertOrUpdateRequest(insert, id, record);
    }

    //data 검색
    public Dictionary<string, string> DBSelect()
    {
        string select = "select * from user order by record limit 8"; //오름차순 정렬 및 상위 8개 제한

        Dictionary<string, string> data = new Dictionary<string, string>();

        data = OnSelectRequest(select);

        return data;
    }

    //db 접근 (insert or update)
    public static bool OnInsertOrUpdateRequest(string str_query, string id, int record)
    {
        //sqlconnection 연결상태 확인
        DBController.Instance.ConnectCheck();

        try
        {
            MySqlCommand cmd = new MySqlCommand(str_query, sqlConnection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@record", record);

            cmd.ExecuteNonQuery();

            sqlConnection.Close();

            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());

            return false;
        }
    }

    //db 접근 (select)
    public static Dictionary<string, string> OnSelectRequest(string p_query)
    {
        //sqlconnection 연결상태 확인
        DBController.Instance.ConnectCheck();

        try
        {
            MySqlCommand cmd = new MySqlCommand(p_query, sqlConnection);
            //cmd.CommandText = "select * from user order by Record ASC;"; //오름차순 정렬 쿼리문

            MySqlDataReader reader = cmd.ExecuteReader(); //데이터 불러오기

            Dictionary<string, string> data = new Dictionary<string, string>(); //id, record 를 dictionary에 저장

            while (reader.Read())
            {
                string key = $"{reader["ID"]}"; //id 기록
                string value = $"{reader["Record"]}"; //record 기록

                data.Add(key, value);
            }

            sqlConnection.Close();

            return data;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            return null;
        }
    }

    private void OnApplicationQuit()
    {
        sqlConnection.Close();
    }
}

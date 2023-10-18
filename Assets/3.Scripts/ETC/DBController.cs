using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

public class DBConnectionData
{
    //DB ���� ��� data
    public string ip_Address = "127.0.0.1";
    public string db_id = "root";
    public string db_pw = "1234";
    public string db_name = "DB";
}

public class DBController : Singleton<DBController>
{
    DBConnectionData dbconnerctionData = new DBConnectionData(); //json ���� ���� ����

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
        //json data �� ���ٸ� json ���� ����
        if (Path.Combine(Application.dataPath, "dbconnerctionData.json") == null)
        {
            JsonSaveDBdata();
        }

        JsonLoadDBdata(); //Json File �ҷ�����

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

    //������� Ȯ��
    public void ConnectCheck()
    {
        if (sqlConnection.State == ConnectionState.Closed)
        {
            for (int i = 0; i < 3; i++)  //���� ���� �õ�
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

    //data ���� �� ������Ʈ
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

    //data �˻�
    public Dictionary<string, string> DBSelect()
    {
        string select = "select * from user order by record limit 8"; //�������� ���� �� ���� 8�� ����

        Dictionary<string, string> data = new Dictionary<string, string>();

        data = OnSelectRequest(select);

        return data;
    }

    //db ���� (insert or update)
    public static bool OnInsertOrUpdateRequest(string str_query, string id, int record)
    {
        //sqlconnection ������� Ȯ��
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

    //db ���� (select)
    public static Dictionary<string, string> OnSelectRequest(string p_query)
    {
        //sqlconnection ������� Ȯ��
        DBController.Instance.ConnectCheck();

        try
        {
            MySqlCommand cmd = new MySqlCommand(p_query, sqlConnection);
            //cmd.CommandText = "select * from user order by Record ASC;"; //�������� ���� ������

            MySqlDataReader reader = cmd.ExecuteReader(); //������ �ҷ�����

            Dictionary<string, string> data = new Dictionary<string, string>(); //id, record �� dictionary�� ����

            while (reader.Read())
            {
                string key = $"{reader["ID"]}"; //id ���
                string value = $"{reader["Record"]}"; //record ���

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

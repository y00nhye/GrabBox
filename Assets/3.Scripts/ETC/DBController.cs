using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;

public class DBController : Singleton<DBController>
{
    public static MySqlConnection sqlConnection;

    //DB 접속 경로 data
    static string ipAddress = "127.0.0.1";
    static string db_id = "root";
    static string db_pw = "1234";
    static string db_name = "DB";

    string strConnection = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8;", ipAddress, db_id, db_pw, db_name);

    private void Start()
    {
        try
        {
            sqlConnection = new MySqlConnection(strConnection);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    //data 삽입 및 업데이트
   public void DBInsert(string id, int record)
    { 
        string select = "select * from user";

        string insert = $"insert into user(ID, Record) value (\"{id}\",{record} )";

        Dictionary<string, string> idList = OnSearchID(select);

        foreach(string key in idList.Keys)
        {
            if(key == id)
            {
                if(int.Parse(idList[key]) > GameManager.Instance.timeRank)
                {
                    insert = $"update user set Record = {record} where ID = \"{id}\"";

                    break;
                }
                else
                {
                    return;
                }
            }
        }

        OnInsertOrUpdateRequest(insert);
    }

    //data 검색
    public List<string> DBSelect()
    {
        string select = "select * from user";

        List<string> data = new List<string>();

        data = OnSelectRequest(select);

        return data;
    }

    //db 접근 (insert or update)
    public static bool OnInsertOrUpdateRequest(string str_query)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = str_query;

            sqlConnection.Open();

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
    public static List<string> OnSelectRequest(string p_query)
    {
        try
        {
            sqlConnection.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = p_query;
            cmd.CommandText = "select * from user order by Record ASC;"; //오름차순 정렬 쿼리문

            MySqlDataReader table = cmd.ExecuteReader(); //데이터 불러오기

            List<string> data = new List<string>();

            while (table.Read())
            {
                string data_in = $"[ {table["ID"]} ]  {table["Record"]}"; //id, record 기록

                data.Add(data_in);
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

    //db 접근 (select) / 현재 id, time record 값과 비교를 위한 db 검색
    public static Dictionary<string, string> OnSearchID(string p_query)
    {
        try
        {
            sqlConnection.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = p_query;

            MySqlDataReader table = cmd.ExecuteReader();

            Dictionary<string, string> data = new Dictionary<string, string>(); //id, record 를 dictionary에 저장해 현재 값과 비교

            while (table.Read())
            {
                string key = $"{table["ID"]}"; //id 기록
                string value = $"{table["Record"]}"; //record 기록

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

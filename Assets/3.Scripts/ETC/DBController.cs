using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using MySql.Data.MySqlClient;

public class DBController : Singleton<DBController>
{
    public static MySqlConnection sqlConnection;

    //DB ���� ��� data
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

    //data ���� �� ������Ʈ
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

    //data �˻�
    public List<string> DBSelect()
    {
        string select = "select * from user";

        List<string> data = new List<string>();

        data = OnSelectRequest(select);

        return data;
    }

    //db ���� (insert or update)
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

    //db ���� (select)
    public static List<string> OnSelectRequest(string p_query)
    {
        try
        {
            sqlConnection.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = p_query;
            cmd.CommandText = "select * from user order by Record ASC;"; //�������� ���� ������

            MySqlDataReader table = cmd.ExecuteReader(); //������ �ҷ�����

            List<string> data = new List<string>();

            while (table.Read())
            {
                string data_in = $"[ {table["ID"]} ]  {table["Record"]}"; //id, record ���

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

    //db ���� (select) / ���� id, time record ���� �񱳸� ���� db �˻�
    public static Dictionary<string, string> OnSearchID(string p_query)
    {
        try
        {
            sqlConnection.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandText = p_query;

            MySqlDataReader table = cmd.ExecuteReader();

            Dictionary<string, string> data = new Dictionary<string, string>(); //id, record �� dictionary�� ������ ���� ���� ��

            while (table.Read())
            {
                string key = $"{table["ID"]}"; //id ���
                string value = $"{table["Record"]}"; //record ���

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

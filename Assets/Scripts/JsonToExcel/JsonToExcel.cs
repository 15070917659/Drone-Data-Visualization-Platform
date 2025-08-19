using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System;



public class JsonToExcel
{
    //json  ת DataTable  ����   Ȼ�󵼳�  excel
    //����1 string json json���� ����2 tabName������Excel������ƣ�����Ҫ��.csv��׺����������ˣ�
    public static void JsonToCsv(string json, string path)
    {
        //json����ת��  DataTable   ����
        DataTable dataTab = JsonToDataTable<PixhawkSensorData>(json);
        //DataTable���ݴ洢�� .csv ��
        DataTableToCsv(dataTab, path);//������ת��Ԫ��ÿһ�е�������һ����Ԫ����
    }

    //json  ת DataTable
    private static DataTable JsonToDataTable<T>(string json)
    {
        DataTable dataTable = new DataTable(); //ʵ����
        DataTable result;
        try
        {
            List<T> arrayList = JsonConvert.DeserializeObject<List<T>>(json);
            if (arrayList.Count <= 0)
            {
                result = dataTable;
                return result;
            }

            foreach (T item in arrayList)
            {
                Dictionary<string, object> dictionary = GetFields<T>(item);//���Test���еĶ������Ծ�ʹ��GetProperties������ȡdictionary
                //Columns
                if (dataTable.Columns.Count == 0)
                {
                    foreach (string current in dictionary.Keys)
                    {
                        Debug.Log("����һ�У�" + current + "�����ͣ�" + dictionary[current].GetType());
                        dataTable.Columns.Add(current, dictionary[current].GetType());
                    }
                }
                //Rows
                DataRow dataRow = dataTable.NewRow();
                foreach (string current in dictionary.Keys)
                {
                    Debug.Log("����һ�У�" + current + " =" + dictionary[current]);
                    dataRow[current] = dictionary[current];
                }
                dataTable.Rows.Add(dataRow); //ѭ������е�DataTable��
            }
        }
        catch
        {

        }
        result = dataTable;
        return result;
    }

    //����Excel
    private static void DataTableToCsv(DataTable table, string file)
    {
        file = file + ".csv";
        if (File.Exists(file))
        {
            File.Delete(file);
        }
        if (table.Columns.Count <= 0)
        {
            Debug.LogError("table.Columns.Count <= 0");
            return;
        }

        string title = "";
        FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
        //StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);//������Ļ�����
        StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.UTF8);
        for (int i = 0; i < table.Columns.Count; i++)
        {
            //title += table.Columns[i].ColumnName + "\t"; //��λ���Զ�������һ��Ԫ��Ȼ�����������к�û��
            title += table.Columns[i].ColumnName + ","; //��λ���Զ�������һ��Ԫ������ûë��
        }

        title = title.Substring(0, title.Length - 1) + "\n";
        sw.Write(title);
        foreach (DataRow row in table.Rows)
        {
            string line = "";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                //line += row[i].ToString().Trim() + "\t"; //���ݣ��Զ�������һ��Ԫ��Ȼ�����������к�û��
                line += row[i].ToString().Trim() + ","; //���ݣ��Զ�������һ��Ԫ������ûë��
            }
            line = line.Substring(0, line.Length - 1) + "\n";
            sw.Write(line);
        }
        sw.Close();
        fs.Close();
    }

    /// <summary>
    ///  ʹ�÷�����ƻ�ȡ���е��ֶ�-ֵ�б�ֻ��ȡһ��ģ�����ȡ�Ӷ�����ֶ�
    /// </summary>
    /// <returns>�����ֶ�����</returns>
    public static Dictionary<string, object> GetFields<T>(T t)
    {
        Dictionary<string, object> keyValues = new Dictionary<string, object>();
        if (t == null)
        {
            return keyValues;
        }
        System.Reflection.FieldInfo[] fields = t.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (fields.Length <= 0)
        {
            return keyValues;
        }
        foreach (System.Reflection.FieldInfo item in fields)
        {
            string name = item.Name; //����
            object value = item.GetValue(t);  //ֵ

            if (item.FieldType.IsValueType || item.FieldType.Name.StartsWith("String"))
            {
                keyValues.Add(name, value);
            }
            else
            {
                Debug.Log("ֵ���ͣ�" + value.GetType().Name);
                //�ݹ齫�����е��ֶμ������
                Dictionary<string, object> subKeyValues = GetFields<object>(value);
                foreach (KeyValuePair<string, object> temp in subKeyValues)
                {
                    keyValues.Add(temp.Key, temp.Value);
                }
            }
        }
        return keyValues;
    }

    /// <summary>
    /// ʹ�÷�����ƻ�ȡ���е�����-ֵ�б�ֻ��ȡһ��ģ�����ȡ�Ӷ��������
    /// </summary>
    /// <returns>������������</returns>
    public static Dictionary<string, object> GetProperties<T>(T t)
    {
        Dictionary<string, object> keyValues = new Dictionary<string, object>();
        if (t == null)
        {
            return keyValues;
        }
        System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        if (properties.Length <= 0)
        {
            return keyValues;
        }
        foreach (System.Reflection.PropertyInfo item in properties)
        {
            string name = item.Name; //����
            object value = item.GetValue(t, null);  //ֵ

            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
            {
                keyValues.Add(name, value);
            }
            else
            {
                Debug.Log("ֵ���ͣ�" + value.GetType().Name);
                //�ݹ齫�����е��ֶμ������
                Dictionary<string, object> subKeyValues = GetProperties<object>(value);
                foreach (KeyValuePair<string, object> temp in subKeyValues)
                {
                    keyValues.Add(temp.Key, temp.Value);
                }
            }
        }
        return keyValues;
    }
}


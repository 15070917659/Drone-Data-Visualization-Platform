using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System;



public class JsonToExcel
{
    //json  转 DataTable  类型   然后导出  excel
    //参数1 string json json数据 参数2 tabName导出成Excel后的名称（不需要带.csv后缀，后面加上了）
    public static void JsonToCsv(string json, string path)
    {
        //json数据转成  DataTable   类型
        DataTable dataTab = JsonToDataTable<PixhawkSensorData>(json);
        //DataTable数据存储到 .csv 表
        DataTableToCsv(dataTab, path);//不会跳转单元格，每一行的数据在一个单元里面
    }

    //json  转 DataTable
    private static DataTable JsonToDataTable<T>(string json)
    {
        DataTable dataTable = new DataTable(); //实例化
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
                Dictionary<string, object> dictionary = GetFields<T>(item);//如果Test类中的都是属性就使用GetProperties方法获取dictionary
                //Columns
                if (dataTable.Columns.Count == 0)
                {
                    foreach (string current in dictionary.Keys)
                    {
                        Debug.Log("增加一列：" + current + "，类型：" + dictionary[current].GetType());
                        dataTable.Columns.Add(current, dictionary[current].GetType());
                    }
                }
                //Rows
                DataRow dataRow = dataTable.NewRow();
                foreach (string current in dictionary.Keys)
                {
                    Debug.Log("增加一行：" + current + " =" + dictionary[current]);
                    dataRow[current] = dictionary[current];
                }
                dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
            }
        }
        catch
        {

        }
        result = dataTable;
        return result;
    }

    //导出Excel
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
        //StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);//这个中文会乱码
        StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.UTF8);
        for (int i = 0; i < table.Columns.Count; i++)
        {
            //title += table.Columns[i].ColumnName + "\t"; //栏位：自动跳到下一单元格，然而我这里运行后并没有
            title += table.Columns[i].ColumnName + ","; //栏位：自动跳到下一单元格，老铁没毛病
        }

        title = title.Substring(0, title.Length - 1) + "\n";
        sw.Write(title);
        foreach (DataRow row in table.Rows)
        {
            string line = "";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                //line += row[i].ToString().Trim() + "\t"; //内容：自动跳到下一单元格，然而我这里运行后并没有
                line += row[i].ToString().Trim() + ","; //内容：自动跳到下一单元格，老铁没毛病
            }
            line = line.Substring(0, line.Length - 1) + "\n";
            sw.Write(line);
        }
        sw.Close();
        fs.Close();
    }

    /// <summary>
    ///  使用反射机制获取类中的字段-值列表：只获取一层的，不获取子对象的字段
    /// </summary>
    /// <returns>所有字段名称</returns>
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
            string name = item.Name; //名称
            object value = item.GetValue(t);  //值

            if (item.FieldType.IsValueType || item.FieldType.Name.StartsWith("String"))
            {
                keyValues.Add(name, value);
            }
            else
            {
                Debug.Log("值类型：" + value.GetType().Name);
                //递归将子类中的字段加入进来
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
    /// 使用反射机制获取类中的属性-值列表：只获取一层的，不获取子对象的属性
    /// </summary>
    /// <returns>所有属性名称</returns>
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
            string name = item.Name; //名称
            object value = item.GetValue(t, null);  //值

            if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
            {
                keyValues.Add(name, value);
            }
            else
            {
                Debug.Log("值类型：" + value.GetType().Name);
                //递归将子类中的字段加入进来
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


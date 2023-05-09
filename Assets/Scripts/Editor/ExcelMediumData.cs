
#if UNITY_EDITOR
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Excel;
using System.Reflection;
using System;
 
//ExcelMediumData
public class ExcelMediumData
{
    //ExcelName
    public string excelName;
    //<dataName, dataType>
    public Dictionary<string, string> propertyNameTypeDic;
    //List<OneLine>，List<Dictionary<dataName, allDataOfOneLine>>
    public List<Dictionary<string, string>> allItemValueRowList;
}
 
public static class ExcelDataReader
{
    //Name
    const int excelNameRow = 2;
    //Type
    const int excelTypeRow = 3;
    //Data
    const int excelDataRow = 4;
 
    //ExcelPath
    public static string excelFilePath = Application.dataPath + "/Excel";
   // public static string excelFilePath = Application.dataPath.Replace("Assets", "Excel");
 
    //CS-File-path
    static string excelCodePath = Application.dataPath + "/Scripts/Excel/AutoCreateCSCode";
    //Asset-File-path
    static string excelAssetPath = "Assets/Resources/ExcelAsset";
 
    #region --- Read Excel ---
 
    //CreateC#Script
    public static void ReadAllExcelToCode()
    {
        //ReadAllExcel
        string[] excelFileFullPaths = Directory.GetFiles(excelFilePath, "*.xlsx");
 
        if (excelFileFullPaths == null || excelFileFullPaths.Length == 0)
        {
            Debug.Log("Excel file count == 0");
            return;
        }

        //CreateAllCS
        for (int i = 0; i < excelFileFullPaths.Length; i++)
        {
            ReadOneExcelToCode(excelFileFullPaths[i]);
        }
    }

    //CreateC#Script
    public static void ReadOneExcelToCode(string excelFileFullPath)
    {
        //CreateMediumData
        ExcelMediumData excelMediumData = CreateClassCodeByExcelPath(excelFileFullPath);
        if (excelMediumData != null)
        {
            //GenerateCS
            string classCodeStr = ExcelCodeCreater.CreateCodeStrByExcelData(excelMediumData);
            if (!string.IsNullOrEmpty(classCodeStr))
            {
                //Check Path
                if (!Directory.Exists(excelCodePath))
                    Directory.CreateDirectory(excelCodePath);
                //Write in
                if (WriteCodeStrToSave(excelCodePath, excelMediumData.excelName + "ExcelData", classCodeStr))
                {
                    Debug.Log("<color=green>Auto Create Excel Scripts Success : </color>" + excelMediumData.excelName);
                    return;
                }
            }
        }
        //Error
        Debug.LogError("Auto Create Excel Scripts Fail : " + (excelMediumData == null ? "" : excelMediumData.excelName));
    }
 
    #endregion
 
    #region Create Asset
 
    public static void CreateAllExcelAsset()
    {
        string[] excelFileFullPaths = Directory.GetFiles(excelFilePath, "*.xlsx");

        if (excelFileFullPaths == null || excelFileFullPaths.Length == 0)
        {
            Debug.Log("Excel file count == 0");
            return;
        }

        for (int i = 0; i < excelFileFullPaths.Length; i++)
        {
            CreateOneExcelAsset(excelFileFullPaths[i]);
        }
    }
 
    public static void CreateOneExcelAsset(string excelFileFullPath)
    {
        ExcelMediumData excelMediumData = CreateClassCodeByExcelPath(excelFileFullPath);
        if (excelMediumData != null)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            object class0bj = assembly.CreateInstance(excelMediumData.excelName + "Assignment",true);
 
            Type type = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type tempType = asm.GetType(excelMediumData.excelName + "AssetAssignment");
                if (tempType != null)
                {
                    type = tempType;
                    break;
                }
            }
            if (type != null)
            {
                MethodInfo methodInfo = type.GetMethod("CreateAsset");
                if (methodInfo != null)
                {
                    methodInfo.Invoke(null, new object[] { excelMediumData.allItemValueRowList, excelAssetPath });
                    Debug.Log("<color=green>Auto Create Excel Asset Success : </color>" + excelMediumData.excelName);
                    return;
                }
            }
        }
        Debug.LogError("Auto Create Excel Asset Fail : " + (excelMediumData == null ? "" : excelMediumData.excelName));
    }
 
    #endregion
 
    #region --- private ---
 
    private static ExcelMediumData CreateClassCodeByExcelPath(string excelFileFullPath)
    {
        if (string.IsNullOrEmpty(excelFileFullPath))
            return null;
 
        excelFileFullPath = excelFileFullPath.Replace("\\", "/");
 
        FileStream stream = File.Open(excelFileFullPath, FileMode.Open, FileAccess.Read);
        if (stream == null)
            return null;

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        if (excelReader == null || !excelReader.IsValid)
        {
            Debug.Log("Invalid excel ： " + excelFileFullPath);
            return null;
        }
 
        KeyValuePair<string, string>[] propertyNameTypes = null;

        List<Dictionary<string, string>> allItemValueRowList = new List<Dictionary<string, string>>();
 
        int propertyCount = 0;

        int curRowIndex = 1;

        while (excelReader.Read())
        {
            if (excelReader.FieldCount == 0)
                continue;

            string[] datas = new string[excelReader.FieldCount];
            for (int j = 0; j < excelReader.FieldCount; ++j)
            {
                datas[j] = excelReader.GetString(j);
            }

            if (datas.Length == 0 || string.IsNullOrEmpty(datas[0]))
            {
                curRowIndex++;
                continue;
            }

            if (curRowIndex >= excelDataRow)
            {

                if (propertyCount <= 0)
                    return null;
 
                Dictionary<string, string> itemDic = new Dictionary<string, string>(propertyCount);
                for (int j = 0; j < propertyCount; j++)
                {
                    if (j < datas.Length)
                        itemDic[propertyNameTypes[j].Key] = datas[j];
                    else
                        itemDic[propertyNameTypes[j].Key] = null;
                }
                allItemValueRowList.Add(itemDic);
            }
            else if (curRowIndex == excelNameRow)
            {
                propertyCount = datas.Length;
                if (propertyCount <= 0)
                    return null;
                propertyNameTypes = new KeyValuePair<string, string>[propertyCount];
                for (int i = 0; i < propertyCount; i++)
                {
                    propertyNameTypes[i] = new KeyValuePair<string, string>(datas[i], null);
                }
            }
            else if (curRowIndex == excelTypeRow)
            {
                if (propertyCount <= 0 || datas.Length < propertyCount)
                    return null;
                for (int i = 0; i < propertyCount; i++)
                {
                    propertyNameTypes[i] = new KeyValuePair<string, string>(propertyNameTypes[i].Key, datas[i]);
                }
            }
            curRowIndex++;
        }
 
        if (propertyNameTypes.Length == 0 || allItemValueRowList.Count == 0)
            return null;
 
        ExcelMediumData excelMediumData = new ExcelMediumData();
        excelMediumData.excelName = excelReader.Name;
        excelMediumData.propertyNameTypeDic = new Dictionary<string, string>();
        for (int i = 0; i < propertyCount; i++)
        {
            if (excelMediumData.propertyNameTypeDic.ContainsKey(propertyNameTypes[i].Key))
                return null;
            excelMediumData.propertyNameTypeDic.Add(propertyNameTypes[i].Key, propertyNameTypes[i].Value);
        }
        excelMediumData.allItemValueRowList = allItemValueRowList;
        return excelMediumData;
    }
 
    private static bool WriteCodeStrToSave(string writeFilePath, string codeFileName, string classCodeStr)
    {
        if (string.IsNullOrEmpty(codeFileName) || string.IsNullOrEmpty(classCodeStr))
            return false;
        if (!Directory.Exists(writeFilePath))
            Directory.CreateDirectory(writeFilePath);
        StreamWriter sw = new StreamWriter(writeFilePath + "/" + codeFileName + ".cs");
        sw.WriteLine(classCodeStr);
        sw.Close();
        UnityEditor.AssetDatabase.Refresh();
        return true;
    }
 
    #endregion
 
}
#endif
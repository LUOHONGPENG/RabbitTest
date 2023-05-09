using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
public class ExcelManager : Singleton<ExcelManager>
{
    //A dictionary?
    Dictionary<Type, object> excelDataDic = new Dictionary<Type, object>();
 
    public T GetExcelData<T, V>() where T : ExcelDataBase<V> where V : ExcelItemBase
    {
        Type type = typeof(T);
        if (excelDataDic.ContainsKey(type) && excelDataDic[type] is T)
            return excelDataDic[type] as T;
 
        T excelData = Resources.Load<T>("ExcelAsset/"+ type.Name);
 
        if (excelData != null)
            excelDataDic.Add(type, excelData as T);
 
        return excelData;
    }
 
    public V GetExcelItem<T, V>(int targetId) where T : ExcelDataBase<V> where V : ExcelItemBase
    {
        var excelData = GetExcelData<T, V>();
 
        if (excelData != null)
            return excelData.GetExcelItem(targetId);
        return null;
    }
 
}
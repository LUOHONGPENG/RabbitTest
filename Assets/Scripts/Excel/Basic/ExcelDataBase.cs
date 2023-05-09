
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
 
public class ExcelDataBase<T> : ScriptableObject where T : ExcelItemBase
{
    /// <summary>
    /// The array that contains all the data
    /// </summary>
    public T[] items;

    /// <summary>
    /// Find and get the ExcelItem that has the targetID
    /// </summary>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public T GetExcelItem(int targetId)
    {
        if(items != null && items.Length > 0)
        {
            return items.FirstOrDefault(item => item.id == targetId);
        }
        return null;
    }
}
 
public class ExcelItemBase
{
    public int id;
}
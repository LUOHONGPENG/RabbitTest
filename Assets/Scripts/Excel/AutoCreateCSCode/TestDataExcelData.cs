/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class TestDataExcelItem : ExcelItemBase
{
	public List<string> name;
	public List<int> value;
	public float dic;
}

[CreateAssetMenu(fileName = "TestDataExcelData", menuName = "Excel To ScriptableObject/Create TestDataExcelData", order = 1)]
public class TestDataExcelData : ExcelDataBase<TestDataExcelItem>
{
}

#if UNITY_EDITOR
public class TestDataAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		TestDataExcelItem[] items = new TestDataExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new TestDataExcelItem();
			items[i].id = Convert.ToInt32(allItemValueRowList[i]["id"]);
			items[i].name = new List<string>(allItemValueRowList[i]["name"].Split(';'));
			items[i].value = new List<int>(Array.ConvertAll((allItemValueRowList[i]["value"]).Split(';'), int.Parse));
			items[i].dic = Convert.ToSingle(allItemValueRowList[i]["dic"]);
		}
		TestDataExcelData excelDataAsset = ScriptableObject.CreateInstance<TestDataExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(TestDataExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif



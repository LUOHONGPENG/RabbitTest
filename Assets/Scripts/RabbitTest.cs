using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTest : MonoBehaviour
{

    private void Start()
    {
        TestDataExcelData data1 = Singleton<ExcelManager>.Instance.GetExcelData<TestDataExcelData, TestDataExcelItem>();

        if (data1 != null)
        {
            for(int i = 0; i < data1.items.Length; i++)
            {
                Debug.Log(string.Join(",", data1.items[i].name));
            }
        }
    }


}

using UnityEngine;
using System.Collections.Generic;

namespace WQ.Core.Data
{
    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 2:40:42 PM
     * Description: 本地保存数据
    ****************************************************/
    public class LocalSaveData
    {
        //保存数据
        public static void SaveData(string data)
        {
            PlayerPrefs.SetString("data", data);
            PlayerPrefs.Save();
        }

        //获取数据
        public static string GetDate()
        {
            string data = string.Empty;
            data = PlayerPrefs.GetString("data", "");
            return data;
        }
    }
}

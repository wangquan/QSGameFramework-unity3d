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
#if UNITY_EDITOR || !UNITY_FLASH
            PlayerPrefs.SetString("data", data);
            PlayerPrefs.Save();
#else
            CallFlash.SaveData(data);
#endif
        }

        //获取数据
        public static string GetDate()
        {
            string data = string.Empty;
#if UNITY_EDITOR || !UNITY_FLASH
            data = PlayerPrefs.GetString("data", "");
#else
            data = CallFlash.GetData();
#endif
            return data;
        }
    }
}

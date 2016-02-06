using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using WQ.Core;

namespace WQ.Editor
{
    /****************************************************
     * Author: wq
     * Create Time: 2/4/2016 3:54:41 PM
     * Description: 通用工具集
    ****************************************************/
    public class GeneralTools
    {
        //清除本地存档
        [MenuItem("WQTools/ClearLocalData")]
        public static void ClearLocalData()
        {
            PlayerPrefs.DeleteAll();
            Debuger.Log("清除本地存档");
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace WQ.Core.Manager
{
    /****************************************************
     * Author: wq
     * Create Time: 2/2/2016 6:26:46 PM
     * Description: 界面管理器 单例
    ****************************************************/
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;//单例
        public static UIManager Instance
        {
            get
            {
                Regist();
                return _instance;
            }
        }

        //注册
        public static void Regist()
        {
            if (_instance == null)
                _instance = gbb.ManagerObject.AddComponent<UIManager>();
        }

        //唤醒阶段
        void Awake()
        {

        }

        //销毁
        void OnDestroy()
        {

        }
    }
}

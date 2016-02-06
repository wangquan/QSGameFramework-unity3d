using UnityEngine;
using System;
using System.Collections;

namespace WQ.Core.Manager
{
    /****************************************************
     * Author: wq
     * Create Time: 1/28/2016 3:05:59 PM
     * Description: 初始化管理器
    ****************************************************/    
    [AddComponentMenu("WQ/Core/Manager/InitManager")]
    public class InitManager : MonoBehaviour
    {

        public bool enableLog = true;//开关输出平台
        public int targetFrameRate = 45;//帧率
        public int sleepTimeout = -1;//休眠时间

        public UnityEngine.Object customInitObject;//自定义初始化对象

        //唤醒阶段
        void Awake()
        {
            Debuger.Log("=============开始初始化=============");
            initSettings();
            initManager();
        }

        //开始阶段
        void Start()
        {
            if (customInitObject != null)
            {
                Debuger.Log("自定义初始化");
                Instantiate(customInitObject, Vector3.zero, Quaternion.identity);
            }else
            {
                Debuger.Log("无自定义初始化，申请跳转login");
                gbb.LoadScene("login", null);
            }
        }

        //初始化设置
        private void initSettings()
        {
            Debuger.Log("初始化设置");
            Debuger.Enable = enableLog;
            UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
#if !UNITY_EDITOR
            Application.targetFrameRate = targetFrameRate;
            Screen.sleepTimeout = sleepTimeout;
#endif
        }

        //初始化管理器
        private void initManager()
        {
            Debuger.Log("初始化管理器");
            gbb.RegistManager();
        }

        //更新循环
        void Update()
        {

        }

        //销魂阶段
        void OnDestroy()
        {
            Debuger.Log("=============初始化完成=============");
        }

    }
}
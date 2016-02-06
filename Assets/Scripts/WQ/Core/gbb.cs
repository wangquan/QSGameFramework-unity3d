using UnityEngine;
using WQ.Core.Manager;
using WQ.Core.Tool;

namespace WQ.Core
{
    /****************************************************
     * Author: wq
     * Create Time: 1/28/2016 3:05:59 PM
     * Description: 全局黑板
    ****************************************************/
    public class gbb
    {
        /// <summary>
        /// 全局管理器对象
        /// </summary>
        private static GameObject _ManagerObject;
        public static GameObject ManagerObject
        {
            get
            {
                if (_ManagerObject == null)
                {
                    _ManagerObject = new GameObject("[Manager]");
                    GameObject.DontDestroyOnLoad(_ManagerObject);//注册为非销毁对象
                }
                return _ManagerObject;
            }
        }

        /// <summary>
        /// 注册全局管理器
        /// </summary>
        public static void RegistManager()
        {
            TimeManager.Regist();
            ResourcesManager.Regist();
            SoundManager.Regist();
            UIManager.Regist();
        }

        /// <summary>
        /// 时间管理器
        /// </summary>
        public static TimeManager GetTimeManager
        {
            get 
            {
                return TimeManager.Instance;
            }
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public static ResourcesManager GetResourcesManager
        {
            get
            {
                return ResourcesManager.Instance;
            }
        }

        /// <summary>
        /// 声音管理器
        /// </summary>
        public static SoundManager GetSoundManager
        {
            get
            {
                return SoundManager.Instance;
            }
        }

        /// <summary>
        /// 界面管理器
        /// </summary>
        public static UIManager GetUIManager
        {
            get
            {
                return UIManager.Instance;
            }
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        public static void LoadScene(string name, string[] paths)
        {
            LoadHelper.LoadScene(name, paths);
        }
    }
}

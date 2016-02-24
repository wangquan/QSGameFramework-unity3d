using UnityEngine;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/21/2016 9:12:11 PM
     * Description: 加载界面
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyLoad")]
    public class MyLoad : MonoBehaviour
    {
        private GameObject _gameObject;

        //唤醒阶段
        void Awake()
        {
            _gameObject = this.gameObject;
        }

        //显示
        public void Show()
        {
            if (_gameObject.activeSelf == false) _gameObject.SetActive(true);
        }

        //隐藏
        public void Hide()
        {
            if (_gameObject.activeSelf == true) _gameObject.SetActive(false);
        }

        //销毁
        void OnDestroy()
        {

        }
    }
}

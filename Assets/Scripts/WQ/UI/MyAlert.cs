using UnityEngine;
using System;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/21/2016 9:05:37 PM
     * Description: 警告界面
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyAlert")]
    public class MyAlert : MonoBehaviour
    {
        public UILabel label;
        public GameObject button;

        private GameObject _gameObject;
        private Action _callback;

        //唤醒阶段
        void Awake()
        {
            _gameObject = this.gameObject;
        }

        //显示
        public void Show(string text, Action callback = null)
        {
            label.text = text;
            _callback = callback;
            if (_gameObject.activeSelf == false) _gameObject.SetActive(true);
        }

        //隐藏
        public void Hide()
        {
            if (_gameObject.activeSelf == true) _gameObject.SetActive(false);
        }

        //确认按钮
        private void onConfirmButtonHandler()
        {
            if (_callback != null) _callback();
            Hide();
        }

        //销毁
        void OnDestroy()
        {

        }
    }
}

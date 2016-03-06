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
    public class MyAlert : MyBaseObject
    {
        public UILabel label;
        public UIPlayTween playTween;
        public GameObject confirmButton;

        private Action _confirmCallback;

        //唤醒阶段
        public override void Awake()
        {
            base.Awake();
            playTween.resetOnPlay = true;
            UIEventListener.Get(confirmButton).onClick += onConfirmClickHandler;
        }

        //显示
        public void Show(string text, Action confirmCallback = null)
        {
            myGameObject.SetActive(true);
            label.text = text;
            playTween.Play(true);
            this._confirmCallback = confirmCallback;
        }

        //隐藏
        public void Hide()
        {
            myGameObject.SetActive(false);
            _confirmCallback = null;
        }

        //确认处理
        private void onConfirmClickHandler(GameObject go)
        {
            if (_confirmCallback != null) _confirmCallback();
            Hide();
        }

        //销毁
        public override void OnDestroy()
        {
            base.OnDestroy();
            UIEventListener.Get(confirmButton).onClick -= onConfirmClickHandler;
            _confirmCallback = null;
        }
    }
}

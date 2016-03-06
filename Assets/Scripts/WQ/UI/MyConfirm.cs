using UnityEngine;
using System;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/21/2016 9:08:53 PM
     * Description: 确认框界面
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyConfirm")]
    public class MyConfirm : MyBaseObject
    {
        public UILabel label;
        public UIPlayTween playTween;
        public GameObject confirmButton;
        public GameObject cancelButton;

        private Action _confirmCallback;
        private Action _cancelCallback;

        //唤醒阶段
        public override void Awake()
        {
            base.Awake();
            playTween.resetOnPlay = true;
            UIEventListener.Get(confirmButton).onClick += onConfirmClickHandler;
            UIEventListener.Get(cancelButton).onClick += onCancelClickHandler;
        }

        //显示
        public void Show(string text, Action confirmCallback = null, Action cancelCallback = null)
        {
            myGameObject.SetActive(true);
            label.text = text;
            playTween.Play(true);
            this._confirmCallback = confirmCallback;
            this._cancelCallback = cancelCallback;
        }

        //隐藏
        public void Hide()
        {
            myGameObject.SetActive(false);
            _confirmCallback = null;
            _cancelCallback = null;
        }

        //确认处理
        private void onConfirmClickHandler(GameObject go)
        {
            if (_confirmCallback != null) _confirmCallback();
            Hide();
        }

        //取消处理
        private void onCancelClickHandler(GameObject go)
        {
            if (_cancelCallback != null) _cancelCallback();
            Hide();
        }

        //销毁
        public override void OnDestroy()
        {
            base.OnDestroy();
            UIEventListener.Get(confirmButton).onClick -= onConfirmClickHandler;
            UIEventListener.Get(cancelButton).onClick -= onCancelClickHandler;
            _confirmCallback = null;
            _cancelCallback = null;
        }
    }
}

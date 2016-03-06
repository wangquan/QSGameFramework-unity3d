using UnityEngine;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/21/2016 9:01:46 PM
     * Description: 提示界面
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyTips")]
    public class MyTips : MyBaseObject
    {
        public UILabel label;
        public UIPlayTween playTween;

        //唤醒阶段
        public override void Awake()
        {
            base.Awake();
            playTween.Play(true);
            playTween.onFinished.Add(new EventDelegate(onTweenFinishedHandler));
        }

        //设置提示信息
        public void SetText(string text)
        {
            label.text = text;
        }

        //动画播放完成事件
        private void onTweenFinishedHandler()
        {
            Destroy(myGameObject);
        }

        //销毁
        public override void OnDestroy()
        {
            base.OnDestroy();
            playTween.onFinished.Clear();
        }
    }
}

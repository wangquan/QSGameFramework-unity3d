using UnityEngine;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/22/2016 2:00:15 PM
     * Description: 停留提示界面
    ****************************************************/
    [AddComponentMenu("WQ/UI/MyStayTips")]
    public class MyStayTips : MyBaseObject
    {
        public UILabel label;
        public UIPlayTween playTween;

        //唤醒阶段
        public override void Awake()
        {
            base.Awake();
            playTween.resetOnPlay = true;
        }

        //显示
        public void Show(string text)
        {
            myGameObject.SetActive(true);
            label.text = text;
            playTween.Play(true);
        }

        //隐藏
        public void Hide()
        {
            myGameObject.SetActive(false);
        }
    }
}

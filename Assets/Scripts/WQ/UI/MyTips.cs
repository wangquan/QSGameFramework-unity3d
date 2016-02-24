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
    public class MyTips : MonoBehaviour
    {
        public UILabel label;

        //唤醒阶段
        void Awake()
        {

        }

        //设置提示信息
        public void SetText(string text)
        {
            label.text = text;
        }

        //销毁
        void OnDestroy()
        {

        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace WQ.UI
{
    /****************************************************
     * Author: wq
     * Create Time: 2/28/2016 11:09:17 PM
     * Description: 界面基础对象
    ****************************************************/
    public class MyBaseObject : MonoBehaviour
    {
        private GameObject _myGameObject;//通用组件
        private Transform _myTransform;

        public GameObject myGameObject { get { return _myGameObject; } }
        public Transform myTransform { get { return _myTransform; } }

        //唤醒阶段
        public virtual void Awake()
        {
            _myGameObject = this.gameObject;
            _myTransform = this.transform;
        }

        //销毁
        public virtual void OnDestroy()
        {
            _myGameObject = null;
            _myTransform = null;
        }
    }
}

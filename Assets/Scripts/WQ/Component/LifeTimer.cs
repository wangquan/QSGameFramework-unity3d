using UnityEngine;
using System.Collections.Generic;

namespace WQ.Component
{
    /****************************************************
     * Author: wq
     * Create Time: 6/10/2016 4:13:02 PM
     * Description: 存活计时器
    ****************************************************/
    [AddComponentMenu("WQ/Component/LifeTimer")]
    public class LifeTimer : MonoBehaviour
    {
        public float time;//存活时间

        //唤醒阶段
        void Awake()
        {
            Destroy(this.gameObject, time);
        }
    }
}

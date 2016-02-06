using UnityEngine;
using System;
using System.Collections.Generic;

namespace WQ.Core.Manager
{
    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 10:30:25 AM
     * Description: 时间管理器 单例
    ****************************************************/
    public class TimeManager : MonoBehaviour
    {
        private static TimeManager _instance;//单例
        public static TimeManager Instance
        {
            get
            {
                Regist();
                return _instance;
            }
        }

        public TimerManager timerManager;//全局计时器管理器

        //注册
        public static void Regist()
        {
            if (_instance == null) 
                _instance = gbb.ManagerObject.AddComponent<TimeManager>();
        }

        //唤醒阶段
        void Awake()
        {
            timerManager = new TimerManager();
        }

        //循环更新
        void Update()
        {
            timerManager.Update(Time.unscaledDeltaTime);
        }

        //销毁
        void OnDestroy()
        {
            _instance = null;
            timerManager.RemoveAll();
        }
    }

    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 10:30:25 AM
     * Description: 计时器状态类型
    ****************************************************/
    public enum TimerStateType
    {
        Normal = 0,//正常
        Pause,//暂停
        Stop//停止 此类型会申请销毁
    }

    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 10:30:25 AM
     * Description: 计时器
    ****************************************************/
    public class Timer
    {
        public TimerStateType state;//计时器状态
        public int times;//执行次数 0-N次 -1无限次
        public float interval;//执行间隔
        public Action<Timer> callback;//回调方法

        private float _timer;//计时

        public Timer(Action<Timer> callback, float interval, int times, TimerStateType state = TimerStateType.Normal)
        {
            this.callback = callback;
            this.interval = interval;
            this.times = times;
            this.state = state;

            this._timer = 0.0f;
        }

        public TimerStateType Tick(float deltaTime)
        {
            if (state == TimerStateType.Normal)
            {
                if (times != 0)
                {
                    _timer += deltaTime;
                    if (_timer >= interval)
                    {
                        _timer = 0.0f;
                        if (times > 0) times--;
                        if (callback != null) callback(this);
                    }
                }else
                {
                    state = TimerStateType.Stop;
                }
            }
            return state;
        }
    }

    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 10:30:25 AM
     * Description: 计时器管理器
    ****************************************************/
    public class TimerManager
    {
        private bool _isPause;//暂停开关
        private List<Timer> _timers;//计时器列表

        public int count { get { return _timers.Count;} }//获取计时器数量

        public TimerManager()
        {
            _isPause = false;
            _timers = new List<Timer>();
        }

        //注册一个计时器
        public Timer Regist(Timer timer)
        {
            _timers.Add(timer);
            return timer;
        }

        public Timer Regist(Action<Timer> callback, float interval, int times, TimerStateType state = TimerStateType.Normal)
        {
            Timer timer = new Timer(callback, interval, times, state);
            _timers.Add(timer);
            return timer;
        }

        public Timer Regist(Action<Timer> callback, float interval)
        {
            return Regist(callback, interval, 1);
        }

        //移除一个计时器
        public void Remove(Timer timer)
        {
            if (_timers.Contains(timer)) _timers.Remove(timer);
        }

        public void Remove(Action<Timer> callback)
        {
            for(int i = _timers.Count - 1; i >= 0; i--)
            {
                if (_timers[i].callback == callback)
                {
                    _timers.Remove(_timers[i]);
                }
            }
        }

        //移除所有计时器
        public void RemoveAll()
        {
            _timers.Clear();
        }

        //播放
        public void Play()
        {
            _isPause = false;
        }
        
        //暂停
        public void Pause()
        {
            _isPause = true;
        }

        //更新循环
        public void Update(float deltaTime)
        {
            if (_isPause) return;
            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                if (_timers[i].Tick(deltaTime) == TimerStateType.Stop)
                {
                    _timers.Remove(_timers[i]);
                }
            }
        }

    }
}

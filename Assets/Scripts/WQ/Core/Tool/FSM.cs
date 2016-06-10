using System;
using System.Collections.Generic;

namespace WQ.Core.Tool
{
    /****************************************************
     * Author: wq
     * Create Time: 2/25/2016 11:10:06 AM
     * Description: 消息
    ****************************************************/
    public class FSMMessage
    {
        public int id;//消息标识
        public object obj;//消息体

        public FSMMessage(int id, object obj)
        {
            this.id = id;
            this.obj = obj;
        }
    }

    /****************************************************
     * Author: wq
     * Create Time: 2/25/2016 11:10:06 AM
     * Description: 状态基类 T转换条件 S转换状态
    ****************************************************/
    public abstract class FSMState<T,S>
        where T : struct
        where S : struct
    {
        protected Dictionary<T, S> map = new Dictionary<T, S>();//转换字典
        protected S id;//状态标识
        public S ID { get { return id; } }

        //添加转换
        public void AddTransition(T t, S s)
        {
            if (map.ContainsKey(t))
            {
                Debuger.Log("[FSM]ContainsKey:" + t.ToString());
                return;
            }
            map.Add(t, s);
        }

        //移除转换
        public void RemoveTransition(T t)
        {
            if (map.ContainsKey(t))
            {
                map.Remove(t);
                return;
            }
            Debuger.Log("[FSM]Not ContainsKey:" + t.ToString());
        }
        
        //获取转换状态
        public S GetTransitionState(T t)
        {
            if (map.ContainsKey(t))
            {
                return map[t];
            }
            return default(S);
        }

        //进入
        public abstract void Enter();
        //执行
        public abstract void Execute(float deltaTime);
        //退出
        public abstract void Exit();
        //消息处理
        public abstract bool MessageHandler(FSMMessage message);
    }

    /****************************************************
     * Author: wq
     * Create Time: 2/25/2016 11:10:06 AM
     * Description: 状态系统 F状态 T转换条件 S转换状态
    ****************************************************/
    public sealed class FSMSystem<F,T,S>
        where F : FSMState<T,S>
        where T : struct
        where S : struct
    {
        private List<F> _states = new List<F>();//状态列表
        private F _globalState;//全局状态
        private F _currentState;//当前状态
        private F _previousState;//前一状态

        public F globalState 
        {
            set { _globalState = value; _globalState.Enter(); }
            get { return _globalState; } 
        }
        public F currentState 
        {
            set { _currentState = value; _currentState.Enter(); }
            get { return _currentState; } 
        }
        public F previousState
        {
            set { _previousState = value; }
            get { return _previousState; } 
        }


        //更新循环
        public void Update(float deltaTime)
        {
            if (_globalState != null) _globalState.Execute(deltaTime);
            if (_currentState != null) _currentState.Execute(deltaTime);
        }

        //添加一个状态
        public void AddState(F f)
        {
            for (int i = _states.Count - 1; i >= 0; i--)
            {
                if (_states[i].ID.Equals(f.ID))
                {
                    Debuger.Log("[FSM]ContainsState:" + f.ID.ToString());
                    return;
                }
            }
            _states.Add(f);
        }

        //移除一个状态
        public void RemoveState(S s)
        {
            for (int i = _states.Count - 1; i >= 0; i--)
            {
                if (_states[i].ID.Equals(s))
                {
                    _states.RemoveAt(i);
                    return;
                }
            }
            Debuger.Log("[FSM]Not ContainsState:" + s.ToString());
        }

        //改变当前状态
        private void changeCurrentState(F f)
        {
            _previousState = _currentState;//记录为前一状态
            _currentState.Exit();//执行退出
            _currentState = f;//改变
            _currentState.Enter();//执行进入
        }

        //执行转换
        public void PerformTransition(T t)
        {
            S s = _currentState.GetTransitionState(t);
            for (int i = _states.Count - 1; i >= 0; i--)
            {
                if (_states[i].ID.Equals(s))
                {
                    changeCurrentState(_states[i]);
                    return;
                }
            }
        }

        //回到前一状态
        public void RevertToPreviousState()
        {
            if (_previousState != null) changeCurrentState(_previousState);
        }

        //消息处理
        public bool MessageHandler(FSMMessage message)
        {
            if (_currentState != null && _currentState.MessageHandler(message))
            {
                return true;
            }
            if (_globalState != null && _globalState.MessageHandler(message))
            {
                return true;
            }
            return false;
        }

    }

}

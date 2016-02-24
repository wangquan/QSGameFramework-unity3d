using UnityEngine;
using System.Collections.Generic;
using WQ.Core.Data;

namespace WQ.Core.Manager
{
    /****************************************************
     * Author: wq
     * Create Time: 1/29/2016 10:58:06 PM
     * Description: 声音管理器 单例
    ****************************************************/
    public class SoundManager : MonoBehaviour
    {
        private const int BUFFER_LENGTH_LIMIT = 5;//缓存数量限制

        private static SoundManager _instance;//单例
        public static SoundManager Instance
        {
            get
            {
                Regist();
                return _instance;
            }
        }

        private AudioSource _bgSource;//背景音道
        private AudioSource[] _efSources;//音效音道组

        private Asset _bgAssetBuffer;//缓存资源
        private List<Asset> _efAssetBuffers;

        private float _bgVolume;//背景音量
        public float bgVolume
        {
            get { return _bgVolume; }
            set
            {
                _bgVolume = value;
                if (_bgVolume < 0) _bgVolume = 0;
                else if (_bgVolume > 1) _bgVolume = 1;
                _bgSource.volume = _bgVolume;
            }
        }
        private float _efVolume;//特效音量
        public float efVolume
        {
            get { return _efVolume; }
            set
            {
                _efVolume = value;
                if (_efVolume < 0) _efVolume = 0;
                else if (_efVolume > 1) _efVolume = 1;
                for (int i = 0; i < _efSources.Length; i++)
                {
                    _efSources[i].volume = _efVolume;
                }
            }
        }

        //注册
        public static void Regist()
        {
            if (_instance == null)
                _instance = gbb.ManagerObject.AddComponent<SoundManager>();
        }

        //唤醒阶段
        void Awake()
        {
            _bgSource = gameObject.AddComponent<AudioSource>();
            _bgSource.loop = true;
            _efSources = new AudioSource[5];
            for (int i = 0; i < _efSources.Length; i++)
            {
                _efSources[i] = gameObject.AddComponent<AudioSource>();
            }
            gameObject.AddComponent<AudioListener>();

            _bgAssetBuffer = null;
            _efAssetBuffers = new List<Asset>();

            DefaultSet();
        }

        //默认设置
        public void DefaultSet()
        {
            bgVolume = 0.6f;
            efVolume = 0.8f;
        }

        //整体控制
        public void Play()
        {
            PlayBG();
            PlayEF();
        }

        public void Pause()
        {
            PauseBG();
            PauseEF();
        }

        public void Stop()
        {
            StopBG();
            StopEF();
        }

        public void Clear()
        {
            ClearBG();
            ClearEF();
        }

        //背景音
        public void PlayBG()
        {
            if (bgVolume > 0 && _bgSource.clip != null) _bgSource.Play();
        }

        public void PlayBG(AudioClip clip)
        {
            _bgSource.clip = clip;
            if (bgVolume > 0) _bgSource.Play();
        }

        public void PlayBG(string name)
        {
            string path = PathData.SOUND_PATH + name;
            if (_bgAssetBuffer != null && _bgAssetBuffer.path == path)
            {
                PlayBG(_bgAssetBuffer.GetObject<AudioClip>());
            }else
            {
                gbb.GetResourcesManager.LoadAsync<AudioClip>(path, (asset) => { 
                    if (_bgAssetBuffer != null)
                    {
                        gbb.GetResourcesManager.Remove(_bgAssetBuffer.path);
                    }
                    _bgAssetBuffer = asset;
                    PlayBG(asset.GetObject<AudioClip>());
                });
            }
        }

        public void PauseBG()
        {
            if (_bgSource.isPlaying) _bgSource.Pause();
        }

        public void StopBG()
        {
            if (_bgSource.isPlaying) _bgSource.Stop();
        }

        public void ClearBG()
        {
            if (_bgSource.isPlaying) _bgSource.Stop();
            _bgSource.clip = null;
        }

        //音效
        public void PlayEF()
        {

        }

        public void PlayEF(AudioClip clip)
        {
            if (efVolume > 0)
            {
                foreach (AudioSource auds in _efSources)
                {
                    if (!auds.isPlaying)
                    {
                        auds.clip = clip;
                        auds.Play();
                        return;
                    }
                }
            }
        }

        public void PlayEF(string name)
        {
            string path = PathData.SOUND_PATH + name;
            PlayEF(GetAssetBuffer(path).GetObject<AudioClip>());
        }

        public void PauseEF()
        {
            foreach(AudioSource auds in _efSources)
            {
                if (auds.isPlaying) auds.Pause();
            }
        }

        public void StopEF()
        {
            foreach (AudioSource auds in _efSources)
            {
                if (auds.isPlaying) auds.Stop();
            }
        }

        public void ClearEF()
        {
            foreach (AudioSource auds in _efSources)
            {
                if (auds.isPlaying) auds.Stop();
                auds.clip = null;
            }
        }

        //取出缓存资源 无就申请加载
        private Asset GetAssetBuffer(string path)
        {
            Asset asset = null;
            for (int i = _efAssetBuffers.Count - 1; i >= 0; i--)
            {
                if (_efAssetBuffers[i].path == path)
                {
                    asset = _efAssetBuffers[i];
                    break;
                }
            }
            if (asset == null)
            {
                asset = gbb.GetResourcesManager.Load<AudioClip>(path);
                _efAssetBuffers.Add(asset);
                if (_efAssetBuffers.Count > BUFFER_LENGTH_LIMIT)
                {
                    gbb.GetResourcesManager.Remove(_efAssetBuffers[0].path);
                    _efAssetBuffers.RemoveAt(0);
                }
            }
            return asset;
        }

        //销毁
        void OnDestroy()
        {
            _instance = null;
        }
    }
}

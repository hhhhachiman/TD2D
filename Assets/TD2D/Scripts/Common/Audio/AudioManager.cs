using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lemon.audio
{
    public class AudioManager : MonoBehaviour
    {
        AudioSourceManager audioSourceManager;
        ClipsManager clipsManager;
        public static AudioManager Instance;
       

        void Awake()
        {
            Instance = this;
            audioSourceManager = new AudioSourceManager(this.gameObject);
            clipsManager = new ClipsManager();
        }
        public void Play(string audioName)
        {
            //拿一个空闲 AudioSource
            AudioSource tempSource = audioSourceManager.GetFreeAudio();
            //根据 audioName参数 获取对应的 AudioClip
            SingleClip tempClip = clipsManager.FindClipByName(audioName);
            //播放
            tempClip.Play(tempSource);
        }
        // 停止 某音效
        public void Stop(string audioName)
        {
            audioSourceManager.Stop(audioName);
        }


    }

}

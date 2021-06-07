using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.audio
{
    /// <summary>
    /// 1、从audioSources 拿出一个空闲的 AudioSource
    /// 2、释放多余AudioSource
    /// 3、播放一个音效
    /// 4、停止播放一个音效
    /// </summary>
    class AudioSourceManager
    {
        List<AudioSource> audioSources;
        GameObject ower;

        // 构造，确定挂载 AudioSource 组件的对象
        public AudioSourceManager(GameObject tempOwer)
        {
            ower = tempOwer;
            InitAS();
        }

        //初始化 生成三个 AudioSource
        public void InitAS()
        {

            audioSources = new List<AudioSource>();
            for (int i = 0; i < 3; i++)
            {
                AudioSource tempSource = ower.AddComponent<AudioSource>();
                tempSource.loop = false;
                audioSources.Add(tempSource);
            }
        }


        // 停止 某音效的 播放
        public void Stop(string audioName)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].isPlaying && audioSources[i].clip.name.Equals(audioName))
                {
                    audioSources[i].Stop();
                }
            }
        }

        // 得到一个 空闲AudioSource， 如果没有则 新建一个
        public AudioSource GetFreeAudio()
        {

            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                  //  isGet = true;
                    return audioSources[i];
                }
            }
            //上面无法返回freeAudio

            AudioSource tempAudio = ower.AddComponent<AudioSource>();
            audioSources.Add(tempAudio);
            return tempAudio;
        }


        /// <summary>
        /// 释放多余 freeaudio ，可设置 阈值
        /// </summary>
        public void ReleaseFreeAudio()
        {
            List<AudioSource> tempSources = new List<AudioSource>();
            int tempCount = 0;

            //遍历找出freeAudio
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    tempCount++;

                    if (tempCount > 3)
                    {
                        tempSources.Add(audioSources[i]);
                    }
                }
            }

            //释放freeAudio
            for (int i = 0; i < tempSources.Count; i++)
            {
                AudioSource tempSource = tempSources[i];
                audioSources.Remove(tempSource);
                GameObject.Destroy(tempSource);
            }

            tempSources.Clear();
            tempSources = null;

        }
    }
}






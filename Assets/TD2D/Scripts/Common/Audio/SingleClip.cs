using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lemon.audio
{
    class SingleClip
    {
        AudioClip myClip;

        // 构造函数 
        public SingleClip(AudioClip tempClip)
        {
            myClip = tempClip;
        }

        /// <summary>
        /// 调用 一个 AudioSource， 来播放 当前 AudioClip
        /// </summary>
        /// <param name="tempSource"></param>
        public void Play(AudioSource tempSource)
        {
            tempSource.clip = myClip;
            tempSource.Play();
        }
    }
}


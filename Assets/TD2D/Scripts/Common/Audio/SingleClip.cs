using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lemon.audio
{
    class SingleClip
    {
        AudioClip myClip;

        // ���캯�� 
        public SingleClip(AudioClip tempClip)
        {
            myClip = tempClip;
        }

        /// <summary>
        /// ���� һ�� AudioSource�� ������ ��ǰ AudioClip
        /// </summary>
        /// <param name="tempSource"></param>
        public void Play(AudioSource tempSource)
        {
            tempSource.clip = myClip;
            tempSource.Play();
        }
    }
}


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
            //��һ������ AudioSource
            AudioSource tempSource = audioSourceManager.GetFreeAudio();
            //���� audioName���� ��ȡ��Ӧ�� AudioClip
            SingleClip tempClip = clipsManager.FindClipByName(audioName);
            //����
            tempClip.Play(tempSource);
        }
        // ֹͣ ĳ��Ч
        public void Stop(string audioName)
        {
            audioSourceManager.Stop(audioName);
        }


    }

}

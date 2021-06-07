using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.audio
{
    /// <summary>
    /// 1����audioSources �ó�һ�����е� AudioSource
    /// 2���ͷŶ���AudioSource
    /// 3������һ����Ч
    /// 4��ֹͣ����һ����Ч
    /// </summary>
    class AudioSourceManager
    {
        List<AudioSource> audioSources;
        GameObject ower;

        // ���죬ȷ������ AudioSource ����Ķ���
        public AudioSourceManager(GameObject tempOwer)
        {
            ower = tempOwer;
            InitAS();
        }

        //��ʼ�� �������� AudioSource
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


        // ֹͣ ĳ��Ч�� ����
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

        // �õ�һ�� ����AudioSource�� ���û���� �½�һ��
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
            //�����޷�����freeAudio

            AudioSource tempAudio = ower.AddComponent<AudioSource>();
            audioSources.Add(tempAudio);
            return tempAudio;
        }


        /// <summary>
        /// �ͷŶ��� freeaudio �������� ��ֵ
        /// </summary>
        public void ReleaseFreeAudio()
        {
            List<AudioSource> tempSources = new List<AudioSource>();
            int tempCount = 0;

            //�����ҳ�freeAudio
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

            //�ͷ�freeAudio
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






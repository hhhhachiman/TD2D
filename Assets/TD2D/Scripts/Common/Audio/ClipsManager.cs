using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lemon.audio
{

    class ClipsManager
    {
        //�������ļ��м��� Clips�ļ�
        string[] clipNames;
        SingleClip[] allSingleClips;

        public ClipsManager()
        {
            ReadConfig();
            LoadClips();
        }

        /// <summary>
        /// ��ȡ�����ļ�
        /// �ļ���Ҫ���� StreamingAssets�ļ���
        /// </summary>
        public void ReadConfig()
        {
            //��ȡ·���� ClipName.txt�����Ǳ�����Ч���ֵ� txt�ļ���
            var fileAddress = System.IO.Path.Combine(
                Application.streamingAssetsPath, "ClipNames.txt");
            FileInfo file = new FileInfo(fileAddress);

           // string s = " ";
            if (file.Exists)
            {
                StreamReader r = new StreamReader(fileAddress);

                string tempLine = "";

                tempLine = r.ReadLine();//����һ�У���� tempLine ����Ч

                int lineCount = 0;
                if (int.TryParse(tempLine, out lineCount))
                {
                    clipNames = new string[lineCount]; // ��ʼ�� clipNames
                    for (int i = 0; i < lineCount; i++)
                    {
                        tempLine = r.ReadLine();
                        //�ָ�����
                        string[] splits = tempLine.Split(" ".ToCharArray());

                        clipNames[i] = splits[0];// ���Ӻ�׺������ǰ׺����
                    }
                }
                r.Close(); //�ͷ�
            }
            else Debug.Log("can not find wenjian ");
        }

        /// <summary>
        /// ����clips���ڴ�
        /// ���ص��ļ������ Resources �ļ��� ������Ҫ��׺
        /// </summary>
        public void LoadClips()
        {
            allSingleClips = new SingleClip[clipNames.Length];
            for (int i = 0; i < clipNames.Length; i++)
            {
                //������Դ�� ע��·�� �ǿɱ��
                AudioClip tempClip = Resources.Load<AudioClip>("Sounds/" + clipNames[i]);
                //Debug.Log(tempClip.name);
                SingleClip tempSingle = new SingleClip(tempClip);
                allSingleClips[i] = tempSingle;
            }
        }

        // �������ֲ��� SingleClip
        public SingleClip FindClipByName(string clipName)
        {
            int tempIndex = -1;
            for (int i = 0; i < clipNames.Length; i++)
            {
                if (clipNames[i].Equals(clipName))
                {
                    tempIndex = i;
                    break;
                }
            }
            //����
            if (tempIndex != -1)
            {
                return allSingleClips[tempIndex];

            }
            else
            {
                Debug.Log("can not find the clip");
                return null;
            }
        }
    }
}


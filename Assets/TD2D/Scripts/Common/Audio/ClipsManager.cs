using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lemon.audio
{

    class ClipsManager
    {
        //从配置文件中加载 Clips文件
        string[] clipNames;
        SingleClip[] allSingleClips;

        public ClipsManager()
        {
            ReadConfig();
            LoadClips();
        }

        /// <summary>
        /// 读取配置文件
        /// 文件需要放在 StreamingAssets文件下
        /// </summary>
        public void ReadConfig()
        {
            //获取路径， ClipName.txt是我们保存音效名字的 txt文件。
            var fileAddress = System.IO.Path.Combine(
                Application.streamingAssetsPath, "ClipNames.txt");
            FileInfo file = new FileInfo(fileAddress);

           // string s = " ";
            if (file.Exists)
            {
                StreamReader r = new StreamReader(fileAddress);

                string tempLine = "";

                tempLine = r.ReadLine();//读第一行，存放 tempLine 个音效

                int lineCount = 0;
                if (int.TryParse(tempLine, out lineCount))
                {
                    clipNames = new string[lineCount]; // 初始化 clipNames
                    for (int i = 0; i < lineCount; i++)
                    {
                        tempLine = r.ReadLine();
                        //分割数组
                        string[] splits = tempLine.Split(" ".ToCharArray());

                        clipNames[i] = splits[0];// 忽视后缀，保存前缀即可
                    }
                }
                r.Close(); //释放
            }
            else Debug.Log("can not find wenjian ");
        }

        /// <summary>
        /// 加载clips到内存
        /// 加载的文件需放在 Resources 文件下 ，不需要后缀
        /// </summary>
        public void LoadClips()
        {
            allSingleClips = new SingleClip[clipNames.Length];
            for (int i = 0; i < clipNames.Length; i++)
            {
                //加载资源， 注意路径 是可变的
                AudioClip tempClip = Resources.Load<AudioClip>("Sounds/" + clipNames[i]);
                //Debug.Log(tempClip.name);
                SingleClip tempSingle = new SingleClip(tempClip);
                allSingleClips[i] = tempSingle;
            }
        }

        // 按照名字查找 SingleClip
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
            //返回
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


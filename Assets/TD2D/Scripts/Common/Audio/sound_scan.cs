using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sound_scan : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        //�̶�һ������ȥɨ�裬ÿ��0.5sɨ��һ��
        this.InvokeRepeating("scan", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //��ʱ������
    void scan()
    {
        SoundManager.disable_over_audio();//��������AudioSource����ӿ�
    }
}
